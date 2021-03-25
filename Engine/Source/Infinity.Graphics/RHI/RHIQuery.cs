using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Mathmatics;
using InfinityEngine.Core.Native.Utility;

namespace InfinityEngine.Graphics.RHI
{
	public class FRHITimeQuery : UObject
	{
		private ID3D12QueryHeap timestamp_Heap;
		private ID3D12Resource timestamp_Result;

		public FRHITimeQuery(ID3D12Device6 d3D12Device) : base()
		{
			QueryHeapDescription queryHeapDesc;
			queryHeapDesc.Type = QueryHeapType.Timestamp;
			queryHeapDesc.Count = 2;
			queryHeapDesc.NodeMask = 0;
			timestamp_Heap = d3D12Device.CreateQueryHeap<ID3D12QueryHeap>(queryHeapDesc);

            HeapProperties resultProperties;
            {
				resultProperties.Type = HeapType.Readback;
				resultProperties.CPUPageProperty = CpuPageProperty.Unknown;
				resultProperties.MemoryPoolPreference = MemoryPool.Unknown;
				resultProperties.CreationNodeMask = 0;
				resultProperties.VisibleNodeMask = 0;
            }
            ResourceDescription resultDesc;
            {
				resultDesc.Dimension = ResourceDimension.Buffer;
				resultDesc.Alignment = 0;
				resultDesc.Width = sizeof(ulong) * 2;
				resultDesc.Height = 1;
				resultDesc.DepthOrArraySize = 1;
				resultDesc.MipLevels = 1;
				resultDesc.Format = Format.Unknown;
				resultDesc.SampleDescription.Count = 1;
				resultDesc.SampleDescription.Quality = 0;
				resultDesc.Layout = TextureLayout.RowMajor;
				resultDesc.Flags = ResourceFlags.None;
            }
			timestamp_Result = d3D12Device.CreateCommittedResource<ID3D12Resource>(resultProperties, HeapFlags.None, resultDesc, ResourceStates.Common, null);
        }

		public void Begin(ID3D12GraphicsCommandList5 d3d12CmdList)
		{
			d3d12CmdList.EndQuery(timestamp_Heap, QueryType.Timestamp, 0);
		}

		public void End(ID3D12GraphicsCommandList5 d3d12CmdList)
		{
			d3d12CmdList.EndQuery(timestamp_Heap, QueryType.Timestamp, 1);
			d3d12CmdList.ResolveQueryData(timestamp_Heap, QueryType.Timestamp, 0, 2, timestamp_Result, 0);
		}

		public float GetQueryResult(float timestampFrequency)
		{
            ulong[] timestamp = new ulong[2];
            IntPtr timeesult_Ptr = timestamp_Result.Map(0);
            timeesult_Ptr.CopyTo(timestamp.AsSpan());
            timestamp_Result.Unmap(0);

			float timeResult = (float)((timestamp[1] - timestamp[0]) / timestampFrequency) * 1000;
            return math.round(timeResult * 100) / 100;
        }

		protected override void Disposed()
		{
			timestamp_Heap?.Dispose();
            timestamp_Result?.Dispose();
        }
	}

    public class FRHIStatisticsQuery : UObject
    {
        public FRHIStatisticsQuery(ID3D12Device6 d3D12Device) : base()
        {

        }

        protected override void Disposed()
        {

        }
    }

    public class FRHIOcclusionQuery : UObject
	{
		private int occlusinResult;
		private ID3D12QueryHeap occlusion_Heap;
		private ID3D12Resource occlusion_Result;

		public FRHIOcclusionQuery(ID3D12Device6 d3D12Device) : base()
		{
			QueryHeapDescription queryHeapDesc;
			queryHeapDesc.Type = QueryHeapType.Occlusion;
			queryHeapDesc.Count = 1;
			queryHeapDesc.NodeMask = 0;
			occlusion_Heap = d3D12Device.CreateQueryHeap<ID3D12QueryHeap>(queryHeapDesc);

            HeapProperties resultProperties;
            {
				resultProperties.Type = HeapType.Readback;
				resultProperties.CPUPageProperty = CpuPageProperty.Unknown;
				resultProperties.MemoryPoolPreference = MemoryPool.Unknown;
				resultProperties.CreationNodeMask = 0;
				resultProperties.VisibleNodeMask = 0;
            }
            ResourceDescription resultDesc;
            {
				resultDesc.Dimension = ResourceDimension.Buffer;
				resultDesc.Alignment = 0;
				resultDesc.Width = sizeof(long);
				resultDesc.Height = 1;
				resultDesc.DepthOrArraySize = 1;
				resultDesc.MipLevels = 1;
				resultDesc.Format = Format.Unknown;
				resultDesc.SampleDescription.Count = 1;
				resultDesc.SampleDescription.Quality = 0;
				resultDesc.Layout = TextureLayout.RowMajor;
				resultDesc.Flags = ResourceFlags.None;
            }
			occlusion_Result = d3D12Device.CreateCommittedResource<ID3D12Resource>(resultProperties, HeapFlags.None, resultDesc, ResourceStates.Common, null);
        }

		public void Begin(ID3D12GraphicsCommandList5 d3d12CmdList)
		{
			d3d12CmdList.BeginQuery(occlusion_Heap, QueryType.Occlusion, 0);
		}

		public void End(ID3D12GraphicsCommandList5 d3d12CmdList)
		{
			d3d12CmdList.EndQuery(occlusion_Heap, QueryType.Occlusion, 0);
			d3d12CmdList.ResolveQueryData(occlusion_Heap, QueryType.Timestamp, 0, 2, occlusion_Result, 0);
		}

		public int GetQueryResult()
		{
			int[] occlusinValue = new int[1];
			IntPtr occlusin_Ptr = occlusion_Result.Map(0);
			occlusin_Ptr.CopyTo(occlusinValue.AsSpan());
			occlusinResult = occlusinValue[0];
			occlusion_Result.Unmap(0);

			return occlusinResult;
		}

		protected override void Disposed()
		{
			occlusion_Heap?.Dispose();
			occlusion_Result?.Dispose();
        }
	}
}
