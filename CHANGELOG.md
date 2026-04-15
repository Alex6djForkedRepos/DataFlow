# Changelog

All notable changes to PipeFlow will be documented in this file.

## [Unreleased]

### Wave 0.A -- Foundation

- Archive v2.1.0 as git tag `v2.1.0-archived`
- Reorganize repository into SLNX solution + Central Package Management + shared MSBuild props
- Multi-target `net8.0` and `net10.0`
- Introduce core abstractions: `IPipelineSource<T>`, `IPipelineSink<T>`, `IPipeline<T>`, `IOrderedPipeline<T>`, `IQueryablePipeline<T>`, `IOrderedQueryablePipeline<T>`
- Redesign `DataRow` with structural `IEquatable<DataRow>` + `GetHashCode` caching
- Structured exception hierarchy: `PipeFlowException`, `PipeFlowSourceException`, `PipeFlowSinkException`, `PipeFlowConfigurationException`, `PipeFlowValidationException`
- GitHub Actions `build.yml` rewritten with NuGet cache, xUnit v3, and coverage artifacts

## [2.1.0] -- 2024-01-14

(see tag v2.1.0-archived)
