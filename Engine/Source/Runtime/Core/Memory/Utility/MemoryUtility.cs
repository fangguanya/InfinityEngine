﻿using System;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Core.Memory
{
    public static class FMemoryUtility
    {
        public static void CopyMemory(IntPtr dest, IntPtr src, int sizeInBytesToCopy)
        {
            unsafe
            {
                Unsafe.CopyBlockUnaligned((void*)dest, (void*)src, (uint)sizeInBytesToCopy);
            }
        }

        public static void CopyMemory<T>(IntPtr dest, ReadOnlySpan<T> src) where T : struct
        {
            unsafe
            {
                src.CopyTo(new Span<T>((void*)dest, src.Length));
            }
        }

        public static unsafe void CopyTo(this IntPtr source, IntPtr destination, int sizeInBytesToCopy)
        {
            CopyTo(new ReadOnlySpan<byte>(source.ToPointer(), sizeInBytesToCopy), destination);
        }

        public static unsafe void CopyTo<T>(this Span<T> source, IntPtr destination) where T : struct
        {
            source.CopyTo(new Span<T>(destination.ToPointer(), source.Length));
        }

        public static unsafe void CopyTo<T>(this ReadOnlySpan<T> source, IntPtr destination) where T : struct
        {
            source.CopyTo(new Span<T>(destination.ToPointer(), source.Length));
        }

        public static unsafe void CopyTo<T>(this IntPtr source, Span<T> destination) where T : struct
        {
            new Span<T>(source.ToPointer(), destination.Length).CopyTo(destination);
        }
    }
}