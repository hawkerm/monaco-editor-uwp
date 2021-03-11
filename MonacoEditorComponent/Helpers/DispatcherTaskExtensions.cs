using Microsoft.System;
using System;
using System.Threading.Tasks;

namespace Monaco.Helpers
{
    /// <summary>
    /// https://github.com/Microsoft/Windows-task-snippets/blob/master/tasks/UI-thread-task-await-from-background-thread.md
    /// </summary>
    internal static class DispatcherTaskExtensions
    {
        internal static async Task<T> RunTaskAsync<T>(this DispatcherQueue dispatcher,
            Func<Task<T>> func, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            _ = dispatcher.TryEnqueue(priority, async () =>
            {
                try
                {
                    taskCompletionSource.SetResult(await func());
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetException(ex);
                }
            });
            return await taskCompletionSource.Task;
        }

        // There is no TaskCompletionSource<void> so we use a bool that we throw away.
        internal static async Task RunTaskAsync(this DispatcherQueue dispatcher,
            Func<Task> func, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal) =>
            await RunTaskAsync(dispatcher, async () => { await func(); return false; }, priority);
    }
}
