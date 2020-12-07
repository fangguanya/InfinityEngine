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

    public static class ITaskExtensions
    {
        public static void Run<T>(this T jobData) where T : struct, ITask
        {
            jobData.Execute();
        }

        public static Task Schedule<T>(this T jobData) where T : struct, ITask
        {
            return Task.Factory.StartNew(jobData.Execute);
        }

        public static Task Schedule<T>(this T jobData, Task dependsTask) where T : struct, ITask
        {
            return dependsTask.ContinueWith(jobData.Execute);
        }

        public static Task Schedule<T>(this T jobData, params Task[] dependsTask) where T : struct, ITask
        {
            return Task.Factory.ContinueWhenAll(dependsTask, jobData.Execute);
        }
    }
}
