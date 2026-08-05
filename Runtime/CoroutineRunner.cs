using System;
using System.Collections.Generic;
using UnityEngine;
using Jeomseon.Singleton;

namespace Jeomseon.Coroutine
{
    using Coroutine = UnityEngine.Coroutine;

    public sealed class CoroutineRunner : Singleton<CoroutineRunner>
    {
        /* TODO(P0-02, lifecycle): Singleton 인스턴스가 Domain Reload 비활성화 상태에서
         * Play Mode 재진입 후에도 이전 코루틴 상태를 유지하지 않는지 검증합니다.
         */
        protected override void Init() {}

        /* TODO(P2-01, api): Unity 6 Awaitable과 코루틴의 취소·예외 전달 차이를 비교하고,
         * 지원 버전별로 비동기 API로 대체 가능한 호출 경로를 제공합니다.
         */
        public Coroutine DoCallWaitForOneFrame(Action action)
            => CoroutineExtensions.DoCallWaitForOneFrame(this, action);

        public Coroutine DoCallWaitForSeconds(float delayTime, Action action)
            => CoroutineExtensions.DoCallWaitForSeconds(this, delayTime, action);

        public Coroutine DoCallRoofCoroutine<T>(Func<bool> action) where T : YieldInstruction
            => CoroutineExtensions.DoCallRoofCoroutine<T>(this, action);

        public Coroutine DoCallRoofCoroutine<T>(Func<bool> action, Action init) where T : YieldInstruction
            => CoroutineExtensions.DoCallRoofCoroutine<T>(this, action, init);

        public Coroutine DoCallRoofCoroutine<T>(Func<bool> action, Action init, Action finish) where T : YieldInstruction
            => CoroutineExtensions.DoCallRoofCoroutine<T>(this, action, init, finish);

        public Coroutine DoCallRoofCoroutineFinish<T>(Func<bool> action, Action finish) where T : YieldInstruction
            => CoroutineExtensions.DoCallRoofCoroutineFinish<T>(this, action, finish);

        public Coroutine WaitCompletedConditions(Func<bool> match, Action callback)
            => CoroutineExtensions.WaitCompletedConditions(this, match, callback);

        public Coroutine WaitCompletedAsync(Action asyncAction, Action callback)
            => CoroutineExtensions.WaitCompletedAsync(this, asyncAction, callback);

        public Coroutine WaitCompletedAsync<T>(Func<T> asycnAction, Action<T> callback)
            => CoroutineExtensions.WaitCompletedAsync(this, asycnAction, callback);

        public Coroutine ProgressFromEnumerable<T>(IEnumerable<T> objects, Action<T> callback)
            => CoroutineExtensions.ProgressFromEnumerable(this, objects, callback);

        public Coroutine GetWaitComponent<T>(MonoBehaviour monoBehaviour, Action<T> callback) where T : Component
            => monoBehaviour.GetWaitComponent(callback);

        public Coroutine GetWaitComponent<T>(MonoBehaviour monoBehaviour, float delayTime, Action<T> callback) where T : Component
            => monoBehaviour.GetWaitComponent(delayTime, callback);
    }
}
