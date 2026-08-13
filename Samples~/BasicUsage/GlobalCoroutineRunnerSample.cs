using Jeomseon.Unity.Coroutines;
using UnityEngine;
using UnityEngine.Serialization;

namespace Jeomseon.Samples.Coroutines
{
    public sealed class GlobalCoroutineRunnerSample : MonoBehaviour
    {
        [SerializeField, Min(0f), FormerlySerializedAs("_delay")] private float delay = 2f;

        private void Start()
        {
            CoroutineRunner.Instance.InvokeAfterSeconds(
                delay,
                () => Debug.Log("전역 CoroutineRunner 작업이 완료되었습니다."));
        }
    }
}
