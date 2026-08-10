using System.Collections;
using Jeomseon.Coroutine;
using UnityEngine;

namespace Jeomseon.Samples.Coroutines
{
    public sealed class CoroutineSample : MonoBehaviour
    {
        private ICoroutineService _coroutines;

        private void Awake()
        {
            _coroutines = new CoroutineService(this);
        }

        private void Start()
        {
            _coroutines.Run(LogAfterOneFrame());
            CoroutineRunner.Instance.InvokeNextFrame(
                () => Debug.Log("전역 Runner에서 한 프레임 뒤에 실행되었습니다."));
        }

        private static IEnumerator LogAfterOneFrame()
        {
            yield return null;
            Debug.Log("호스트 수명 서비스에서 한 프레임 뒤에 실행되었습니다.");
        }
    }
}
