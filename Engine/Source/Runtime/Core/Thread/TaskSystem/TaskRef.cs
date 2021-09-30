using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Core.Thread.TaskSystem
{
    public struct FTaskRef
    {
        internal Task task;

        public FTaskRef(Task task)
        {
            this.task = task;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Complete()
        {
            return task == null ? true : task.IsCompleted;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Wait()
        {
            task.Wait();
        }
    }
}
