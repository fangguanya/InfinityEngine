using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Memory;
using InfinityEngine.Core.Mathmatics;

namespace InfinityEngine.Graphics.RHI
{
	public enum EQueryType
    {
		Occlusion = 0,
		Timestamp = 1,
		Statistics = 2,
		CopyTimestamp = 5
	}
	
	internal static class FRHIQueryUtility
    {
		internal static QueryType GetNativeQueryType(this EQueryType queryType)
		{
			QueryType outType = default;
			switch (queryType)
			{
				case EQueryType.Occlusion:
					outType = QueryType.Occlusion;
					break;
				case EQueryType.Timestamp:
					outType = QueryType.Timestamp;
					break;
				case EQueryType.Statistics:
					outType = QueryType.PipelineStatistics;
					break;
				case EQueryType.CopyTimestamp:
					outType = QueryType.Timestamp;
					break;
			}
			return outType;
		}
	}

	public class FRHIQuery : FDisposable
	{
		internal ulong[] queryData;
		internal EQueryType queryType;
		internal ID3D12QueryHeap queryHeap;
		internal ID3D12Resource queryResult;

		internal FRHIQuery(FRHIDevice device, in EQueryType queryType, in ulong count) : base()
		{
			this.queryType = queryType;
			this.queryData = new ulong[count];

			QueryHeapDescription queryHeapDesc;
			queryHeapDesc.Type = (QueryHeapType)queryType;
			queryHeapDesc.Count = 2;
			queryHeapDesc.NodeMask = 0;
			this.queryHeap = device.d3dDevice.CreateQueryHeap<ID3D12QueryHeap>(queryHeapDesc);

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
				resourceDesc.Alignment = 0;
				resourceDesc.Dimension = ResourceDimension.Buffer;
				resourceDesc.Width = sizeof(ulong) * count;
				resourceDesc.Height = 1;
				resourceDesc.DepthOrArraySize = 1;
				resourceDesc.MipLevels = 1;
				resourceDesc.Format = Format.Unknown;
				resourceDesc.Flags = ResourceFlags.None;
				resourceDesc.Layout = TextureLayout.RowMajor;
				resourceDesc.SampleDescription.Count = 1;
				resourceDesc.SampleDescription.Quality = 0;
            }
			this.queryResult = device.d3dDevice.CreateCommittedResource<ID3D12Resource>(heapProperties, HeapFlags.None, resourceDesc, ResourceStates.CopyDestination, null);
        }

		public void RequestReadback(FRHICommandList cmdList)
        {
			cmdList.d3dCmdList.ResolveQueryData(queryHeap, queryType.GetNativeQueryType(), 0, 2, queryResult, 0);
		}

		public float GetQueryResult(float timestampFrequency)
		{
            IntPtr timeesult_Ptr = queryResult.Map(0);
            timeesult_Ptr.CopyTo(queryData.AsSpan());
			queryResult.Unmap(0);

			float timeResult = (float)((queryData[1] - queryData[0]) / timestampFrequency) * 1000;
            return math.round(timeResult * 100) / 100;
        }

		protected override void Release()
		{
			queryHeap?.Dispose();
			queryResult?.Dispose();
        }
	}
}
