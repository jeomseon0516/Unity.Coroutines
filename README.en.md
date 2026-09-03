# Jeomseon Unity Coroutines

Coroutine runner and coroutine handle utilities for Unity.

Requires Unity 6000.6.0f1 or newer and `com.jeomseon.unity.singleton` 0.3.0 or newer.

## WaitForSeconds cache

`CoroutineHelper.WaitForSeconds` caches instructions with the same delay. Caching is unlimited by default.
Enable a limit and set its maximum in `Project Settings > Jeomseon > Coroutines` when delay values can vary without a fixed bound. The Settings page displays a warning while the cache is unlimited.

## Coroutine services

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

## Operation state

`RunOperation` returns a `CoroutineOperation`, which implements `Jeomseon.Unity.Operations.IManagedOperation`. Observe normal completion, cancellation, and faults through `Status`, `Exception`, and `Completed`.

```csharp
CoroutineOperation operation = coroutines.RunOperation(routine);
operation.Completed += completed => Debug.Log(completed.Status);
operation.Cancel();
```

`CoroutineService` operations are canceled when their host is destroyed. `CoroutineRunner` operations are canceled when the global runner is destroyed.

## API migration

Version 0.2.0 removes the ambiguous and misspelled coroutine API names. Compatibility aliases are
not provided during the 0.x stabilization phase. See the replacement table in
[Migration 0.1.5 to 0.2.0](Documentation~/Migration-0.1.5-to-0.2.0.md).

## Awaitable package assessment

The iterator lifecycle contract remains in this package. A separate Unity Awaitables package is viable;
the dependency, lifecycle, and operation-contract assessment is documented in
[Awaitables](Documentation~/Awaitables.md).
