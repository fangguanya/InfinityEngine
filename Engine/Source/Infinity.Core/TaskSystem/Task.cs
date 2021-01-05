using System.Threading.Tasks;

namespace InfinityEngine.Core.TaskSystem
{
    public interface ITask
    {
        public abstract void Execute();

        internal void Run()
        {
            Execute();
        }

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
        public static void Run<T>(this T jobData) where T : struct, ITask
        {
            jobData.Execute();
        }

        public static FTaskHandle Schedule<T>(this T jobData) where T : struct, ITask
        {
            return new FTaskHandle(Task.Factory.StartNew(jobData.Execute));
        }

        public static FTaskHandle Schedule<T>(this T jobData, FTaskHandle dependsHandle) where T : struct, ITask
        {
            return new FTaskHandle(dependsHandle.TaskRef.ContinueWith(jobData.Execute));
        }

        public static FTaskHandle Schedule<T>(this T jobData, params FTaskHandle[] dependsHandle) where T : struct, ITask
        {
            Task[] dependsTask = new Task[dependsHandle.Length];
            for (int i = 0; i < dependsHandle.Length; i++)
            {
                dependsTask[i] = dependsHandle[i].TaskRef;
            }

            return new FTaskHandle(Task.Factory.ContinueWhenAll(dependsTask, jobData.Execute));
        }
    }
}
