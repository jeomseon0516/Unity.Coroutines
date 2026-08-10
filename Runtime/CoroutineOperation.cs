using System;
using System.Threading;
using Jeomseon.Unity.Operations;

namespace Jeomseon.Coroutine
{
    /// <summary>Coroutine 실행의 종료 상태와 취소를 제공하는 핸들입니다.</summary>
    public sealed class CoroutineOperation : IManagedOperation
    {
        private readonly Action<CoroutineOperation> _cancel;
        private CancellationTokenRegistration _hostLifetimeRegistration;

        internal CoroutineOperation(Action<CoroutineOperation> cancel)
        {
            _cancel = cancel;
        }

        /// <inheritdoc />
        public ManagedOperationStatus Status { get; private set; } = ManagedOperationStatus.Running;

        /// <inheritdoc />
        public Exception Exception { get; private set; }

        /// <inheritdoc />
        public bool IsCompleted => Status != ManagedOperationStatus.Running;

        /// <summary>Unity에서 실행 중인 Coroutine을 가져옵니다.</summary>
        public UnityEngine.Coroutine Coroutine { get; private set; }

        /// <inheritdoc />
        public event Action<IManagedOperation> Completed;

        /// <inheritdoc />
        public void Cancel() => _cancel(this);

        internal void SetCoroutine(UnityEngine.Coroutine coroutine) => Coroutine = coroutine;

        internal void RegisterHostLifetime(CancellationToken token, Action<CoroutineOperation> onHostDestroyed)
        {
            _hostLifetimeRegistration = token.Register(() => onHostDestroyed(this));
        }

        internal void Complete() => SetTerminalStatus(ManagedOperationStatus.Completed, null);

        internal void CancelFromHost() => SetTerminalStatus(ManagedOperationStatus.Canceled, null);

        internal void Fault(Exception exception) => SetTerminalStatus(ManagedOperationStatus.Faulted, exception);

        private void SetTerminalStatus(ManagedOperationStatus status, Exception exception)
        {
            if (IsCompleted) return;

            Status = status;
            Exception = exception;
            _hostLifetimeRegistration.Dispose();
            Completed?.Invoke(this);
        }
    }
}
