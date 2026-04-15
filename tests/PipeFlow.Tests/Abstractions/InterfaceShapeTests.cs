using PipeFlow.Abstractions;

namespace PipeFlow.Tests.Abstractions;

public class InterfaceShapeTests
{
    [Fact]
    public void IPipeline_IsGeneric()
    {
        typeof(IPipeline<>).IsGenericTypeDefinition.Should().BeTrue();
    }

    [Fact]
    public void IPipelineSource_IsCovariant()
    {
        var sourceParam = typeof(IPipelineSource<>).GetGenericArguments()[0];
        sourceParam.GenericParameterAttributes
            .HasFlag(System.Reflection.GenericParameterAttributes.Covariant)
            .Should().BeTrue();
    }

    [Fact]
    public void IPipelineSink_IsContravariant()
    {
        var sinkParam = typeof(IPipelineSink<>).GetGenericArguments()[0];
        sinkParam.GenericParameterAttributes
            .HasFlag(System.Reflection.GenericParameterAttributes.Contravariant)
            .Should().BeTrue();
    }

    [Fact]
    public void IOrderedPipeline_ExtendsIPipeline()
    {
        typeof(IOrderedPipeline<>).GetInterfaces()
            .Should().Contain(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipeline<>));
    }

    [Fact]
    public void IQueryablePipeline_ExtendsIPipeline()
    {
        typeof(IQueryablePipeline<>).GetInterfaces()
            .Should().Contain(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipeline<>));
    }

    [Fact]
    public void IOrderedQueryablePipeline_ExtendsBothQueryableAndOrdered()
    {
        var interfaces = typeof(IOrderedQueryablePipeline<>).GetInterfaces();
        interfaces.Should().Contain(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryablePipeline<>));
        interfaces.Should().Contain(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IOrderedPipeline<>));
    }

    [Fact]
    public void PipelineContext_IsReadOnlyStruct()
    {
        typeof(PipelineContext).IsValueType.Should().BeTrue();
        typeof(PipelineContext).GetCustomAttributes(typeof(System.Runtime.CompilerServices.IsReadOnlyAttribute), inherit: false)
            .Should().NotBeEmpty();
    }

    [Fact]
    public void PipelineContext_Empty_IsValid()
    {
        var empty = PipelineContext.Empty;
        empty.Logger.Should().NotBeNull();
        empty.Options.Should().NotBeNull();
        empty.HttpClientFactory.Should().BeNull();
        empty.Services.Should().BeNull();
        empty.CancellationToken.Should().Be(CancellationToken.None);
    }
}
