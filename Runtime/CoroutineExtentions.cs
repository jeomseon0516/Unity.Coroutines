using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Jeomseon.Coroutine
{
    using Coroutine = UnityEngine.Coroutine;

    public static class CoroutineExtensions
    {
        public static void StopIfRunning(this MonoBehaviour host, Coroutine coroutine)
        {
            if (!host || coroutine is null) return;

            host.StopCoroutine(coroutine);
        }

        public static void StopIfRunning(this MonoBehaviour host, IEnumerator routine)
        {
            if (!host || routine is null) return;

            host.StopCoroutine(routine);
        }

        public static Coroutine InvokeNextFrame(this MonoBehaviour host, Action callback)
            => host.StartCoroutine(InvokeNextFrameRoutine(callback));

        public static Coroutine InvokeAfterSeconds(this MonoBehaviour host, float delay, Action callback)
            => host.StartCoroutine(InvokeAfterSecondsRoutine(delay, callback));

        public static Coroutine RepeatWhile<TYieldInstruction>(this MonoBehaviour host, Func<bool> continueCondition)
            where TYieldInstruction : YieldInstruction
            => host.StartCoroutine(RepeatWhileRoutine<TYieldInstruction>(continueCondition));

        public static Coroutine RepeatWhile<TYieldInstruction>(this MonoBehaviour host, Func<bool> continueCondition, Action onStarted)
            where TYieldInstruction : YieldInstruction
            => host.StartCoroutine(RepeatWhileRoutine<TYieldInstruction>(continueCondition, onStarted));

        public static Coroutine RepeatWhile<TYieldInstruction>(this MonoBehaviour host, Func<bool> continueCondition, Action onStarted, Action onFinished)
            where TYieldInstruction : YieldInstruction
            => host.StartCoroutine(RepeatWhileRoutine<TYieldInstruction>(continueCondition, onStarted, onFinished));

        public static Coroutine RepeatWhileWithCompletion<TYieldInstruction>(this MonoBehaviour host, Func<bool> continueCondition, Action onFinished)
            where TYieldInstruction : YieldInstruction
            => host.StartCoroutine(RepeatWhileRoutine<TYieldInstruction>(continueCondition, null, onFinished));

        public static Coroutine InvokeWhen(this MonoBehaviour host, Func<bool> predicate, Action callback)
            => host.StartCoroutine(InvokeWhenRoutine(predicate, callback));

        public static Coroutine RunInBackground(this MonoBehaviour host, Action backgroundWork, Action callback)
            => host.StartCoroutine(WaitForTaskRoutine(Task.Run(backgroundWork), callback));

        public static Coroutine RunInBackground<T>(this MonoBehaviour host, Func<T> backgroundWork, Action<T> callback)
            => host.StartCoroutine(WaitForTaskRoutine(Task.Run(backgroundWork), callback));

        public static Coroutine ProcessEachFrame<T>(this MonoBehaviour host, IEnumerable<T> items, Action<T> callback)
            => host.StartCoroutine(ProcessEachFrameRoutine(items, callback));

        public static Coroutine GetComponentWithTimeout<T>(this MonoBehaviour host, Action<T> callback) where T : Component
            => host.StartCoroutine(GetComponentWithTimeoutRoutine(host, 2.0f, callback));

        public static Coroutine GetComponentWithTimeout<T>(this MonoBehaviour host, float timeout, Action<T> callback) where T : Component
            => host.StartCoroutine(GetComponentWithTimeoutRoutine(host, timeout, callback));

        public static IEnumerator InvokeAfter(this Coroutine coroutine, Action callback)
        {
            yield return coroutine;
            callback.Invoke();
        }

        public static IEnumerator InvokeAfter(this IEnumerator routine, Action callback)
        {
            yield return routine;
            callback.Invoke();
        }

        private static IEnumerator InvokeWhenRoutine(Func<bool> predicate, Action callback)
        {
            yield return new WaitUntil(predicate);
            callback.Invoke();
        }

        private static IEnumerator WaitForTaskRoutine(Task task, Action callback)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                throw task.Exception;
            }

            if (!task.IsCanceled)
            {
                callback.Invoke();
            }
        }

        private static IEnumerator WaitForTaskRoutine<T>(Task<T> task, Action<T> callback)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                throw task.Exception;
            }

            if (!task.IsCanceled)
            {
                callback.Invoke(task.Result);
            }
        }

        private static IEnumerator ProcessEachFrameRoutine<T>(IEnumerable<T> items, Action<T> callback)
        {
            foreach (T item in items)
            {
                callback.Invoke(item);
                yield return null;
            }
        }

        private static IEnumerator InvokeNextFrameRoutine(Action callback)
        {
            yield return null;
            callback.Invoke();
        }

        private static IEnumerator InvokeAfterSecondsRoutine(float delay, Action callback)
        {
            yield return CoroutineHelper.WaitForSeconds(delay);
            callback.Invoke();
        }

        private static IEnumerator RepeatWhileRoutine<TYieldInstruction>(Func<bool> continueCondition)
            where TYieldInstruction : YieldInstruction
        {
            TYieldInstruction yieldInstruction = GetReusableYieldInstruction<TYieldInstruction>();

            do
            {
                yield return yieldInstruction;
            } while (continueCondition.Invoke());
        }

        private static IEnumerator RepeatWhileRoutine<TYieldInstruction>(Func<bool> continueCondition, Action onStarted)
            where TYieldInstruction : YieldInstruction
        {
            onStarted?.Invoke();
            yield return RepeatWhileRoutine<TYieldInstruction>(continueCondition);
        }

        private static IEnumerator RepeatWhileRoutine<TYieldInstruction>(Func<bool> continueCondition, Action onStarted, Action onFinished)
            where TYieldInstruction : YieldInstruction
        {
            onStarted?.Invoke();
            yield return RepeatWhileRoutine<TYieldInstruction>(continueCondition);
            onFinished?.Invoke();
        }

        private static IEnumerator GetComponentWithTimeoutRoutine<T>(MonoBehaviour host, float timeout, Action<T> callback)
            where T : Component
        {
            T component;

            while (!host.TryGetComponent(out component) && timeout > 0.0f)
            {
                yield return null;
                timeout -= Time.deltaTime;
            }

            callback.Invoke(component);
        }

        private static T GetReusableYieldInstruction<T>() where T : YieldInstruction => typeof(T) switch
        {
            { } type when type == typeof(WaitForEndOfFrame) => CoroutineHelper.WaitForEndOfFrame as T,
            { } type when type == typeof(WaitForFixedUpdate) => CoroutineHelper.WaitForFixedUpdate as T,
            _ => null
        };
    }
}
