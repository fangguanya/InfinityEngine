using System.Threading.Tasks;

namespace InfinityEngine.Core.TaskSystem
{
    public interface ITask
    {
        public abstract void Execute();


        internal void Execute(Task dependsTask)
        {
            Execute();
        }

        internal void Execute(Task[] dependsTaskS)
        {
            Execute();
        }
    }

    public static class ITaskExtension
    {
        public static void Run<T>(this T taskData) where T : struct, ITask
        {
            taskData.Execute();
        }

        public static FTaskHandle Schedule<T>(this T taskData) where T : struct, ITask
        {
            return new FTaskHandle(Task.Factory.StartNew(taskData.Execute));
        }

        public static FTaskHandle Schedule<T>(this T taskData, FTaskHandle dependsHandle) where T : struct, ITask
        {
            return new FTaskHandle(dependsHandle.TaskRef.ContinueWith(taskData.Execute));
        }

        public static FTaskHandle Schedule<T>(this T taskData, params FTaskHandle[] dependsHandle) where T : struct, ITask
        {
            Task[] dependsTask = new Task[dependsHandle.Length];
            for (int i = 0; i < dependsHandle.Length; i++)
            {
                dependsTask[i] = dependsHandle[i].TaskRef;
            }

            return new FTaskHandle(Task.Factory.ContinueWhenAll(dependsTask, taskData.Execute));
        }
    }
}
