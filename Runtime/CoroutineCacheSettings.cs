using UnityEngine;
using UnityEngine.Serialization;

namespace Jeomseon.Unity.Coroutines
{
    public sealed class CoroutineCacheSettings : ScriptableObject
    {
        internal const string ResourcePath = "Jeomseon/Coroutines/CoroutineCacheSettings";
        public const string AssetPath = "Assets/Resources/Jeomseon/Coroutines/CoroutineCacheSettings.asset";
        public const int DefaultMaxCachedWaitForSecondsCount = 128;

        [SerializeField, FormerlySerializedAs("_isWaitForSecondsCacheLimitEnabled")] private bool isWaitForSecondsCacheLimitEnabled;
        [SerializeField, Min(1), FormerlySerializedAs("_maxCachedWaitForSecondsCount")] private int maxCachedWaitForSecondsCount = DefaultMaxCachedWaitForSecondsCount;

        public bool IsWaitForSecondsCacheLimitEnabled => isWaitForSecondsCacheLimitEnabled;
        public int MaxCachedWaitForSecondsCount => maxCachedWaitForSecondsCount;

        internal static CoroutineCacheSettings Load() => Resources.Load<CoroutineCacheSettings>(ResourcePath);
    }
}
