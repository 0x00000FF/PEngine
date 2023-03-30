using PEngine.Exceptions;
using PEngine.Shared;
using System.Collections.Concurrent;

namespace PEngine.Tasks
{


    public class TaskQueue
    {
        private readonly ConcurrentQueue<Func<Task>> _queue;

        private delegate Task EnqueueRequestedHandler(ITaskQueueSubscriber subscriber, Task task);

        public event Action<ITaskQueueSubscriber, Func<Task>> OnTaskEnqueueRequested;
        public event Action<ITaskQueueSubscriber> OnTaskCompleted;

        public bool IsEmpty => _queue.IsEmpty;

        public TaskQueue()
        {
            _queue = new ConcurrentQueue<Func<Task>>();

            OnTaskEnqueueRequested += async(sub, fnc) => await OnTaskEnqueueRequestedAsync(sub, fnc);
            OnTaskCompleted += async (sub) => await OnTaskCompletedAsync(sub);
        }

        public async Task OnTaskEnqueueRequestedAsync(ITaskQueueSubscriber subscriber, Func<Task> task)
        {
            var cts = new CancellationTokenSource();

            _queue.Enqueue(task);
            subscriber.NotifyEventCompleted();

            if (_queue.Count == 1)
            {
                await Task.Run(task, cts.Token);
            }
        }

        public async Task OnTaskCompletedAsync(ITaskQueueSubscriber subscriber)
        {
            if (_queue.TryDequeue(out _))
            {
                _queue.TryPeek(out var nextTask);

                if (nextTask is not null)
                {
                    await nextTask();
                    return;
                }

                subscriber.NotifyEventCompleted();
            }
        }

        public void QueueTask(ITaskQueueSubscriber subscriber, Action syncTask)
        {
            QueueTask(subscriber, syncTask, (Action<PEngineException>?)null);
        }

        public void QueueTask(ITaskQueueSubscriber subscriber, Action syncTask, Action<PEngineException>? whenFailedSync)
        {
            QueueTask(subscriber, () => Task.Run(syncTask),
            ex => Task.Run(() => whenFailedSync?.Invoke(ex)));
        }

        public void QueueTask(ITaskQueueSubscriber subscriber, Func<Task> asyncTask)
        {
            QueueTask(subscriber, asyncTask, null);
        }

        public void QueueTask(ITaskQueueSubscriber subscriber, Func<Task> asyncTask, Action<PEngineException> whenFailedSync)
        {
            QueueTask(subscriber, asyncTask,
                ex => Task.Run(() => whenFailedSync?.Invoke(ex)));
        }

        public void QueueTask(ITaskQueueSubscriber subscriber, Action syncTask, Func<PEngineException, Task> whenFailed)
        {
            QueueTask(subscriber, () => Task.Run(syncTask), whenFailed);
        }

        public void QueueTask(ITaskQueueSubscriber subscriber, Func<Task> asyncTask, Func<PEngineException, Task>? whenFailed)
        {
            Func<Task> taskHandler = async () =>
            {
                var spawnedTask = asyncTask();

                try
                {
                    await spawnedTask;
                }
                finally
                {
                    if (spawnedTask.IsFaulted)
                    {
                        var exception = new PEngineException(spawnedTask.Exception);
                        whenFailed?.Invoke(exception).Wait();
                    }

                    OnTaskCompleted(subscriber);
                }
            };

            OnTaskEnqueueRequested(subscriber, taskHandler);
        }

    }
}
