using System.Collections;

namespace Jeomseon.Unity.Coroutines
{
    public interface ICoroutineService
    {
        CoroutineOperation RunOperation(IEnumerator routine);
        UnityEngine.Coroutine Run(IEnumerator routine);
        void Stop(UnityEngine.Coroutine coroutine);
        void StopAll();
    }
}
