using Jeomseon.Coroutine;
using NUnit.Framework;
using UnityEngine;

namespace Jeomseon.Tests
{
    public sealed class CoroutineHelperTests
    {
        [SetUp]
        [TearDown]
        public void ResetCache()
        {
            CoroutineHelper.ResetWaitForSecondsCache();
        }

        [Test]
        public void WaitForSeconds_ReusesCachedInstructionForTheSameDelay()
        {
            var first = CoroutineHelper.WaitForSeconds(1f);
            var second = CoroutineHelper.WaitForSeconds(1f);

            Assert.That(second, Is.SameAs(first));
            Assert.That(CoroutineHelper.CachedWaitForSecondsCount, Is.EqualTo(1));
        }

        [Test]
        public void WaitForSeconds_UsesTheConfiguredCacheLimit()
        {
            SetCacheSettings(isLimitEnabled: true, maxCount: 2);

            CoroutineHelper.WaitForSeconds(0f);
            CoroutineHelper.WaitForSeconds(1f);
            var firstUncached = CoroutineHelper.WaitForSeconds(2f);
            var secondUncached = CoroutineHelper.WaitForSeconds(2f);

            Assert.That(CoroutineHelper.CachedWaitForSecondsCount, Is.EqualTo(2));
            Assert.That(secondUncached, Is.Not.SameAs(firstUncached));
        }

        [Test]
        public void WaitForSeconds_GrowsWithoutLimitByDefault()
        {
            SetCacheSettings(isLimitEnabled: false, maxCount: 2);

            CoroutineHelper.WaitForSeconds(0f);
            CoroutineHelper.WaitForSeconds(1f);
            CoroutineHelper.WaitForSeconds(2f);

            Assert.That(CoroutineHelper.CachedWaitForSecondsCount, Is.EqualTo(3));
        }

        [Test]
        public void ResetWaitForSecondsCache_ClearsCachedInstructions()
        {
            CoroutineHelper.WaitForSeconds(1f);

            CoroutineHelper.ResetWaitForSecondsCache();

            Assert.That(CoroutineHelper.CachedWaitForSecondsCount, Is.Zero);
        }

        private static void SetCacheSettings(bool isLimitEnabled, int maxCount)
        {
            var settings = ScriptableObject.CreateInstance<CoroutineCacheSettings>();
            typeof(CoroutineCacheSettings).GetField("_isWaitForSecondsCacheLimitEnabled", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(settings, isLimitEnabled);
            typeof(CoroutineCacheSettings).GetField("_maxCachedWaitForSecondsCount", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(settings, maxCount);
            typeof(CoroutineHelper).GetField("_cacheSettings", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                .SetValue(null, settings);
        }
    }
}
