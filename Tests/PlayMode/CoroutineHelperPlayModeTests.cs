using System.Collections;
using Jeomseon.Unity.Coroutines;
using Jeomseon.Unity.Core.Operations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Jeomseon.Tests
{
    public sealed class CoroutineHelperPlayModeTests
    {
        private const float Delay = 0.1f;

        [UnityTest]
        public IEnumerator WaitForSeconds_AllowsConcurrentUseOfTheSameCachedInstruction()
        {
            var gameObject = new GameObject(nameof(CoroutineHelperPlayModeTests));
            var runner = gameObject.AddComponent<TestCoroutineRunner>();
            var completedCount = 0;
            var firstElapsed = 0f;
            var secondElapsed = 0f;

            runner.StartCoroutine(WaitAndMeasure(elapsed => firstElapsed = elapsed, () => completedCount++));
            yield return null;
            runner.StartCoroutine(WaitAndMeasure(elapsed => secondElapsed = elapsed, () => completedCount++));

            yield return new WaitUntil(() => completedCount == 2);

            Assert.That(firstElapsed, Is.GreaterThanOrEqualTo(Delay));
            Assert.That(secondElapsed, Is.GreaterThanOrEqualTo(Delay));

            Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator CoroutineService_StopsWorkWhenItsHostIsDestroyed()
        {
            var gameObject = new GameObject(nameof(CoroutineService));
            ICoroutineService service = new CoroutineService(gameObject.AddComponent<TestCoroutineRunner>());
            var completed = false;

            service.Run(InvokeAfterDelay(() => completed = true));
            yield return null;
            Object.Destroy(gameObject);
            yield return new WaitForSecondsRealtime(Delay * 2f);

            Assert.That(completed, Is.False);
        }

        [UnityTest]
        public IEnumerator CoroutineOperation_CompletesAndRaisesCompleted()
        {
            var gameObject = new GameObject(nameof(CoroutineOperation_CompletesAndRaisesCompleted));
            var service = new CoroutineService(gameObject.AddComponent<TestCoroutineRunner>());
            ManagedOperationStatus completedStatus = ManagedOperationStatus.Running;

            CoroutineOperation operation = service.RunOperation(CompleteAfterOneFrame());
            IManagedOperation managedOperation = operation;
            operation.Completed += completed => completedStatus = completed.Status;

            Assert.That(managedOperation.IsCompleted, Is.False);
            Assert.That(managedOperation.Exception, Is.Null);

            yield return null;
            yield return null;

            Assert.That(operation.Status, Is.EqualTo(ManagedOperationStatus.Completed));
            Assert.That(completedStatus, Is.EqualTo(ManagedOperationStatus.Completed));
            Assert.That(managedOperation.IsCompleted, Is.True);
            Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator CoroutineOperation_CancelStopsTheRoutine()
        {
            var gameObject = new GameObject(nameof(CoroutineOperation_CancelStopsTheRoutine));
            var service = new CoroutineService(gameObject.AddComponent<TestCoroutineRunner>());
            CoroutineOperation operation = service.RunOperation(WaitForever());

            operation.Cancel();
            yield return null;

            Assert.That(operation.Status, Is.EqualTo(ManagedOperationStatus.Canceled));
            Assert.That(operation.IsCompleted, Is.True);
            Assert.That(operation.Exception, Is.Null);
            Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator CoroutineOperation_FaultsWhenRoutineThrows()
        {
            var gameObject = new GameObject(nameof(CoroutineOperation_FaultsWhenRoutineThrows));
            var service = new CoroutineService(gameObject.AddComponent<TestCoroutineRunner>());

            LogAssert.Expect(LogType.Exception, "InvalidOperationException: Expected operation failure");
            CoroutineOperation operation = service.RunOperation(ThrowAfterOneFrame());
            yield return null;
            yield return null;

            Assert.That(operation.Status, Is.EqualTo(ManagedOperationStatus.Faulted));
            Assert.That(operation.Exception, Is.TypeOf<System.InvalidOperationException>());
            Object.Destroy(gameObject);
        }

        [UnityTest]
        public IEnumerator CoroutineOperation_IsCanceledWhenItsHostIsDestroyed()
        {
            var gameObject = new GameObject(nameof(CoroutineOperation_IsCanceledWhenItsHostIsDestroyed));
            var service = new CoroutineService(gameObject.AddComponent<TestCoroutineRunner>());
            CoroutineOperation operation = service.RunOperation(WaitForever());
            ManagedOperationStatus completedStatus = ManagedOperationStatus.Running;
            operation.Completed += completed => completedStatus = completed.Status;

            Object.Destroy(gameObject);
            yield return null;

            Assert.That(operation.Status, Is.EqualTo(ManagedOperationStatus.Canceled));
            Assert.That(completedStatus, Is.EqualTo(ManagedOperationStatus.Canceled));
        }

        [UnityTest]
        public IEnumerator CoroutineRunner_KeepsGlobalWorkWhenTheActiveSceneChanges()
        {
            CoroutineRunner runner = CoroutineRunner.Instance;
            var completed = false;

            runner.Run(InvokeAfterDelay(() => completed = true));
            yield return null;

            Scene scene = SceneManager.CreateScene(nameof(CoroutineRunner_KeepsGlobalWorkWhenTheActiveSceneChanges));
            SceneManager.SetActiveScene(scene);
            yield return new WaitForSecondsRealtime(Delay * 2f);

            Assert.That(completed, Is.True);
        }

        [UnityTest]
        public IEnumerator CoroutineRunner_StopsWorkWhenItsInstanceIsDestroyed()
        {
            CoroutineRunner runner = CoroutineRunner.Instance;
            var completed = false;

            runner.Run(InvokeAfterDelay(() => completed = true));
            yield return null;
            Object.Destroy(runner.gameObject);
            yield return new WaitForSecondsRealtime(Delay * 2f);

            Assert.That(completed, Is.False);
        }

        [UnityTest]
        public IEnumerator RunInBackground_WaitsForTheTaskBeforeInvokingTheCallback()
        {
            var gameObject = new GameObject(nameof(RunInBackground_WaitsForTheTaskBeforeInvokingTheCallback));
            var runner = gameObject.AddComponent<TestCoroutineRunner>();
            var result = 0;

            runner.RunInBackground(() => 42, completed => result = completed);
            yield return new WaitUntil(() => result != 0);

            Assert.That(result, Is.EqualTo(42));
            Object.Destroy(gameObject);
        }

        private static IEnumerator WaitAndMeasure(System.Action<float> setElapsed, System.Action completed)
        {
            var startTime = Time.time;
            yield return CoroutineHelper.WaitForSeconds(Delay);
            setElapsed(Time.time - startTime);
            completed();
        }

        private static IEnumerator InvokeAfterDelay(System.Action callback)
        {
            yield return new WaitForSecondsRealtime(Delay);
            callback();
        }

        private static IEnumerator CompleteAfterOneFrame()
        {
            yield return null;
        }

        private static IEnumerator WaitForever()
        {
            while (true)
            {
                yield return null;
            }
        }

        private static IEnumerator ThrowAfterOneFrame()
        {
            yield return null;
            throw new System.InvalidOperationException("Expected operation failure");
        }

        private sealed class TestCoroutineRunner : MonoBehaviour
        {
        }
    }
}
