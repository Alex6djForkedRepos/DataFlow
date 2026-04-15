# PipeFlow

[![NuGet](https://img.shields.io/nuget/v/PipeFlowCore.svg)](https://www.nuget.org/packages/PipeFlowCore)
[![Build](https://github.com/Nonanti/PipeFlow/actions/workflows/build.yml/badge.svg)](https://github.com/Nonanti/PipeFlow/actions)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8%20%7C%2010-512BD4)](https://dotnet.microsoft.com/download)

A .NET ETL pipeline library. Read, transform, and write data with a fluent, async-first API.

> **v3 is under active development on this branch.** The stable v2.1.0 package is
> still the one on NuGet as `PipeFlowCore`. v3 will ship split across
> `PipeFlow` + `PipeFlow.SqlServer` + `PipeFlow.PostgreSql` + `PipeFlow.MongoDb` +
> `PipeFlow.Excel` + `PipeFlow.EntityFrameworkCore` + `PipeFlow.Aws` +
> `PipeFlow.Azure` + `PipeFlow.GoogleCloud`. Until then use v2 or build from source.

## Install (v2, current stable)

```bash
dotnet add package PipeFlowCore
```

The v2 source lives at tag [`v2.1.0-archived`](https://github.com/Nonanti/PipeFlow/tree/v2.1.0-archived).

## Quick start (v3, from source)

```csharp
using PipeFlow;

// DataRow is a case-insensitive, structurally-equal row type
var row = new DataRow
{
    ["Id"] = 1,
    ["Email"] = "alice@example.com",
    ["Age"] = "30",
};

// InvariantCulture type conversion with Nullable<T> support
int id = row.GetValue<int>("Id");
int? age = row.GetValue<int?>("Age");
string email = row.GetValue<string>("EMAIL")!; // key lookup is case-insensitive
```

The pipeline, source, and sink abstractions are defined; concrete CSV / JSON / HTTP /
database sources land in Wave 0.B and beyond. See `samples/PipeFlow.Sandbox/Program.cs`
for every public API exercised end-to-end.

## Build and test locally

Requires the .NET 10 SDK (also builds for net8.0).

```bash
dotnet build
dotnet test
dotnet run --project samples/PipeFlow.Sandbox -c Release
```

## Status

- Core abstractions, `DataRow`, exception hierarchy: done (Wave 0.A).
- `Pipeline<T>`, `ParallelPipeline<T>`, builder, DI, options: next (Wave 0.B).
- Source/sink concrete implementations (CSV, JSON, HTTP, DB, cloud): Wave 0.C onward.
- NuGet release as split packages: v3.0.0 GA.

## Contributing

PRs welcome. See [CONTRIBUTING.md](CONTRIBUTING.md). Run `dotnet format` before committing.

## License

MIT. See [LICENSE](LICENSE).

## Author

Berkant - [Nonanti](https://github.com/Nonanti)
