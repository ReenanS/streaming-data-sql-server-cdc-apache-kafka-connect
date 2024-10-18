using Domain.Interfaces.BackgroundTask;
using System.Collections.Concurrent;

namespace Worker.BackgroundTask
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems;
        private readonly SemaphoreSlim _signal;
        private object _lock = new object();

        public BackgroundTaskQueue(IConfiguration config)
        {
            _workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
            _signal = new SemaphoreSlim(config.GetSection("ThreadsConfig:InitialSlim").Get<int>());
        }
        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentException(nameof(workItem));
            }

            lock (_lock)
            {
                _workItems.Enqueue(workItem);
            }

            _signal.Release();

        }

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);

            lock (_lock)
            {
                _workItems.TryDequeue(out var workItem);
                return workItem;
            }
        }
    }
}
