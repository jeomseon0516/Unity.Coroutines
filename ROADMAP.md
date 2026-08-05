# Coroutines 로드맵

우선순위: `P0` 결함·안전성 → `P1` 핵심 구조 → `P2` API·성능 → `P3` 장기 확장

## 작업 순서

1. **P0-01 — YieldInstruction 캐시 수명과 정확성**
   - Domain Reload 재진입과 캐시 무한 증가를 검증합니다.
   - `WaitForSecondsRealtime` 캐시 생성 조건을 회귀 테스트합니다.
2. **P0-02 — CoroutineRunner 수명 안정화**
   - 씬 전환, 인스턴스 파괴, Play Mode 재진입 시 실행 중 코루틴을 정리합니다.
3. **P1-01 — 취소와 예외 계약 추가**
   - 취소 핸들, 예외 전달, owner 파괴 시 동작을 정의합니다.
4. **P2-01 — Unity 6 Awaitable 연동**
   - 지원 버전에서 Awaitable로 대체 가능한 API와 코루틴 전용 API를 분리합니다.
5. **P2-02 — API 명명 정리**
   - `DoCallRoofCoroutine`, `asycnAction` 등 오탈자와 의도가 불명확한 이름을 안정화 전에 변경합니다.
