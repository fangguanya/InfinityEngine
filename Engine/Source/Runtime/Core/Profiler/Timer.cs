using System.Diagnostics;

namespace InfinityEngine.Core.Profiler
{
    public class FGameTime
    {
        public static double SecondsPerTick { get { return 1.0 / Stopwatch.Frequency; } }
        public static double MilliSecsPerTick { get { return 1000.0f / Stopwatch.Frequency; } }
        public static double MicroSecsPerTick { get { return 1000000.0f / Stopwatch.Frequency; } }

        static float deltaTime;
        public static float DeltaTime { get { return deltaTime; } }

        static float elapsedTime = 0;
        public static float ElapsedTime { get { return elapsedTime; } }

        static int frameIndex = 0;
        public static int FrameIndex { get { return frameIndex; } }

        public static void Tick(in float deltaTime)
        {
            FGameTime.frameIndex += 1;
            FGameTime.deltaTime = deltaTime;
            FGameTime.elapsedTime += elapsedTime;
        }
    }

    public class FTimeProfiler
    {
        private Stopwatch m_Stopwatch;

        public FTimeProfiler()
        {
            m_Stopwatch = new Stopwatch();
        }

        public long microseconds { get { return (long)(m_Stopwatch.ElapsedTicks * FGameTime.MicroSecsPerTick); } }
        public long milliseconds {get { return m_Stopwatch.ElapsedMilliseconds; } }
        public float seconds {get { return m_Stopwatch.ElapsedMilliseconds / 1000.0f; } }
        public void Reset() => m_Stopwatch.Reset();
        public void Start() => m_Stopwatch.Start();
        public void Restart() => m_Stopwatch.Restart();
        public void Stop() => m_Stopwatch.Stop();

    }
}
