using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Memory;
using InfinityEngine.Core.Container;
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

    public struct FRHIQuery : IDisposable
    {
		internal int indexHead;
		internal int indexLast;
		internal FRHIQueryPool queryPool;

		internal FRHIQuery(FRHIQueryPool queryPool)
		{
			this.queryPool = queryPool;
			this.indexHead = queryPool.AllocateQuery();
			this.indexLast = queryPool.bTimeQuery ? queryPool.AllocateQuery() : -1;
		}

		public int GetResult()
		{
			return (int)queryPool.queryData[indexHead];
		}

		public float GetResult(in float frequency)
		{
			if (!queryPool.bTimeQuery) { return 0; }
			return math.round((float)((queryPool.queryData[indexLast] - queryPool.queryData[indexHead]) / frequency) * 1000 * 100) / 100;
		}

		public void Dispose()
        {
			queryPool.ReleaseQuery(indexHead);

			if(queryPool.bTimeQuery)
            {
				queryPool.ReleaseQuery(indexLast);
			}
		}
    }

	internal class FRHIQueryPool : FDisposable
	{
		internal bool bCopyReady;
		internal int queryCount;
		internal ulong[] queryData;
		internal EQueryType queryType;
		private TArray<int> m_QueryMap;
		internal FRHIFence queryFence;
		internal ID3D12QueryHeap queryHeap;
		internal ID3D12Resource queryResult;
		internal bool bTimeQuery
		{
			get
			{
				return queryType == EQueryType.Timestamp || queryType == EQueryType.CopyTimestamp;
			}
		}

		public FRHIQueryPool(FRHIDevice device, in EQueryType queryType, in int queryCount) : base()
		{
			this.bCopyReady = true;
			this.queryType = queryType;
			this.queryCount= (int)queryCount;
			this.queryData = new ulong[queryCount];
			this.queryFence = new FRHIFence(device, null);
			this.m_QueryMap = new TArray<int>(queryCount);
			for(int i = 0; i < queryCount; ++i)
            {
				m_QueryMap.Add(i);
			}

			QueryHeapDescription queryHeapDesc;
			queryHeapDesc.Type = (QueryHeapType)queryType;
			queryHeapDesc.Count = queryCount;
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
				resourceDesc.Width = sizeof(ulong) * (ulong)queryCount;
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

		public void RequestReadback(FRHIGraphicsContext graphicsContext, FRHICommandList cmdList)
        {
			if (bCopyReady) 
			{
				cmdList.d3dCmdList.ResolveQueryData(queryHeap, queryType.GetNativeQueryType(), 0, queryCount, queryResult, 0);
				graphicsContext.ExecuteCommandList(EContextType.Copy, cmdList);
				graphicsContext.WriteFence(EContextType.Copy, queryFence);
			}
		}

		public void RefreshResult()
		{
			bCopyReady = queryFence.Completed();
			if (bCopyReady) 
			{
				IntPtr queryResult_Ptr = queryResult.Map(0);
				queryResult_Ptr.CopyTo(queryData.AsSpan());
				queryResult.Unmap(0);
			}
		}

		public int AllocateQuery()
        {
			if (m_QueryMap.length != 0)
			{
				int poolIndex = m_QueryMap[0];
				m_QueryMap.RemoveSwapAtIndex(0);
				return poolIndex;
			}
			return -1;
		}

		public void ReleaseQuery(in int index)
		{
			m_QueryMap.Add(index);
		}

		protected override void Release()
		{
			queryData = null;
			m_QueryMap = null;
			queryHeap?.Dispose();
			queryFence?.Dispose();
			queryResult?.Dispose();
        }
	}
}
