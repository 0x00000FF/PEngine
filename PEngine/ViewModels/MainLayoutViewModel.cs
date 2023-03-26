﻿using PEngine.Extensions;
using PEngine.Shared;
using System.Collections.Concurrent;
using PEngine.States;

namespace PEngine.ViewModels
{
    public class MainLayoutViewModel : IViewModel<MainLayout>
    {
        private MainLayout? _layout;
        private UserContext? _userContext;

        private ConcurrentQueue<Func<Task>> TaskQueue { get; }

        private event Action<MainLayout?, Func<Task>> OnTaskEnqueueRequested;
        private event Action<MainLayout?> OnTaskCompleted;

        public bool IsAuthenticated => _userContext?.ContextValid ?? false;
        public bool IsBusy => !TaskQueue.IsEmpty;

        public void SetPronamaEnabled(bool enabled)
        {
            if (_layout is null) return;
            
            _layout.IsPronamaEnabled = enabled;
            _layout.RequestUpdate();
        }

        public MainLayoutViewModel(UserContext context) : this()
        {
            _userContext = context;
        }
        
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

        public void Init(MainLayout view)
        {
            _layout = view;
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

        public void QueueTask(Func<Task> asyncTask, Func<Task>? whenFailed = null)
        {
            try
            {
                Func<Task> wrappedTask = async () =>
                {
                    await asyncTask();
                    OnTaskCompleted(_layout);
                };

                OnTaskEnqueueRequested(_layout, wrappedTask);
            }
            catch (Exception e)
            {
                whenFailed?.Invoke().Wait();
            }
        }

    }
}
