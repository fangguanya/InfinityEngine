using System.Threading.Tasks;

namespace InfinityEngine.Core.Thread.TaskSystem
{
    public interface ITaskAsync
    {
        public abstract void Execute();
    }

    public static class ITaskAsyncExtension
    {
        public static FTaskRef Run<T>(this T taskData) where T : struct, ITaskAsync
        {
            return new FTaskRef(Task.Factory.StartNew(taskData.Execute));
        }

        public static void Run<T>(this T taskData, ref FTaskRef taskRef) where T : struct, ITaskAsync
        {
            taskRef = new FTaskRef(Task.Factory.StartNew(taskData.Execute));
        }
    }
}
