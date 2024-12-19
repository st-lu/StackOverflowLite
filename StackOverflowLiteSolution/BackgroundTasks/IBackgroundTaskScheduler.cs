namespace Stackoverflow_Lite.BackgroundTasks
{
    public interface IBackgroundTaskScheduler
    {
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);

        Task QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem);
    }
}
