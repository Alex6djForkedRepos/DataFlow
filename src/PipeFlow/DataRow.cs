// CA1710: DataRow is an intentional domain-model name; renaming to DataRowDictionary
// or DataRowCollection would break the established public API contract.
#pragma warning disable CA1710
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PipeFlow;

/// <summary>
/// A case-insensitive, order-preserving bag of named column values. Used as the
/// generic record type for source-agnostic pipeline operations.
/// </summary>
/// <remarks>
/// v3 changes versus v2:
/// <list type="bullet">
///   <item>Getter returns <c>null</c> on missing column (v2 threw).</item>
///   <item>Structural <see cref="IEquatable{T}"/>; <c>Distinct()</c>/<c>GroupBy</c> work.</item>
///   <item>Type conversion uses <see cref="CultureInfo.InvariantCulture"/>.</item>
/// </list>
/// </remarks>
public sealed class DataRow : IReadOnlyDictionary<string, object?>, IEquatable<DataRow>
{
    private readonly Dictionary<string, object?> _data;
    private readonly List<string> _columnOrder;
    private int? _cachedHashCode;

    public DataRow() : this(capacity: 16) { }

    public DataRow(int capacity)
    {
        _data = new Dictionary<string, object?>(capacity, StringComparer.OrdinalIgnoreCase);
        _columnOrder = new List<string>(capacity);
    }

    public DataRow(IEnumerable<KeyValuePair<string, object?>> source) : this()
    {
        ArgumentNullException.ThrowIfNull(source);
        foreach (var kvp in source)
            this[kvp.Key] = kvp.Value;
    }

    public object? this[string columnName]
    {
        get => _data.TryGetValue(columnName, out var value) ? value : null;
        set
        {
            if (!_data.ContainsKey(columnName))
                _columnOrder.Add(columnName);
            _data[columnName] = value;
            _cachedHashCode = null;
        }
    }

    public object? this[int columnIndex]
    {
        get
        {
            if ((uint)columnIndex >= (uint)_columnOrder.Count)
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            return _data[_columnOrder[columnIndex]];
        }
        set
        {
            if ((uint)columnIndex >= (uint)_columnOrder.Count)
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            _data[_columnOrder[columnIndex]] = value;
            _cachedHashCode = null;
        }
    }

    public int ColumnCount => _columnOrder.Count;
    public IEnumerable<string> Columns => _columnOrder;

    public bool ContainsColumn(string columnName) => _data.ContainsKey(columnName);

    public bool Remove(string columnName)
    {
        if (!_data.Remove(columnName))
            return false;
        _columnOrder.RemoveAll(c => c.Equals(columnName, StringComparison.OrdinalIgnoreCase));
        _cachedHashCode = null;
        return true;
    }

    public DataRow Clone()
    {
        var clone = new DataRow(_columnOrder.Count);
        foreach (var col in _columnOrder)
            clone[col] = _data[col];
        return clone;
    }

    /// <summary>
    /// Returns the column value as type <typeparamref name="T"/>.
    /// Missing columns and null values return <c>default</c>.
    /// Conversions use <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    public T? GetValue<T>(string columnName)
    {
        if (!_data.TryGetValue(columnName, out var value) || value is null)
            return default;

        if (value is T typed)
            return typed;

        // Unwrap Nullable<T> - Convert.ChangeType doesn't handle Nullable<> directly.
        var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        try
        {
            return (T)Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
        }
        catch (Exception ex) when (ex is InvalidCastException or FormatException or OverflowException)
        {
            return default;
        }
    }

    /// <summary>
    /// Tries to return the column value as type <typeparamref name="T"/>.
    /// Returns <c>false</c> if the column is missing, null, or unconvertible.
    /// Conversions use <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    public bool TryGetValue<T>(string columnName, out T? value)
    {
        value = default;

        if (!_data.TryGetValue(columnName, out var raw) || raw is null)
            return false;

        if (raw is T typed)
        {
            value = typed;
            return true;
        }

        var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        try
        {
            value = (T)Convert.ChangeType(raw, targetType, CultureInfo.InvariantCulture);
            return true;
        }
        catch (Exception ex) when (ex is InvalidCastException or FormatException or OverflowException)
        {
            return false;
        }
    }

    // ---- IReadOnlyDictionary<string, object?> explicit implementation ----

    IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys => _columnOrder;

    IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values
    {
        get
        {
            foreach (var col in _columnOrder)
                yield return _data[col];
        }
    }

    int IReadOnlyCollection<KeyValuePair<string, object?>>.Count => _data.Count;

    bool IReadOnlyDictionary<string, object?>.ContainsKey(string key) => _data.ContainsKey(key);

    bool IReadOnlyDictionary<string, object?>.TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
        => _data.TryGetValue(key, out value);

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        foreach (var col in _columnOrder)
            yield return new KeyValuePair<string, object?>(col, _data[col]);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // ---- Structural equality ----

    /// <inheritdoc/>
    public bool Equals(DataRow? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        if (_data.Count != other._data.Count)
            return false;

        foreach (var kvp in _data)
        {
            if (!other._data.TryGetValue(kvp.Key, out var otherValue))
                return false;
            if (!Equals(kvp.Value, otherValue))
                return false;
        }
        return true;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is DataRow other && Equals(other);

    /// <summary>
    /// Returns a cached, order-independent hash code based on all column names and values.
    /// The cache is invalidated on any mutation.
    /// </summary>
    public override int GetHashCode()
    {
        if (_cachedHashCode is int cached)
            return cached;

        var hash = new HashCode();
        // Order-independent: sort keys by OrdinalIgnoreCase so identical content
        // with different insertion order produces equal hashes.
        foreach (var key in _columnOrder
            .OrderBy(k => k, StringComparer.OrdinalIgnoreCase))
        {
            hash.Add(key, StringComparer.OrdinalIgnoreCase);
            hash.Add(_data[key]);
        }

        var result = hash.ToHashCode();
        _cachedHashCode = result;
        return result;
    }

    /// <summary>Structural equality operator.</summary>
    public static bool operator ==(DataRow? left, DataRow? right)
        => left is null ? right is null : left.Equals(right);

    /// <summary>Structural inequality operator.</summary>
    public static bool operator !=(DataRow? left, DataRow? right) => !(left == right);
}
