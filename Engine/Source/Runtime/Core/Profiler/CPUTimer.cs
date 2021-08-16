using System;
using System.Runtime.InteropServices;

namespace InfinityEngine.Core.Profiler
{
    public unsafe class CPUTimer : IDisposable
    {
        [DllImport("CPUTimer")]
        public static extern IntPtr CreateCPUTimer();

        [DllImport("CPUTimer")]
        public static extern void BeginCPUTimer(IntPtr cpuTimer);

        [DllImport("CPUTimer")]
        public static extern void EndCPUTimer(IntPtr cpuTimer);

        [DllImport("CPUTimer")]
        public static extern long GetCPUTimer(IntPtr cpuTimer);

        [DllImport("CPUTimer")]
        public static extern void ReleaseCPUTimer(IntPtr cpuTimer);

        [DllImport("CPUTimer")]
        public static extern void DoTask(int* IntArray, int BaseCount, int SecondCount);

        
        private IntPtr cpuTimer;

        public CPUTimer()
        {
            cpuTimer = CreateCPUTimer();
        }

        public void Begin()
        {
            BeginCPUTimer(cpuTimer);
        }

        public void End()
        {
            EndCPUTimer(cpuTimer);
        }

        public long GetMillisecond()
        {
            return GetCPUTimer(cpuTimer);
        }

        public void Dispose()
        {
            ReleaseCPUTimer(cpuTimer);
        }
    }
}
