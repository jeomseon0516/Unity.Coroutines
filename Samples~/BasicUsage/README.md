# Coroutines 기본 예제

Sample을 Import한 뒤 `CoroutineLifetimeSample` Scene을 열어 Play Mode에서 Console을 확인합니다.
1초 뒤 `Scene Coroutine Service` GameObject가 파괴되며 `Canceled` 상태를 출력하고,
2초 뒤 전역 Runner 완료 로그가 출력됩니다.
`Coroutine Operation` GameObject는 한 프레임 뒤 `Completed`, `Canceled` 상태를 각각 출력합니다.

각 컴포넌트를 빈 GameObject에 직접 추가해서도 확인할 수 있습니다.

- `CoroutineSample`: 호스트 수명 서비스와 전역 Runner의 한 프레임 대기를 함께 비교합니다.
- `GlobalCoroutineRunnerSample`: 시작 뒤 GameObject를 파괴하거나 씬을 전환해도 지정한 시간이 지나면
  전역 Runner 완료 로그가 출력됩니다.
- `SceneCoroutineServiceSample`: 지정 시간 전에 GameObject를 파괴하면 `Canceled`, 정상 완료되면 `Completed` 상태를 출력합니다.
- `CoroutineOperationSample`: `CoroutineOperation`의 정상 완료와 명시적 취소 상태를 Console에서 확인합니다.

`SceneCoroutineServiceSample.Initialize(ICoroutineService)`는 DI Composition Root에서 서비스 구현을
주입하는 경로입니다. 호출하지 않으면 같은 GameObject를 호스트로 하는 `CoroutineService`를 기본 사용합니다.
