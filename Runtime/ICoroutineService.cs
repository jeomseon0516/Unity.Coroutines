using System.Collections;

namespace Jeomseon.Coroutine
{
    public interface ICoroutineService
    {
        CoroutineOperation RunOperation(IEnumerator routine);
        UnityEngine.Coroutine Run(IEnumerator routine);
        void Stop(UnityEngine.Coroutine coroutine);
        void StopAll();
    }
}
