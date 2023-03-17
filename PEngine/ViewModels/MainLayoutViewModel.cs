using PEngine.Extensions;
using PEngine.Shared;
using System.Collections.Concurrent;

namespace PEngine.ViewModels
{
    public class MainLayoutViewModel : IViewModel
    {
        private MainLayout? Layout { get; set; }

        private ConcurrentQueue<Func<Task>> TaskQueue { get; set; }

        private event Action<MainLayout?, Func<Task>> OnTaskEnqueueRequested;
        private event Action<MainLayout?> OnTaskCompleted;

        public bool IsBusy => !TaskQueue.IsEmpty;

        public MainLayoutViewModel()
        {
            TaskQueue = new ConcurrentQueue<Func<Task>>();

            OnTaskEnqueueRequested += async (c, t) =>
            {
                var cts = new CancellationTokenSource();

                TaskQueue.Enqueue(t);

                c?.RequestUpdate();

                if (TaskQueue.Count == 1)
                {
                    await Task.Run(t, cts.Token);
                }
            };

            OnTaskCompleted += async (c) =>
            {
                if (TaskQueue.TryDequeue(out _))
                {
                    TaskQueue.TryPeek(out var nextTask);

                    if (nextTask is not null)
                    {
                        await nextTask();
                    }
                }

                c?.RequestUpdate();
            };
        }

        public void Init(MainLayout layout)
        {
            Layout = layout;
        }

        public void QueueTask(Action syncTask)
        {
            QueueTask(() =>
            {
                var asynchronizedTask = new Task(syncTask);
                asynchronizedTask.Start();

                return asynchronizedTask;
            });
        }

        public void QueueTask(Func<Task> asyncTask)
        {
            Func<Task> wrappedTask = async () =>
            {
                await asyncTask();
                OnTaskCompleted(Layout);
            };

            OnTaskEnqueueRequested(Layout, wrappedTask);
        }

    }
}
