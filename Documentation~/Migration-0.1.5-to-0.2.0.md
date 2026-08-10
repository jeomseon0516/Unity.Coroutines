# Migration: 0.1.5 → 0.2.0

0.x 안정화 정책에 따라 이전 API와 호환 별칭은 제거했습니다. 아래 표에 따라 호출부를 교체하세요.

| 제거된 API | 새 API |
| --- | --- |
| `StopCoroutineIfNotNull` | `StopIfRunning` |
| `DoCallWaitForOneFrame` | `InvokeNextFrame` |
| `DoCallWaitForSeconds` | `InvokeAfterSeconds` |
| `DoCallRoofCoroutine<T>` | `RepeatWhile<T>` |
| `DoCallRoofCoroutine<T>(..., init, finish)` | `RepeatWhile<T>(..., onStarted, onFinished)` |
| `DoCallRoofCoroutineFinish<T>` | `RepeatWhileWithCompletion<T>` |
| `WaitCompletedConditions` | `InvokeWhen` |
| `WaitCompletedAsync` | `RunInBackground` |
| `ProgressFromEnumerable` | `ProcessEachFrame` |
| `GetWaitComponent` | `GetComponentWithTimeout` |
| `AddCallback` | `InvokeAfter` |

`CoroutineRunner`의 동등한 전달 메서드도 같은 이름으로 교체되었습니다.

## 대표 교체

```csharp
// 0.1.5
CoroutineRunner.Instance.DoCallWaitForSeconds(1f, OnCompleted);

// 0.2.0
CoroutineRunner.Instance.InvokeAfterSeconds(1f, OnCompleted);
```

```csharp
// 0.1.5
host.WaitCompletedAsync(LoadData, ApplyData);

// 0.2.0
host.RunInBackground(LoadData, ApplyData);
```

`RunInBackground`의 `backgroundWork`에서는 UnityEngine 객체를 접근하지 마세요. 완료 콜백은
Coroutine이 재개되는 Unity 메인 스레드에서 실행됩니다. 작업 실패는 Coroutine 예외로 전달됩니다.

`RepeatWhile<TYieldInstruction>`은 기존과 같이 `WaitForEndOfFrame`과 `WaitForFixedUpdate`만 재사용합니다.
다른 `YieldInstruction` 타입을 전달하는 경우는 지원하지 않습니다.
