using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jeomseon.Unity.Singleton;

namespace Jeomseon.Unity.Coroutines
{
    using Coroutine = UnityEngine.Coroutine;

    public sealed class CoroutineRunner : Singleton<CoroutineRunner>, ICoroutineService
    {
        private CoroutineService _service;

        protected override void Init() => _service = new CoroutineService(this);

        public CoroutineOperation RunOperation(IEnumerator routine) => _service.RunOperation(routine);

        public Coroutine Run(IEnumerator routine) => _service.Run(routine);

        public void Stop(Coroutine coroutine) => _service.Stop(coroutine);

        public void StopAll() => _service.StopAll();

        private void OnDestroy() => StopAll();

        public Coroutine InvokeNextFrame(Action callback)
            => CoroutineExtensions.InvokeNextFrame(this, callback);

        public Coroutine InvokeAfterSeconds(float delay, Action callback)
            => CoroutineExtensions.InvokeAfterSeconds(this, delay, callback);

        public Coroutine RepeatWhile<TYieldInstruction>(Func<bool> continueCondition) where TYieldInstruction : YieldInstruction
            => CoroutineExtensions.RepeatWhile<TYieldInstruction>(this, continueCondition);

        public Coroutine RepeatWhile<TYieldInstruction>(Func<bool> continueCondition, Action onStarted) where TYieldInstruction : YieldInstruction
            => CoroutineExtensions.RepeatWhile<TYieldInstruction>(this, continueCondition, onStarted);

        public Coroutine RepeatWhile<TYieldInstruction>(Func<bool> continueCondition, Action onStarted, Action onFinished) where TYieldInstruction : YieldInstruction
            => CoroutineExtensions.RepeatWhile<TYieldInstruction>(this, continueCondition, onStarted, onFinished);

        public Coroutine RepeatWhileWithCompletion<TYieldInstruction>(Func<bool> continueCondition, Action onFinished) where TYieldInstruction : YieldInstruction
            => CoroutineExtensions.RepeatWhileWithCompletion<TYieldInstruction>(this, continueCondition, onFinished);

        public Coroutine InvokeWhen(Func<bool> predicate, Action callback)
            => CoroutineExtensions.InvokeWhen(this, predicate, callback);

        public Coroutine RunInBackground(Action backgroundWork, Action callback)
            => CoroutineExtensions.RunInBackground(this, backgroundWork, callback);

        public Coroutine RunInBackground<T>(Func<T> backgroundWork, Action<T> callback)
            => CoroutineExtensions.RunInBackground(this, backgroundWork, callback);

        public Coroutine ProcessEachFrame<T>(IEnumerable<T> items, Action<T> callback)
            => CoroutineExtensions.ProcessEachFrame(this, items, callback);

        public Coroutine GetComponentWithTimeout<T>(MonoBehaviour host, Action<T> callback) where T : Component
            => host.GetComponentWithTimeout(callback);

        public Coroutine GetComponentWithTimeout<T>(MonoBehaviour host, float timeout, Action<T> callback) where T : Component
            => host.GetComponentWithTimeout(timeout, callback);
    }
}
