using System.Collections;
using Jeomseon.Unity.Coroutines;
using UnityEngine;

namespace Jeomseon.Samples.Coroutines
{
    public sealed class CoroutineOperationSample : MonoBehaviour
    {
        private ICoroutineService _coroutines;

        private void Awake()
        {
            _coroutines = new CoroutineService(this);
        }

        private void Start()
        {
            CoroutineOperation completed = _coroutines.RunOperation(CompleteAfterOneFrame());
            completed.Completed += operation => Debug.Log($"완료 작업 상태: {operation.Status}");

            CoroutineOperation canceled = _coroutines.RunOperation(WaitForever());
            canceled.Completed += operation => Debug.Log($"취소 작업 상태: {operation.Status}");
            StartCoroutine(CancelAfterOneFrame(canceled));
        }

        private static IEnumerator CompleteAfterOneFrame()
        {
            yield return null;
        }

        private static IEnumerator WaitForever()
        {
            while (true)
            {
                yield return null;
            }
        }

        private static IEnumerator CancelAfterOneFrame(CoroutineOperation operation)
        {
            yield return null;
            operation.Cancel();
        }
    }
}
