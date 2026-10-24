# Jeomseon Unity Coroutines

한국어 | [English](README.en.md)

Coroutine 실행 호스트와 완료·취소·예외를 관찰하는 Operation 계약을 제공합니다.

## 빠르게 시작하기

전역 CoroutineRunner 또는 MonoBehaviour에 묶인 CoroutineService 선택 → Run/RunOperation → 상태·Completed 관찰 → Cancel 또는 호스트 수명 종료.

## 계약과 주의사항

호스트 비활성/파괴와 coroutine 종료를 고려하세요. WaitForSeconds 캐시는 기본 무제한이므로 연속적인 임의 지연값을 캐시할 때 Project Settings에서 상한을 설정합니다. Unity Awaitable과 다른 iterator 수명 계약입니다.

## 요구 사항

- Unity `6000.6.0f1`
- 패키지: `com.jeomseon.unity.coroutines` `0.4.1`
- `com.jeomseon.unity.core`: `0.3.2`
- `com.jeomseon.unity.singleton`: `0.3.1`

## 설치

프로젝트 `Packages/manifest.json`의 기존 dependencies와 registry를 유지하면서 다음 항목을 병합합니다.

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

또는 Package Manager의 **Install package from git URL**을 사용합니다:

```text
https://github.com/jeomseon0516/Unity.Coroutines.git#v0.4.1
```

## API 사용과 상세 설명

### WaitForSeconds 캐시

`CoroutineHelper.WaitForSeconds`는 같은 지연값을 캐시합니다. 기본값은 무제한이며,
`Project Settings > Jeomseon > Coroutines`에서 제한을 켜고 최대 개수를 설정할 수 있습니다.
지연값이 동적으로 계속 달라질 수 있으면 제한을 켜세요. 무제한 상태에서는 Settings 창에 경고가 표시됩니다.

### 코루틴 서비스

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

### Operation 상태

`RunOperation`은 `Jeomseon.Unity.Core.Operations.IManagedOperation`을 구현한 `CoroutineOperation`을 반환합니다.
완료·취소·예외 종료를 `Status`, `Exception`, `Completed`로 관찰할 수 있습니다.

```csharp
CoroutineOperation operation = coroutines.RunOperation(routine);
operation.Completed += completed => Debug.Log(completed.Status);
operation.Cancel();
```

`CoroutineService` 작업은 호스트 파괴 시 취소되고, `CoroutineRunner` 작업은 전역 Runner가 파괴될 때 취소됩니다.

### API 마이그레이션

0.2.0에서 불명확하거나 오탈자가 있던 Coroutine API를 제거하고 명확한 이름으로 교체했습니다.
이전 API와 호환 별칭은 제공하지 않습니다. 호출부 교체 표는
[Migration 0.1.5 to 0.2.0](Documentation~/Migration-0.1.5-to-0.2.0.md)을 확인하세요.

## 샘플 실행

**Package Manager > Samples**에서 예제를 Import합니다. 포함된 Scene 또는 Editor 도구를 열고 필요한 Setup 메뉴를 먼저 실행합니다.

- [Basic Usage](Samples~/BasicUsage/README.md)
  - [CoroutineLifetimeSample](Samples~/BasicUsage/CoroutineLifetimeSample.unity)

## 테스트

[테스트 코드](Tests)

manifest의 `testables`에 패키지 ID를 추가하고 Unity Test Runner의 관련 EditMode/PlayMode 어셈블리를 실행합니다.

## 추가 문서

[전체 코드 지도와 패키지 구성](Documentation~/Overview.md)
- [Changelog](CHANGELOG.md)

## 라이선스

[License](LICENSE.md)
