using System.Threading.Tasks;

namespace InfinityEngine.Core.TaskSystem
{
    public struct TaskRef
    {
        Task TaskHandle;

        public TaskRef(Task InTask)
        {
            TaskHandle = InTask;
        }

        public bool Complete()
        {
            return TaskHandle.IsCompleted;
        }

        public void Sync()
        {
            TaskHandle.Wait();
        }
    }
}
