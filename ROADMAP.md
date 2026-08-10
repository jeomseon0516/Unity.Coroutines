# Coroutines 로드맵

우선순위: `P0` 결함·안전성 → `P1` 핵심 구조 → `P2` API·성능 → `P3` 장기 확장

## 작업 순서

1. **P0-01 — YieldInstruction 캐시 수명과 정확성** ✅
   - Subsystem 등록 시 캐시를 초기화해 Domain Reload 비활성화 Play Mode 재진입에도 이전 대기 명령을 유지하지 않습니다.
   - Unity 기본 API의 단순 Wrapper인 `WaitForSecondsRealtime`과 전용 캐시를 제거했습니다.
   - `Project Settings > Jeomseon > Coroutines`에서 `WaitForSeconds` 캐시 제한을 설정할 수 있으며, 무제한 상태는 경고로 알립니다.
   - EditMode에서 캐시 재사용·제한·무제한·초기화를, PlayMode에서 동시 사용을 회귀 테스트했습니다.
2. **P0-02 — CoroutineRunner 수명 안정화** ✅
   - `CoroutineRunner`는 전역 작업을 씬 전환 후에도 유지하며, 인스턴스 파괴 시 실행 중 작업을 정리합니다.
   - `CoroutineService`는 제공한 MonoBehaviour 호스트 수명을 따르며, 호스트 파괴 시 Unity가 실행 중 작업을 정리합니다.
   - EditMode·PlayMode 테스트로 전역 작업의 씬 전환 유지와 두 호스트의 파괴 정리를 확인했습니다.
   - Domain Reload 비활성화 상태의 Play Mode 재진입에서 이전 작업이 재개되지 않고 새 Runner가 생성되는 것을 EditMode 회귀 테스트로 확인했습니다.
3. **P1-01 — 취소와 예외 계약 추가** ✅
   - `Jeomseon.Unity.Core`의 `IManagedOperation` 공용 계약과 `CoroutineOperation` 구현으로 취소 핸들, 예외 전달, owner 파괴 시 동작을 정의합니다.
4. **P2-01 — Awaitable 별도 패키지 타당성 검토** ✅
   - `com.jeomseon.unity.awaitables`는 `Jeomseon.Unity.Core` 작업 계약만 재사용하고 Coroutines에는 의존하지 않는 별도 패키지로 구현 가능합니다.
   - Unity Awaitable의 단일 await·명시적 취소 토큰·동기 continuation 계약에 맞춰 실제 소비 요구가 생길 때 구현합니다.
5. **P2-02 — API 명명 정리** ✅
   - `DoCallRoofCoroutine`, `asycnAction` 등 오탈자와 의도가 불명확한 이름을 동사 중심 API로 교체하고 이전 API는 제거했습니다.
   - 0.x 정책에 맞춰 호환 별칭은 제공하지 않으며 호출부 전환 표를 Migration 문서에 기록했습니다.
