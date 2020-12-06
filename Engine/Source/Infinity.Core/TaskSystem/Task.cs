using System.Threading.Tasks;

namespace InfinityEngine.Core.TaskSystem
{
    public interface ITask
    {
        public abstract void Execute();

        public void Execute(Task dependsTask)
        {
            Execute();
        }

        public void Execute(Task[] dependsTaskS)
        {
            Execute();
        }
    }

    public static class ITaskExtensions
    {
        public static Task Dispatch<T>(this T jobData) where T : struct, ITask
        {
            return Task.Factory.StartNew(jobData.Execute);
        }

        public static Task Dispatch<T>(this T jobData, Task dependsTask) where T : struct, ITask
        {
            return dependsTask.ContinueWith(jobData.Execute);
        }

        public static Task Dispatch<T>(this T jobData, params Task[] dependsTask) where T : struct, ITask
        {
            return Task.Factory.ContinueWhenAll(dependsTask, jobData.Execute);
        }
    }
}
