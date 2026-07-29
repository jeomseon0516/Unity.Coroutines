using Jeomseon.Coroutine;
using UnityEngine;

namespace Jeomseon.Samples.Coroutines
{
    public sealed class CoroutineSample : MonoBehaviour
    {
        private void Start()
        {
            this.DoCallWaitForOneFrame(
                () => Debug.Log("한 프레임 뒤에 실행되었습니다."));
        }
    }
}
