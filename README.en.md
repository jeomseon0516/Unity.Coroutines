# Jeomseon Unity Coroutines

[한국어](README.md) | English

Provides coroutine hosting and Operations that expose completion, cancellation, and exceptions.

## Quick start

Choose global CoroutineRunner or a MonoBehaviour-bound CoroutineService, Run/RunOperation, observe Status/Completed, then Cancel or end the host lifetime.

## Contracts and limitations

Account for host deactivation/destruction. WaitForSeconds caching is unlimited by default; bound it in Project Settings for continuously varying delays. Iterator lifetime differs from Unity Awaitable.

## Requirements

- Unity `6000.6.0f1`
- Package: `com.jeomseon.unity.coroutines` `0.4.1`
- `com.jeomseon.unity.core`: `0.3.2`
- `com.jeomseon.unity.singleton`: `0.3.1`

## Installation

Merge this into your project's `Packages/manifest.json`; preserve existing dependencies and registries.

```json
{
  "scopedRegistries": [
    {
      "name": "OpenUPM",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.jeomseon"
      ]
    }
  ],
  "dependencies": {
    "com.jeomseon.unity.coroutines": "0.4.1"
  }
}
```

Or use **Install package from git URL** in Package Manager:

```text
https://github.com/jeomseon0516/Unity.Coroutines.git#v0.4.1
```

## API usage and details

### WaitForSeconds cache

`CoroutineHelper.WaitForSeconds` caches instructions with the same delay. Caching is unlimited by default.
Enable a limit and set its maximum in `Project Settings > Jeomseon > Coroutines` when delay values can vary without a fixed bound. The Settings page displays a warning while the cache is unlimited.

### Coroutine services

For a global task without DI, start it through `CoroutineRunner.Instance`.

```csharp
CoroutineRunner.Instance.Run(routine);
```

For scene-lifetime work or DI registration, create a `CoroutineService` with the calling object as its host. Unity stops its work when the host GameObject is disabled or destroyed.

```csharp
ICoroutineService coroutines = new CoroutineService(this);
coroutines.Run(routine);
```

The package has no dependency on a DI container. Register either `CoroutineService` or `CoroutineRunner.Instance` as `ICoroutineService` from the application's composition root.

### Operation state

`RunOperation` returns a `CoroutineOperation`, which implements `Jeomseon.Unity.Core.Operations.IManagedOperation`. Observe normal completion, cancellation, and faults through `Status`, `Exception`, and `Completed`.

```csharp
CoroutineOperation operation = coroutines.RunOperation(routine);
operation.Completed += completed => Debug.Log(completed.Status);
operation.Cancel();
```

`CoroutineService` operations are canceled when their host is destroyed. `CoroutineRunner` operations are canceled when the global runner is destroyed.

### API migration

Version 0.2.0 removes the ambiguous and misspelled coroutine API names. Compatibility aliases are
not provided during the 0.x stabilization phase. See the replacement table in
[Migration 0.1.5 to 0.2.0](Documentation~/Migration-0.1.5-to-0.2.0.md).

### Awaitable package assessment

The iterator lifecycle contract remains in this package. A separate Unity Awaitables package is viable;
the dependency, lifecycle, and operation-contract assessment is documented in
[Awaitables](Documentation~/Awaitables.md).

## Samples

Import the desired entry from **Package Manager > Samples**. Use the included Scene/Editor tool and complete its Setup step first, if required.

- [Basic Usage](Samples~/BasicUsage/README.md)
  - [CoroutineLifetimeSample](Samples~/BasicUsage/CoroutineLifetimeSample.unity)

## Tests

[Test sources](Tests)

Add the package ID to manifest `testables`, then execute its relevant EditMode/PlayMode assemblies in Unity Test Runner.

## Further reading

[Complete source map and package metadata](Documentation~/Overview.en.md)
- [Changelog](CHANGELOG.md)

## License

[License](LICENSE.md)
