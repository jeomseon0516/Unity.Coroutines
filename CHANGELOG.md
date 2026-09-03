# 변경 기록

## [Unreleased]

- Unity 최소 버전을 `6000.6.0f1`로 올렸습니다.
- `CoroutineRunner`가 사용하는 `OnSingletonInitialize()`/`OnSingletonDispose()` 수명 계약에 맞춰
  `com.jeomseon.unity.singleton` 최소 의존성을 `0.3.0`으로 올렸습니다.
- Test asmdef에 `Jeomseon.Unity.Singleton` 직접 참조를 추가하고, PlayMode 테스트를 Editor에서
  제외하던 잘못된 플랫폼 설정을 제거해 Test Runner가 테스트를 발견하도록 수정했습니다.
- 선택 사항인 Sample Import 여부만 검사하고 Coroutines 기능 계약을 검증하지 않던
  `SampleAssetsTests`를 제거했습니다.

## [0.3.0] - 2026-08-13

- **(Breaking)** 네임스페이스를 `Jeomseon.Coroutine`(단수)/`Jeomseon.Coroutine.Editor` →
  `Jeomseon.Unity.Coroutines`/`Jeomseon.Unity.Coroutines.Editor`로 변경했습니다. 워크스페이스 전체
  네임스페이스 규칙(`AGENTS.md` 참고)을 적용한 것으로, 폴더 구조 변경은 없습니다.

## [0.2.1] - 2026-08-11

- 워크스페이스 명명 규칙에 맞춰 `[SerializeField] private` 필드를 `_camelCase`에서 `camelCase`로
  정리하고 기존 이름을 `[FormerlySerializedAs]`로 보존했습니다. `CoroutineCacheSettingsProvider`의
  `FindProperty` 문자열과 테스트의 reflection 기반 `GetField` 문자열도 함께 갱신했습니다. 공개
  C# API 변경은 없으며 기존 Scene·Prefab의 직렬화된 값은 그대로 유지됩니다.

## [0.2.0] - 2026-08-10

- **Breaking**: `DoCall*`, `WaitCompleted*`, `ProgressFromEnumerable`, `GetWaitComponent`, `AddCallback`,
  `StopCoroutineIfNotNull` API를 명확한 동사 중심 명칭으로 교체하고 이전 명칭을 제거했습니다. 0.x
  안정화 정책에 따라 호환 별칭은 제공하지 않습니다. [Migration 0.1.5 to 0.2.0](Documentation~/Migration-0.1.5-to-0.2.0.md)을 확인하세요.
- `WaitCompletedAsync`가 `Task` 완료 전에 콜백을 실행할 수 있던 문제를 수정해 `RunInBackground`이
  완료·실패를 올바르게 기다리도록 했습니다.
- Unity 기본 API의 단순 Wrapper이자 첫 호출에 `null`을 반환하는 결함이 있던
  `CoroutineHelper.WaitForSecondsRealtime`과 전용 캐시를 제거했습니다.
- `WaitForSeconds` 캐시 제한을 `Project Settings > Jeomseon > Coroutines`에서 선택할 수 있게
  하고, 무제한 캐시 상태에 경고를 추가했습니다. Play Mode 재진입 시 캐시를 초기화합니다.
- DI 컨테이너에 독립적인 `ICoroutineService`와 MonoBehaviour 호스트 수명 `CoroutineService`를
  추가했습니다. 기존 Singleton 기반 `CoroutineRunner`는 `ICoroutineService`를 구현해 패키지
  단독 전역 실행 경로를 유지합니다.
- `CoroutineOperation`과 `ICoroutineService.RunOperation`을 추가해 완료·취소·예외 상태를
  관찰할 수 있게 했습니다. Unity 작업 수명 계약을 위해 `com.jeomseon.unity.core`(`0.2.2`)
  의존성을 추가했습니다.
- Domain Reload 비활성화 상태의 Play Mode 재진입에서 이전 Runner 작업이 재개되지 않는지
  확인하는 수명 회귀 테스트를 추가했습니다.
- Basic Usage Sample에 전역 Runner, 호스트 수명 서비스, DI 주입 경로, `CoroutineOperation`
  완료·취소 관찰 예제를 추가하고, `CoroutineLifetimeSample` Scene에서 Console로 확인할 수
  있게 했습니다.
- Unity Awaitable 별도 패키지(`com.jeomseon.unity.awaitables`)의 타당성·의존성·수명 계약을
  [Documentation~/Awaitables.md](Documentation~/Awaitables.md)에 문서화했습니다. 실제
  소비 요구가 생길 때 구현합니다.

## [0.1.3] - 2026-08-05

- Unity 6000.5.7f1을 최소 지원 버전으로 상향했습니다.

## [0.1.2] - 2026-07-29

- asmdef의 `rootNamespace`와 소스 파일 위치를 namespace에 맞게 정리했습니다.

## [0.1.1] - 2026-07-29

- 프레임 지연 콜백을 확인하는 `Basic Usage` 샘플을 추가했습니다.

## [0.1.0] - 2026-07-29

- JeomseonScriptPack의 관련 모듈을 독립 UPM 패키지로 분리했습니다.
