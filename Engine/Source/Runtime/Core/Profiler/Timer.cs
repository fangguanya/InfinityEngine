using System.Diagnostics;

namespace InfinityEngine.Core.Profiler
{
    public class FTime
    {
        static float delta;
        public static float DeltaTime { get { return delta; } }

        static float elapsed = 0;
        public static float ElapsedTime { get { return elapsed; } }

        static int frameIndex = 0;
        public static int FrameIndex { get { return frameIndex; } }

        public static void Tick(float timeStep)
        {
            frameIndex += 1;
            delta = timeStep;
            elapsed += delta;
        }
    }

    public class FTimeProfiler
    {
        public static double SecondsPerTick { get; }
        public static double MilliSecsPerTick { get; }
        public static double MicroSecsPerTick { get; }

        private Stopwatch stopwatch;
        static FTimeProfiler()
        {
            SecondsPerTick = 0.0;
            long countsPerSec = Stopwatch.Frequency;
            SecondsPerTick = 1.0 / countsPerSec;
            MilliSecsPerTick = 1000.0f / countsPerSec;
            MicroSecsPerTick = 1000000.0f / countsPerSec;
        }

        public FTimeProfiler()
        {
            Debug.Assert(Stopwatch.IsHighResolution,
                "System does not support high-resolution performance counter.");
            stopwatch = new Stopwatch();
        }

        public long microseconds { get { return (long)(stopwatch.ElapsedTicks * MicroSecsPerTick); } }
        public long milliseconds {get { return stopwatch.ElapsedMilliseconds; } }
        public float seconds {get { return stopwatch.ElapsedMilliseconds / 1000.0f; } }
        public void Reset() => stopwatch.Reset();
        public void Start() => stopwatch.Start();
        public void Restart() => stopwatch.Restart();
        public void Stop() => stopwatch.Stop();

    }
}
