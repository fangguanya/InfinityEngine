using System.Threading.Tasks;

namespace InfinityEngine.Core.TaskSystem
{
    public interface ITaskParallelFor
    {
        public abstract void Execute(int index);
    }

    /*public static class ITaskParallelForExtensions
    {
        public static Task Dispatch<T>(this T jobData, int arrayLength, int batchCount, Task dependsOn = null) where T : struct, ITask
        {
            return Task.Factory.StartNew(jobData.Execute);
        }
    }*/
}
