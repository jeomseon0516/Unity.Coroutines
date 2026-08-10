# Awaitable 별도 패키지 타당성 검토

검토 기준: Unity 6000.5.7f1의 `UnityEngine.Awaitable`과 현재 Coroutine 수명 계약을 혼합하지 않습니다.

## 결론

`com.jeomseon.unity.awaitables`를 별도 UPM 패키지로 제공하는 것은 타당합니다. 다만 현재 Coroutines를
Awaitable로 전환하거나 이 패키지에 Awaitable API를 추가하지 않습니다. 두 API는 호출 형태, 취소,
재사용 규칙이 다르므로 별도 구현과 별도 Sample이 필요합니다.

## 패키지 경계

```text
Jeomseon.Core
        ↑
Jeomseon.Unity.Core (IManagedOperation, ManagedOperationStatus)
        ↑                         ↑
Jeomseon.Unity.Coroutines    Jeomseon.Unity.Awaitables (후속)
```

- Awaitables는 `com.jeomseon.unity.core`만 참조합니다. Coroutines와 `CoroutineRunner`를 참조하지 않습니다.
- 컨테이너 비의존 경로는 호스트 기반 `AwaitableService`, DI 경로는 `IAwaitableService` 등록으로 제공합니다.
- 전역 실행 경로가 필요할 때만 별도 `AwaitableRunner`를 제공합니다. CoroutineRunner를 재사용하지 않습니다.
- `IManagedOperation`은 Unity 수명 상태·취소·예외 관찰 계약이므로 `Jeomseon.Unity.Core`에 유지합니다. Awaitable 구현체도 같은 상태를 보고할 수 있습니다.

## 구현 시 지켜야 할 계약

- 작업 시작 API는 `Func<CancellationToken, Awaitable>` 형태로 호스트 파괴 토큰과 호출 취소 토큰을 연결합니다.
- 호스트 수명 서비스는 `MonoBehaviour.destroyCancellationToken`으로 취소합니다. 전역 Runner는 자체 수명 토큰을 소유합니다.
- 완료는 `Completed`, 취소는 `Canceled`, 예외는 `Faulted`와 `Exception`으로 `IManagedOperation`에 반영합니다.
- `Awaitable` 인스턴스는 한 번만 await합니다. 여러 소비자 관찰이나 재-await가 필요한 API는 새 Awaitable을 만들거나 별도 Task 어댑터를 명시적으로 설계합니다.
- Awaitable continuation은 같은 프레임에서 동기 실행될 수 있으므로, 완료 이벤트와 사용자 콜백은 재진입을 안전하게 처리해야 합니다.

## 구현 보류 사유

Unity가 이미 프레임·초·FixedUpdate·스레드 전환 Awaitable을 제공합니다. 현재 패키지에는 Awaitable 전용
소비 시나리오가 없으므로, 추상화만 먼저 추가하면 사용되지 않는 수명 어댑터와 Sample을 유지해야 합니다.
실제 Async API 요구가 생기면 이 문서의 경계와 계약으로 새 패키지를 시작합니다.

## 근거

- [Unity Awaitable 소개](https://docs.unity3d.com/6000.5/Documentation/Manual/async-awaitable-introduction.html): Awaitable의 Unity 루프 API, 풀링, 단일 await, continuation 특성을 설명합니다.
- [MonoBehaviour.destroyCancellationToken](https://docs.unity3d.com/6000.5/Documentation/ScriptReference/MonoBehaviour-destroyCancellationToken.html): 호스트 파괴에 연결할 취소 토큰을 제공합니다.
