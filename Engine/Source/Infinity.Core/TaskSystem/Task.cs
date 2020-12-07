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

        public static TaskHandle Schedule<T>(this T jobData) where T : struct, ITask
        {
            return new TaskHandle(Task.Factory.StartNew(jobData.Execute));
        }

        public static TaskHandle Schedule<T>(this T jobData, TaskHandle dependsHandle) where T : struct, ITask
        {
            return new TaskHandle(dependsHandle.TaskRef.ContinueWith(jobData.Execute));
        }

        public static TaskHandle Schedule<T>(this T jobData, params TaskHandle[] dependsHandle) where T : struct, ITask
        {
            Task[] dependsTask = new Task[dependsHandle.Length];
            for (int i = 0; i < dependsHandle.Length; i++)
            {
                dependsTask[i] = dependsHandle[i].TaskRef;
            }

            return new TaskHandle(Task.Factory.ContinueWhenAll(dependsTask, jobData.Execute));
        }
    }
}
