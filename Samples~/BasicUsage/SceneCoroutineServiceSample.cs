using System;
using System.Collections;
using Jeomseon.Coroutine;
using UnityEngine;

namespace Jeomseon.Samples.Coroutines
{
    public sealed class SceneCoroutineServiceSample : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _delay = 2f;
        [SerializeField] private bool _destroyHostBeforeCompletion;

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

            if (_destroyHostBeforeCompletion && _delay > 0f)
            {
                StartCoroutine(DestroyHostBeforeCompletion());
            }
        }

        private IEnumerator WaitForDelay()
        {
            yield return new WaitForSeconds(_delay);
        }

        private IEnumerator DestroyHostBeforeCompletion()
        {
            yield return new WaitForSeconds(_delay * 0.5f);
            Destroy(gameObject);
        }
    }
}
