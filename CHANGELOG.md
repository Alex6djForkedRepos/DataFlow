# Changelog

All notable changes to PipeFlow will be documented in this file.

## [Unreleased]

### Wave 0.A - Foundation  Complete

- [x] Archive v2.1.0 as git tag `v2.1.0-archived`
- [x] Delete v2 code from working tree
- [x] Repository restructure: SLNX + Central Package Management + `Directory.Build.props`
- [x] Multi-target `net8.0` and `net10.0`
- [x] Core abstractions: `IPipelineSource<T>`, `IPipelineSink<T>`, `IPipeline<T>`, `IOrderedPipeline<T>`, `IQueryablePipeline<T>`, `IOrderedQueryablePipeline<T>`
- [x] `DataRow` with `IEquatable<DataRow>`, cached `GetHashCode`, null-friendly indexer, `InvariantCulture` type conversion (incl. `Nullable<T>` unwrap)
- [x] Structured exception hierarchy
- [x] `PipelineContext` (public readonly record struct) with Logger/Options/HTTP/CT/Services
- [x] GitHub Actions `build.yml`: ubuntu/windows/macos × net8/net10 matrix with NuGet cache
- [x] v2 `release.yml` and `dependency-check.yml` disabled pending Wave 4 rewrite
- **61 tests green**

**Next: Wave 0.B - Pipeline Implementation** (Pipeline<T>, ParallelPipeline<T>, Builder, DI, Options)

## [2.1.0] -- 2024-01-14

(see tag v2.1.0-archived)
