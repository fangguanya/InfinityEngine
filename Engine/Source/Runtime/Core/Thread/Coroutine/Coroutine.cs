using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Core.Thread.Coroutine
{
    // A container for running multiple routines in parallel. Coroutines can be nested.
    public class FCoroutineDispatcher
    {
        // How many coroutines are currently running.
        public int count
        {
            get { return m_Dunning.Count; }
        }

        private List<float> m_Delays;
        private List<IEnumerator> m_Dunning;

        public FCoroutineDispatcher()
        {
            this.m_Delays = new List<float>(8);
            this.m_Dunning = new List<IEnumerator>(8);
        }

        // Start a coroutine.
        // <returns>A handle to the new coroutine.</returns>
        // <param name="delay">How many seconds to delay before starting.</param>
        // <param name="routine">The routine to run.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FCoroutineRef Start(in float delay, IEnumerator routine)
        {
            m_Delays.Add(delay);
            m_Dunning.Add(routine);
            return new FCoroutineRef(this, routine);
        }

        // Start a coroutine.
        // <returns>A handle to the new coroutine.</returns>
        // <param name="routine">The routine to run.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FCoroutineRef Start(IEnumerator routine)
        {
            return Start(0, routine);
        }

        // Stop the specified routine.
        // <returns>True if the routine was actually stopped.</returns>
        // <param name="routine">The routine to stop.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Stop(IEnumerator routine)
        {
            int i = m_Dunning.IndexOf(routine);
            if (i < 0)
                return false;
            m_Dunning[i] = null;
            m_Delays[i] = 0f;
            return true;
        }

        // Stop the specified routine.
        // <returns>True if the routine was actually stopped.</returns>
        // <param name="routine">The routine to stop.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Stop(in FCoroutineRef routine)
        {
            return routine.Stop();
        }

        // Stop all running routines.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StopAll()
        {
            m_Delays.Clear();
            m_Dunning.Clear();
        }

        // Check if the routine is currently running.
        // <returns>True if the routine is running.</returns>
        // <param name="routine">The routine to check.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsRunning(IEnumerator routine)
        {
            return m_Dunning.Contains(routine);
        }

        // Check if the routine is currently running.
        // <returns>True if the routine is running.</returns>
        // <param name="routine">The routine to check.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsRunning(in FCoroutineRef routine)
        {
            return routine.IsRunning;
        }

        // Update all running coroutines.
        // <returns>True if any routines were updated.</returns>
        // <param name="deltaTime">How many seconds have passed sinced the last update.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool OnUpdate(in float deltaTime)
        {
            if (m_Dunning.Count > 0)
            {
                for (int i = 0; i < m_Dunning.Count; i++)
                {
                    if (m_Delays[i] > 0f)
                    {
                        m_Delays[i] -= deltaTime;
                    }
                    else if (m_Dunning[i] == null || !MoveNext(m_Dunning[i], i))
                    {
                        m_Dunning.RemoveAt(i);
                        m_Delays.RemoveAt(--i);
                    }
                }
                return true;
            }
            return false;
        }

        bool MoveNext(IEnumerator routine, in int index)
        {
            if (routine.Current is IEnumerator)
            {
                if (MoveNext((IEnumerator)routine.Current, index))
                    return true;

                m_Delays[index] = 0f;
            }

            bool result = routine.MoveNext();

            if (routine.Current is float)
                m_Delays[index] = (float)routine.Current;

            return result;
        }
    }

    // A handle to a (potentially running) coroutine.
    public struct FCoroutineRef
    {
        // True if the enumerator is currently running.
        public bool IsRunning
        {
            get { return enumerator != null && dispatcher.IsRunning(enumerator); }
        }
        // Reference to the routine's enumerator.
        public IEnumerator enumerator;
        // Reference to the routine's runner.
        public FCoroutineDispatcher dispatcher;

        // Construct a coroutine. Never call this manually, only use return values from Coroutines.Run().
        // <param name="runner">The routine's runner.</param>
        // <param name="enumerator">The routine's enumerator.</param>
        public FCoroutineRef(FCoroutineDispatcher dispatcher, IEnumerator enumerator)
        {
            this.dispatcher = dispatcher;
            this.enumerator = enumerator;
        }

        // Stop this coroutine if it is running.
        // <returns>True if the coroutine was stopped.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool Stop()
        {
            return IsRunning && dispatcher.Stop(enumerator);
        }

        // A routine to wait until this coroutine has finished running.
        // <returns>The wait enumerator.</returns>
        public IEnumerator Wait()
        {
            if (enumerator != null)
                while (dispatcher.IsRunning(enumerator))
                    yield return null;
        }
    }

}
