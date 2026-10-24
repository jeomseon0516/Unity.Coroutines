# Jeomseon Unity Coroutines

[English](Overview.en.md) · [설치](../README.md)

## 목적

Coroutine 실행 호스트와 완료·취소·예외를 관찰하는 Operation 계약을 제공합니다.

## 사용 흐름

전역 CoroutineRunner 또는 MonoBehaviour에 묶인 CoroutineService 선택 → Run/RunOperation → 상태·Completed 관찰 → Cancel 또는 호스트 수명 종료.

## 계약과 제한

호스트 비활성/파괴와 coroutine 종료를 고려하세요. WaitForSeconds 캐시는 기본 무제한이므로 연속적인 임의 지연값을 캐시할 때 Project Settings에서 상한을 설정합니다. Unity Awaitable과 다른 iterator 수명 계약입니다.

## 패키지 요구 사항

- Package ID: `com.jeomseon.unity.coroutines`
- Version: `0.4.1`
- Unity: `6000.6` / `0f1`
- `com.jeomseon.unity.core`: `0.3.2`
- `com.jeomseon.unity.singleton`: `0.3.1`

## 샘플과 검증

Package Manager > Samples에서 다음 예제를 Import하세요. 포함된 Scene 또는 Editor 도구를 열고 샘플 README의 Setup 절차를 먼저 수행합니다.

- [Basic Usage](../Samples~/BasicUsage/README.md)
  - [CoroutineLifetimeSample](../Samples~/BasicUsage/CoroutineLifetimeSample.unity)

[테스트 코드](../Tests)

소비 프로젝트 manifest의 `testables`에 패키지 ID를 추가하고 Unity Test Runner의 관련 어셈블리를 실행합니다. 위 링크는 테스트 위치이며 이번 문서 수정으로 새 테스트 실행을 주장하지 않습니다.

## 코드 지도

- [CoroutineCacheSettings](../Runtime/CoroutineCacheSettings.cs)
- [CoroutineExtensions](../Runtime/CoroutineExtentions.cs)
- [CoroutineHelper](../Runtime/CoroutineHelper.cs)
- [CoroutineOperation](../Runtime/CoroutineOperation.cs)
- [CoroutineRunner](../Runtime/CoroutineRunner.cs)
- [CoroutineService](../Runtime/CoroutineService.cs)
- [ICoroutineService](../Runtime/ICoroutineService.cs)
