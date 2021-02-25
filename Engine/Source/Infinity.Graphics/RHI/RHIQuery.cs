using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Native.Utility;

namespace InfinityEngine.Graphics.RHI
{
	public class FRHITimeQuery : UObject
	{
		private float TimeResult;
		private ID3D12QueryHeap Timestamp_Heap;
		private ID3D12Resource Timestamp_Result;

		public FRHITimeQuery(ID3D12Device6 NativeDevice) : base()
		{
			QueryHeapDescription QueryHeapDesc;
			QueryHeapDesc.Type = QueryHeapType.Timestamp;
			QueryHeapDesc.Count = 2;
			QueryHeapDesc.NodeMask = 0;
			Timestamp_Heap = NativeDevice.CreateQueryHeap<ID3D12QueryHeap>(QueryHeapDesc);

            HeapProperties CPUResultProperties;
            {
                CPUResultProperties.Type = HeapType.Readback;
                CPUResultProperties.CPUPageProperty = CpuPageProperty.Unknown;
                CPUResultProperties.MemoryPoolPreference = MemoryPool.Unknown;
                CPUResultProperties.CreationNodeMask = 0;
                CPUResultProperties.VisibleNodeMask = 0;
            }
            ResourceDescription CPUResultDesc;
            {
                CPUResultDesc.Dimension = ResourceDimension.Buffer;
                CPUResultDesc.Alignment = 0;
                CPUResultDesc.Width = sizeof(long) * 2;
                CPUResultDesc.Height = 1;
                CPUResultDesc.DepthOrArraySize = 1;
                CPUResultDesc.MipLevels = 1;
                CPUResultDesc.Format = Format.Unknown;
                CPUResultDesc.SampleDescription.Count = 1;
                CPUResultDesc.SampleDescription.Quality = 0;
                CPUResultDesc.Layout = TextureLayout.RowMajor;
                CPUResultDesc.Flags = ResourceFlags.None;
            }
			Timestamp_Result = NativeDevice.CreateCommittedResource<ID3D12Resource>(CPUResultProperties, HeapFlags.None, CPUResultDesc, ResourceStates.Common, null);
        }

		public void Begin(ID3D12GraphicsCommandList5 NativeCmdList)
		{
			NativeCmdList.EndQuery(Timestamp_Heap, QueryType.Timestamp, 0);
		}

		public void End(ID3D12GraphicsCommandList5 NativeCmdList)
		{
			NativeCmdList.EndQuery(Timestamp_Heap, QueryType.Timestamp, 1);

			NativeCmdList.ResolveQueryData(Timestamp_Heap, QueryType.Timestamp, 0, 2, Timestamp_Result, 0);
		}

		public float GetQueryResult(float TimestampFrequency)
		{
			float[] Timestamp = new float[2];
			IntPtr Timeesult_Ptr = Timestamp_Result.Map(0);
			Timeesult_Ptr.CopyTo(Timestamp.AsSpan());
			TimeResult = (Timestamp[1] - Timestamp[0]) / TimestampFrequency;
			Timestamp_Result.Unmap(0);

			return TimeResult;
		}

		protected override void Disposed()
		{
			Timestamp_Heap?.Dispose();
            Timestamp_Result?.Dispose();
        }
	}

    public class FRHIStatisticsQuery : UObject
    {
        public FRHIStatisticsQuery(ID3D12Device6 NativeDevice) : base()
        {

        }

        protected override void Disposed()
        {

        }
    }

    public class FRHIOcclusionQuery : UObject
	{
		private int OcclusinResult;
		private ID3D12QueryHeap Occlusion_Heap;
		private ID3D12Resource Occlusion_Result;

		public FRHIOcclusionQuery(ID3D12Device6 NativeDevice) : base()
		{
			QueryHeapDescription QueryHeapDesc;
			QueryHeapDesc.Type = QueryHeapType.Occlusion;
			QueryHeapDesc.Count = 1;
			QueryHeapDesc.NodeMask = 0;
			Occlusion_Heap = NativeDevice.CreateQueryHeap<ID3D12QueryHeap>(QueryHeapDesc);

            HeapProperties CPUResultProperties;
            {
				CPUResultProperties.Type = HeapType.Readback;
				CPUResultProperties.CPUPageProperty = CpuPageProperty.Unknown;
				CPUResultProperties.MemoryPoolPreference = MemoryPool.Unknown;
				CPUResultProperties.CreationNodeMask = 0;
				CPUResultProperties.VisibleNodeMask = 0;
            }
            ResourceDescription CPUResultDesc;
            {
				CPUResultDesc.Dimension = ResourceDimension.Buffer;
				CPUResultDesc.Alignment = 0;
				CPUResultDesc.Width = sizeof(long);
				CPUResultDesc.Height = 1;
				CPUResultDesc.DepthOrArraySize = 1;
				CPUResultDesc.MipLevels = 1;
				CPUResultDesc.Format = Format.Unknown;
				CPUResultDesc.SampleDescription.Count = 1;
				CPUResultDesc.SampleDescription.Quality = 0;
				CPUResultDesc.Layout = TextureLayout.RowMajor;
				CPUResultDesc.Flags = ResourceFlags.None;
            }
			Occlusion_Result = NativeDevice.CreateCommittedResource<ID3D12Resource>(CPUResultProperties, HeapFlags.None, CPUResultDesc, ResourceStates.Common, null);
        }

		public void Begin(ID3D12GraphicsCommandList5 NativeCmdList)
		{
			NativeCmdList.BeginQuery(Occlusion_Heap, QueryType.Occlusion, 0);
		}

		public void End(ID3D12GraphicsCommandList5 NativeCmdList)
		{
			NativeCmdList.EndQuery(Occlusion_Heap, QueryType.Occlusion, 0);


			NativeCmdList.ResolveQueryData(Occlusion_Heap, QueryType.Timestamp, 0, 2, Occlusion_Result, 0);
		}

		public int GetQueryResult()
		{
			int[] OcclusinValue = new int[1];
			IntPtr Timeesult_Ptr = Occlusion_Result.Map(0);
			Timeesult_Ptr.CopyTo(OcclusinValue.AsSpan());
			OcclusinResult = OcclusinValue[0];
			Occlusion_Result.Unmap(0);

			return OcclusinResult;
		}

		protected override void Disposed()
		{
			Occlusion_Heap?.Dispose();
			Occlusion_Result?.Dispose();
        }
	}
}
