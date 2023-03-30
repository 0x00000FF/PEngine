namespace PEngine.Tasks
{
    public interface ITaskQueueSubscriber
    {
        public void NotifyEventCompleted();
    }

}
