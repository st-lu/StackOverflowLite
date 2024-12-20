namespace Stackoverflow_Lite.BackgroundTasks
{
    public class QueuedHostedService : BackgroundService, IHostedService
    {
        private readonly IBackgroundTaskScheduler _taskScheduler;

        public QueuedHostedService(IBackgroundTaskScheduler taskScheduler)
        {
            _taskScheduler = taskScheduler;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await _taskScheduler.DequeueAsync(stoppingToken);

                try
                {
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
