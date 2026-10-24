# Jeomseon Unity Coroutines

[한국어](Overview.md) · [Installation](../README.en.md)

## Purpose

Provides coroutine hosting and Operations that expose completion, cancellation, and exceptions.

## Usage flow

Choose global CoroutineRunner or a MonoBehaviour-bound CoroutineService, Run/RunOperation, observe Status/Completed, then Cancel or end the host lifetime.

## Contracts and limitations

Account for host deactivation/destruction. WaitForSeconds caching is unlimited by default; bound it in Project Settings for continuously varying delays. Iterator lifetime differs from Unity Awaitable.

## Package requirements

- Package ID: `com.jeomseon.unity.coroutines`
- Version: `0.4.1`
- Unity: `6000.6` / `0f1`
- `com.jeomseon.unity.core`: `0.3.2`
- `com.jeomseon.unity.singleton`: `0.3.1`

## Samples and verification

Import the following from Package Manager > Samples. Open the included Scene or Editor tool; follow any Setup step in its sample README.

- [Basic Usage](../Samples~/BasicUsage/README.md)
  - [CoroutineLifetimeSample](../Samples~/BasicUsage/CoroutineLifetimeSample.unity)

[Test sources](../Tests)

Add the package ID to manifest `testables`, then run the relevant Unity Test Runner assemblies. These links describe available tests, not a new successful test run.

## Source map

- [CoroutineCacheSettings](../Runtime/CoroutineCacheSettings.cs)
- [CoroutineExtensions](../Runtime/CoroutineExtentions.cs)
- [CoroutineHelper](../Runtime/CoroutineHelper.cs)
- [CoroutineOperation](../Runtime/CoroutineOperation.cs)
- [CoroutineRunner](../Runtime/CoroutineRunner.cs)
- [CoroutineService](../Runtime/CoroutineService.cs)
- [ICoroutineService](../Runtime/ICoroutineService.cs)
