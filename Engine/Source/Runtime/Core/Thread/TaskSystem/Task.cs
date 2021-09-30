using System.Threading.Tasks;

namespace InfinityEngine.Core.Thread.TaskSystem
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

        public static FTaskRef Schedule<T>(this T taskData) where T : struct, ITask
        {
            return new FTaskRef(Task.Factory.StartNew(taskData.Execute));
        }

        public static FTaskRef Schedule<T>(this T taskData, in FTaskRef depend) where T : struct, ITask
        {
            return new FTaskRef(depend.task.ContinueWith(taskData.Execute));
        }

        public static FTaskRef Schedule<T>(this T taskData, params FTaskRef[] depends) where T : struct, ITask
        {
            Task[] dependsTask = new Task[depends.Length];
            for (int i = 0; i < depends.Length; ++i) { dependsTask[i] = depends[i].task; }
            return new FTaskRef(Task.Factory.ContinueWhenAll(dependsTask, taskData.Execute));
        }
    }
}
