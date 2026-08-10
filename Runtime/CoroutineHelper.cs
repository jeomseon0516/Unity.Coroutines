using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jeomseon.Coroutine
{
    public static class CoroutineHelper
    {
        public static WaitForEndOfFrame WaitForEndOfFrame { get; } = new WaitForEndOfFrame();
        public static WaitForFixedUpdate WaitForFixedUpdate { get; } = new WaitForFixedUpdate();

        private static readonly Dictionary<float, WaitForSeconds> _waitForSecondsDic = new Dictionary<float, WaitForSeconds>();
        private static CoroutineCacheSettings _cacheSettings;

        public static WaitForSeconds WaitForSeconds(float delayTime)
        {
            if (!_waitForSecondsDic.TryGetValue(delayTime, out WaitForSeconds value))
            {
                value = new WaitForSeconds(delayTime);

                if (CanCacheWaitForSeconds())
                {
                    _waitForSecondsDic.Add(delayTime, value);
                }
            }

            return value;
        }

        internal static int CachedWaitForSecondsCount => _waitForSecondsDic.Count;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        internal static void ResetWaitForSecondsCache()
        {
            _waitForSecondsDic.Clear();
            _cacheSettings = null;
        }

        private static bool CanCacheWaitForSeconds()
        {
            _cacheSettings ??= CoroutineCacheSettings.Load();

            return !_cacheSettings ||
                   !_cacheSettings.IsWaitForSecondsCacheLimitEnabled ||
                   _waitForSecondsDic.Count < _cacheSettings.MaxCachedWaitForSecondsCount;
        }
    }
}
