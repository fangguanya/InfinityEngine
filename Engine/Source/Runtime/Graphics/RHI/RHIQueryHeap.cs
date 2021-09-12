using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Memory;
using InfinityEngine.Core.Mathmatics;

namespace InfinityEngine.Graphics.RHI
{
	public class FRHITimeQuery : FDisposable
	{
		internal ID3D12QueryHeap timestamp_Heap;
		internal ID3D12Resource timestamp_Result;

		internal FRHITimeQuery(ID3D12Device6 d3dDevice, in bool copyQueue) : base()
		{
			QueryHeapDescription queryHeapDesc;
			queryHeapDesc.Type = (QueryHeapType)(copyQueue ? 5 : 1);
			queryHeapDesc.Count = 2;
			queryHeapDesc.NodeMask = 0;
			timestamp_Heap = d3dDevice.CreateQueryHeap<ID3D12QueryHeap>(queryHeapDesc);

            HeapProperties heapProperties;
            {
				heapProperties.Type = HeapType.Readback;
				heapProperties.CPUPageProperty = CpuPageProperty.Unknown;
				heapProperties.MemoryPoolPreference = MemoryPool.Unknown;
				heapProperties.VisibleNodeMask = 0;
				heapProperties.CreationNodeMask = 0;
            }
            ResourceDescription resourceDesc;
            {
				resourceDesc.Dimension = ResourceDimension.Buffer;
				resourceDesc.Alignment = 0;
				resourceDesc.Width = sizeof(ulong) * 2;
				resourceDesc.Height = 1;
				resourceDesc.DepthOrArraySize = 1;
				resourceDesc.MipLevels = 1;
				resourceDesc.Format = Format.Unknown;
				resourceDesc.SampleDescription.Count = 1;
				resourceDesc.SampleDescription.Quality = 0;
				resourceDesc.Flags = ResourceFlags.None;
				resourceDesc.Layout = TextureLayout.RowMajor;
            }
			timestamp_Result = d3dDevice.CreateCommittedResource<ID3D12Resource>(heapProperties, HeapFlags.None, resourceDesc, ResourceStates.CopyDestination, null);
        }

		public void Begin(ID3D12GraphicsCommandList5 d3dCmdList)
		{
			d3dCmdList.EndQuery(timestamp_Heap, QueryType.Timestamp, 0);
		}

		public void End(ID3D12GraphicsCommandList5 d3dCmdList)
		{
			d3dCmdList.EndQuery(timestamp_Heap, QueryType.Timestamp, 1);
			d3dCmdList.ResolveQueryData(timestamp_Heap, QueryType.Timestamp, 0, 2, timestamp_Result, 0);
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

		protected override void Release()
		{
			timestamp_Heap?.Dispose();
            timestamp_Result?.Dispose();
        }
	}

    public class FRHIStatisticsQuery : FDisposable
	{
		internal FRHIStatisticsQuery(ID3D12Device6 d3dDevice) : base()
        {

        }

        protected override void Release()
        {

        }
    }

    public class FRHIOcclusionQuery : FDisposable
	{
		private int occlusinResult;
		private ID3D12QueryHeap occlusion_Heap;
		private ID3D12Resource occlusion_Result;

		internal FRHIOcclusionQuery(ID3D12Device6 d3dDevice) : base()
		{
			QueryHeapDescription queryHeapDesc;
			queryHeapDesc.Type = QueryHeapType.Occlusion;
			queryHeapDesc.Count = 1;
			queryHeapDesc.NodeMask = 0;
			occlusion_Heap = d3dDevice.CreateQueryHeap<ID3D12QueryHeap>(queryHeapDesc);

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
			occlusion_Result = d3dDevice.CreateCommittedResource<ID3D12Resource>(resultProperties, HeapFlags.None, resultDesc, ResourceStates.Common, null);
        }

		public void Begin(ID3D12GraphicsCommandList5 d3dCmdList)
		{
			d3dCmdList.BeginQuery(occlusion_Heap, QueryType.Occlusion, 0);
		}

		public void End(ID3D12GraphicsCommandList5 d3dCmdList)
		{
			d3dCmdList.EndQuery(occlusion_Heap, QueryType.Occlusion, 0);
			d3dCmdList.ResolveQueryData(occlusion_Heap, QueryType.Timestamp, 0, 2, occlusion_Result, 0);
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

		protected override void Release()
		{
			occlusion_Heap?.Dispose();
			occlusion_Result?.Dispose();
        }
	}
}
