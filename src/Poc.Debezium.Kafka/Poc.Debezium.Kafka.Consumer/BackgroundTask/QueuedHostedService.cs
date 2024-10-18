using Domain.Interfaces.BackgroundTask;

namespace Worker.BackgroundTask
{
    public class QueuedHostedService : BackgroundService
    {
        public IBackgroundTaskQueue TaskQueue { get; set; }
        private static Semaphore _semaphore;

        public QueuedHostedService(IBackgroundTaskQueue taskQueue, IConfiguration config)
        {
            var min = config.GetSection("ThreadsConfig:Min").Get<int>();
            var max = config.GetSection("ThreadsConfig:Max").Get<int>();

            TaskQueue = taskQueue;
            _semaphore = new Semaphore(min, max);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await BackgroundProcessing(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Func<CancellationToken, Task> workItem = await TaskQueue.DequeueAsync(stoppingToken);

                try
                {
                    var thread = new Thread(new ThreadStart(new ThreadWorkItem(workItem, stoppingToken).ThreadProc));
                    thread.Start();
                }
                catch (Exception ex)
                {
                    string message = (workItem is not null) ? $"" : $"";

                    Console.WriteLine(message);
                }
            }

        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }

        public class ThreadWorkItem
        {
            private Func<CancellationToken, Task> workItem;
            private CancellationToken cancellationToken;

            public ThreadWorkItem(Func<CancellationToken, Task> workItem, CancellationToken cancellationToken)
            {
                this.workItem = workItem;
                this.cancellationToken = cancellationToken;
            }

            public void ThreadProc()
            {
                _semaphore.WaitOne();

                this.workItem(this.cancellationToken)
                    .ContinueWith(a =>
                    {
                        _semaphore.Release();
                    });
            }
        }
    }
}
