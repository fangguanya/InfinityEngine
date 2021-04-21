using System.Threading.Tasks;

namespace InfinityEngine.Core.TaskSystem
{
    public interface ITaskASync
    {
        public abstract void Execute();
    }

    public static class ITaskASyncExtension
    {
        public static FTaskHandle Run<T>(this T taskData) where T : struct, ITaskASync
        {
            return new FTaskHandle(Task.Factory.StartNew(taskData.Execute));
        }

        public static void Run<T>(this T taskData, ref FTaskHandle taskHandle) where T : struct, ITaskASync
        {
            taskHandle = new FTaskHandle(Task.Factory.StartNew(taskData.Execute));
        }
    }
}
