using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Core.TaskSystem
{
    public struct FTaskHandle
    {
        internal Task TaskRef;

        public FTaskHandle(Task InTask)
        {
            TaskRef = InTask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Complete()
        {
            return TaskRef.IsCompleted;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Wait()
        {
            TaskRef.Wait();
        }
    }
}
