using System;
using TerraFX.Interop;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Core.Memory
{
    public unsafe static class FMemoryUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOf<T>()
        {
            return Unsafe.SizeOf<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo<T>(this Span<T> src, IntPtr dsc) where T : struct
        {
            src.CopyTo(new Span<T>(dsc.ToPointer(), src.Length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo<T>(this IntPtr src, Span<T> dsc) where T : struct
        {
            new Span<T>(src.ToPointer(), dsc.Length).CopyTo(dsc);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(this IntPtr src, IntPtr dsc, in int size)
        {
            CopyTo(new ReadOnlySpan<byte>(src.ToPointer(), size), dsc);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo<T>(this ReadOnlySpan<T> src, IntPtr dsc) where T : struct
        {
            src.CopyTo(new Span<T>(dsc.ToPointer(), src.Length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy<T>(ReadOnlySpan<T> src, IntPtr dsc) where T : struct
        {
            unsafe
            {
                src.CopyTo(new Span<T>((void*)dsc, src.Length));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock(IntPtr src, IntPtr dsc, in int size)
        {
            unsafe
            {
                Unsafe.CopyBlockUnaligned((void*)dsc, (void*)src, (uint)size);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(void* src, void* dsc, in long size)
        {
            Buffer.MemoryCopy(src, dsc, size, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MemMove(void* src, void* dsc, in long size)
        {
            Buffer.MemoryCopy(src, dsc, size, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MemSet(void* src, long size, in byte value)
        {
            byte* ptr = (byte*)src;
            while (size-- > 0) {
                *(ptr + size) = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MemZero(void* dsc, in long size)
        {
            MemSet(dsc, size, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* Malloc(in int size, in int count, in int alignment = 64)
        {
            return Mimalloc.mi_mallocn_aligned((uint)count, (uint)size, (uint)alignment);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* Realloc(void* memory, in int size, in int count, in int alignment = 64)
        {
            return Mimalloc.mi_reallocn_aligned(memory, (uint)count, (uint)size, (uint)alignment);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Free(void* memory, in int alignment = 64)
        {
            Mimalloc.mi_free_aligned(memory, (uint)alignment);
        }
    }
}
