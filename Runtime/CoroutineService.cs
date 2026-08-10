using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jeomseon.Coroutine
{
    public sealed class CoroutineService : ICoroutineService
    {
        private readonly MonoBehaviour _host;
        private readonly HashSet<CoroutineOperation> _operations = new HashSet<CoroutineOperation>();

        public CoroutineService(MonoBehaviour host)
        {
            _host = host ? host : throw new ArgumentNullException(nameof(host));
        }

        public UnityEngine.Coroutine Run(IEnumerator routine)
        {
            if (routine is null) throw new ArgumentNullException(nameof(routine));
            if (!_host) throw new ObjectDisposedException(nameof(CoroutineService));

            return _host.StartCoroutine(routine);
        }

        public CoroutineOperation RunOperation(IEnumerator routine)
        {
            if (routine is null) throw new ArgumentNullException(nameof(routine));
            if (!_host) throw new ObjectDisposedException(nameof(CoroutineService));

            var operation = new CoroutineOperation(Cancel);
            _operations.Add(operation);
            operation.RegisterHostLifetime(_host.destroyCancellationToken, CancelFromHost);
            operation.SetCoroutine(_host.StartCoroutine(Execute(operation, routine)));
            return operation;
        }

        public void Stop(UnityEngine.Coroutine coroutine)
        {
            if (!_host || coroutine is null) return;

            foreach (CoroutineOperation operation in _operations)
            {
                if (operation.Coroutine == coroutine)
                {
                    Cancel(operation);
                    return;
                }
            }

            _host.StopCoroutine(coroutine);
        }

        public void StopAll()
        {
            if (!_host) return;

            foreach (CoroutineOperation operation in new List<CoroutineOperation>(_operations))
            {
                Cancel(operation);
            }

            _host.StopAllCoroutines();
        }

        private IEnumerator Execute(CoroutineOperation operation, IEnumerator routine)
        {
            while (true)
            {
                object current = null;
                var completed = false;
                var faulted = false;
                try
                {
                    if (!routine.MoveNext())
                    {
                        completed = true;
                    }
                    else
                    {
                        current = routine.Current;
                    }
                }
                catch (Exception exception)
                {
                    operation.Fault(exception);
                    _operations.Remove(operation);
                    Debug.LogException(exception, _host);
                    faulted = true;
                }

                if (faulted) yield break;
                if (completed)
                {
                    operation.Complete();
                    _operations.Remove(operation);
                    yield break;
                }

                yield return current;
            }
        }

        private void Cancel(CoroutineOperation operation)
        {
            if (!_operations.Remove(operation)) return;

            if (_host && operation.Coroutine is not null)
            {
                _host.StopCoroutine(operation.Coroutine);
            }

            operation.CancelFromHost();
        }

        private void CancelFromHost(CoroutineOperation operation)
        {
            _operations.Remove(operation);
            operation.CancelFromHost();
        }
    }
}
