# Jeomseon Unity Coroutines

Coroutine runner and coroutine handle utilities for Unity.

## 설치

OpenUPM 등록 전에는 Package Manager의 **Add package from git URL**에서 다음 주소를 사용합니다.

```text
https://github.com/jeomseon0516/Unity.Coroutines.git#v0.1.1
```

## 리팩토링 방침

Unity가 제공하는 동등 기능과 비교해 대체 가능한 코드는 소스의 한글 TODO 주석과 CHANGELOG의 Unreleased 항목에서 추적합니다.

## WaitForSeconds 캐시

`CoroutineHelper.WaitForSeconds`는 같은 지연값을 캐시합니다. 기본값은 무제한이며,
`Project Settings > Jeomseon > Coroutines`에서 제한을 켜고 최대 개수를 설정할 수 있습니다.
지연값이 동적으로 계속 달라질 수 있으면 제한을 켜세요. 무제한 상태에서는 Settings 창에 경고가 표시됩니다.

## 코루틴 서비스

DI를 사용하지 않는 프로젝트에서는 전역 작업을 `CoroutineRunner.Instance`로 시작합니다.

```csharp
CoroutineRunner.Instance.Run(routine);
```

씬 수명 작업이나 DI 등록에는 호출 객체를 호스트로 하는 `CoroutineService`를 사용합니다.
호스트 GameObject가 비활성화되거나 파괴되면 Unity가 작업을 중지합니다.

```csharp
ICoroutineService coroutines = new CoroutineService(this);
coroutines.Run(routine);
```

패키지는 특정 DI 컨테이너에 의존하지 않습니다. 앱의 Composition Root에서
`ICoroutineService`에 `CoroutineService` 또는 `CoroutineRunner.Instance`를 등록하세요.

## Operation 상태

`RunOperation`은 `Jeomseon.Unity.Operations.IManagedOperation`을 구현한 `CoroutineOperation`을 반환합니다.
완료·취소·예외 종료를 `Status`, `Exception`, `Completed`로 관찰할 수 있습니다.

```csharp
CoroutineOperation operation = coroutines.RunOperation(routine);
operation.Completed += completed => Debug.Log(completed.Status);
operation.Cancel();
```

`CoroutineService` 작업은 호스트 파괴 시 취소되고, `CoroutineRunner` 작업은 전역 Runner가 파괴될 때 취소됩니다.

## API 마이그레이션

0.2.0에서 불명확하거나 오탈자가 있던 Coroutine API를 제거하고 명확한 이름으로 교체했습니다.
이전 API와 호환 별칭은 제공하지 않습니다. 호출부 교체 표는
[Migration 0.1.5 to 0.2.0](Documentation~/Migration-0.1.5-to-0.2.0.md)을 확인하세요.

## Awaitable 패키지 검토

Coroutine의 iterator 수명 계약은 유지합니다. Unity `Awaitable`은 별도 패키지에서 설계할 수 있으며,
현 단계의 의존성·수명·작업 계약 검토 결과는 [Awaitables](Documentation~/Awaitables.md)에 기록했습니다.
