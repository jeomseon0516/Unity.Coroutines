using System.Collections;
using Jeomseon.Coroutine;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Jeomseon.Tests
{
    public sealed class CoroutineRunnerLifecycleTests
    {
        private const float Delay = 0.5f;

        private bool _wasEnterPlayModeOptionsEnabled;
        private EnterPlayModeOptions _enterPlayModeOptions;
        private static int _completedBeforeReentryCount;

        [SetUp]
        public void SetUp()
        {
            _wasEnterPlayModeOptionsEnabled = EditorSettings.enterPlayModeOptionsEnabled;
            _enterPlayModeOptions = EditorSettings.enterPlayModeOptions;
            _completedBeforeReentryCount = 0;
        }

        [TearDown]
        public void TearDown()
        {
            EditorSettings.enterPlayModeOptionsEnabled = _wasEnterPlayModeOptionsEnabled;
            EditorSettings.enterPlayModeOptions = _enterPlayModeOptions;
        }

        [UnityTest]
        public IEnumerator CoroutineRunner_DoesNotResumeWorkAfterDomainReloadDisabledReentry()
        {
            EditorSettings.enterPlayModeOptionsEnabled = true;
            EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload;

            yield return new EnterPlayMode();

            CoroutineRunner.Instance.Run(CompleteAfterDelay());
            yield return null;

            yield return new ExitPlayMode();
            yield return new EnterPlayMode();

            CoroutineRunner reenteredRunner = CoroutineRunner.Instance;
            yield return new WaitForSecondsRealtime(Delay * 1.5f);
            bool isRunnerAvailable = reenteredRunner;

            yield return new ExitPlayMode();

            Assert.That(isRunnerAvailable, Is.True);
            Assert.That(_completedBeforeReentryCount, Is.Zero);
        }

        private static IEnumerator CompleteAfterDelay()
        {
            yield return new WaitForSecondsRealtime(Delay);
            _completedBeforeReentryCount++;
        }
    }
}
