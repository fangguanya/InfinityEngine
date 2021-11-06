﻿using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Core.Memory
{
    public static class FMemoryUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyTo<T>(this Span<T> src, IntPtr dsc) where T : struct
        {
            src.CopyTo(new Span<T>(dsc.ToPointer(), src.Length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyTo<T>(this IntPtr src, Span<T> dsc) where T : struct
        {
            new Span<T>(src.ToPointer(), dsc.Length).CopyTo(dsc);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyTo(this IntPtr src, IntPtr dsc, int sizeInBytesToCopy)
        {
            CopyTo(new ReadOnlySpan<byte>(src.ToPointer(), sizeInBytesToCopy), dsc);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CopyTo<T>(this ReadOnlySpan<T> src, IntPtr dsc) where T : struct
        {
            src.CopyTo(new Span<T>(dsc.ToPointer(), src.Length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MemCpy<T>(ReadOnlySpan<T> src, IntPtr dsc) where T : struct
        {
            unsafe
            {
                src.CopyTo(new Span<T>((void*)dsc, src.Length));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MemCpyStride(IntPtr src, IntPtr dsc, int sizeInBytesToCopy)
        {
            unsafe
            {
                Unsafe.CopyBlockUnaligned((void*)dsc, (void*)src, (uint)sizeInBytesToCopy);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* Malloc(in uint size, in uint count, in uint alignment = 64)
        {
            return TerraFX.Interop.Mimalloc.mi_mallocn_aligned(count, size, alignment);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Free(in void* memory, in uint alignment = 64)
        {
            TerraFX.Interop.Mimalloc.mi_free_aligned(memory, alignment);
        }
    }
}