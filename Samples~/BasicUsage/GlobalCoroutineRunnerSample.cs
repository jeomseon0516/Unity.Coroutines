using Jeomseon.Coroutine;
using UnityEngine;

namespace Jeomseon.Samples.Coroutines
{
    public sealed class GlobalCoroutineRunnerSample : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _delay = 2f;

        private void Start()
        {
            CoroutineRunner.Instance.InvokeAfterSeconds(
                _delay,
                () => Debug.Log("전역 CoroutineRunner 작업이 완료되었습니다."));
        }
    }
}
