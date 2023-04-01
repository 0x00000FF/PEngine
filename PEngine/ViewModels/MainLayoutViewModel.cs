using PEngine.Extensions;
using PEngine.Shared;
using System.Collections.Concurrent;
using PEngine.Common.DataModels;
using PEngine.Exceptions;
using PEngine.States;
using PEngine.Tasks;

namespace PEngine.ViewModels
{
    public class MainLayoutViewModel : IViewModel<MainLayout>, ITaskQueueSubscriber
    {
        private MainLayout? _layout;
        private UserContext? _userContext;

        private TaskQueue _taskQueue { get; }

        public bool IsAuthenticated => _userContext?.ContextValid ?? false;
        public bool IsBusy => !_taskQueue.IsEmpty;

        public MainLayoutViewModel(UserContext context) : this()
        {
            _userContext = context;
        }

        public MainLayoutViewModel()
        {
            _taskQueue = new ();
        }

        public void Init(MainLayout view)
        {
            _layout = view;
        }

        public void SetPronamaEnabled(bool enabled)
        {
            if (_layout is null)
            {
                return;
            }

            _layout.IsPronamaEnabled = enabled;
            _layout.RequestUpdate();
        }

        public void NotifyEventCompleted()
        {
            _layout?.RequestUpdate();
        }

        public void BusyLoad(Func<Task> task)
        {
            _taskQueue.QueueTask(this, task, null);
        }

        public void BusyLoad(Func<Task> task, Func<PEngineException, Task>? whenFailed)
        {
            _taskQueue.QueueTask(this, task, whenFailed);
        }
    }
}
