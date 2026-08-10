using UnityEngine;

namespace Jeomseon.Coroutine
{
    public sealed class CoroutineCacheSettings : ScriptableObject
    {
        internal const string ResourcePath = "Jeomseon/Coroutines/CoroutineCacheSettings";
        public const string AssetPath = "Assets/Resources/Jeomseon/Coroutines/CoroutineCacheSettings.asset";
        public const int DefaultMaxCachedWaitForSecondsCount = 128;

        [SerializeField] private bool _isWaitForSecondsCacheLimitEnabled;
        [SerializeField, Min(1)] private int _maxCachedWaitForSecondsCount = DefaultMaxCachedWaitForSecondsCount;

        public bool IsWaitForSecondsCacheLimitEnabled => _isWaitForSecondsCacheLimitEnabled;
        public int MaxCachedWaitForSecondsCount => _maxCachedWaitForSecondsCount;

        internal static CoroutineCacheSettings Load() => Resources.Load<CoroutineCacheSettings>(ResourcePath);
    }
}
