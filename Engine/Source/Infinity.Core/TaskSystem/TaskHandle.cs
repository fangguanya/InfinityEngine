using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Core.TaskSystem
{
    public struct TaskHandle
    {
        Task TaskRef;

        public TaskHandle(Task InTask)
        {
            TaskRef = InTask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Complete()
        {
            return TaskRef.IsCompleted;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Sync()
        {
            TaskRef.Wait();
        }
    }
}
