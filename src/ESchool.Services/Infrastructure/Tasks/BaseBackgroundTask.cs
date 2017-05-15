using System;
using System.Threading;
using System.Threading.Tasks;

namespace ESchool.Services.Infrastructure.Tasks
{
    public abstract class BaseBackgroundTask : IBackgroundTask
    {
        private readonly int _taskId;
        private readonly int _loopInSeconds;

        private Task _backgroundTask;
        private CancellationTokenSource _cancellationTokenSource;

        public BaseBackgroundTask(int taskId, int loopInSeconds)
        {
            _taskId = taskId;
            _loopInSeconds = loopInSeconds;
        }

        public virtual void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _backgroundTask = Task.Factory.StartNew(
                Execute,
                _cancellationTokenSource.Token,
                TaskCreationOptions.LongRunning);
        }

        public virtual void Stop()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }
        }

        public abstract Task Execute();

        private async Task Execute(object state)
        {
            if (state == null || _taskId <= 0 || _loopInSeconds <= 0)
            {
                return;
            }

            int sleepInSeconds = _loopInSeconds * 1000;
            var token = (CancellationToken)state;

            while (true)
            {
                await Task.Delay(sleepInSeconds, token);

                try
                {
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }

                    await Execute();
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (AggregateException)
                {
                }
            }
        }
    }
}
