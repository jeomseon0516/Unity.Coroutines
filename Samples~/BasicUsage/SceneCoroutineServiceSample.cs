using System;
using System.Collections;
using Jeomseon.Coroutine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Jeomseon.Samples.Coroutines
{
    public sealed class SceneCoroutineServiceSample : MonoBehaviour
    {
        [SerializeField, Min(0f), FormerlySerializedAs("_delay")] private float delay = 2f;
        [SerializeField, FormerlySerializedAs("_destroyHostBeforeCompletion")] private bool destroyHostBeforeCompletion;

        private ICoroutineService _coroutines;

        private void Awake()
        {
            _coroutines ??= new CoroutineService(this);
        }

        public void Initialize(ICoroutineService coroutines)
        {
            _coroutines = coroutines ?? throw new ArgumentNullException(nameof(coroutines));
        }

        private void Start()
        {
            CoroutineOperation operation = _coroutines.RunOperation(WaitForDelay());
            operation.Completed += completed =>
                Debug.Log($"호스트 수명 CoroutineService 작업 상태: {completed.Status}");

            if (destroyHostBeforeCompletion && delay > 0f)
            {
                StartCoroutine(DestroyHostBeforeCompletion());
            }
        }

        private IEnumerator WaitForDelay()
        {
            yield return new WaitForSeconds(delay);
        }

        private IEnumerator DestroyHostBeforeCompletion()
        {
            yield return new WaitForSeconds(delay * 0.5f);
            Destroy(gameObject);
        }
    }
}
