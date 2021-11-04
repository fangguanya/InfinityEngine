using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using System.Collections.Generic;
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
		CopyTimestamp = 5,
		ComputeTimestamp = 6
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
		internal int indexHead;
		internal int indexLast;
		internal FRHIQueryContext queryContext;

		internal FRHIQuery(FRHIQueryContext queryContext)
		{
			this.queryContext = queryContext;
			this.indexHead = queryContext.AllocateQueryID();
			this.indexLast = queryContext.IsTimeQuery ? queryContext.AllocateQueryID() : -1;
		}

		public int GetResult()
		{
			return (int)queryContext.queryData[indexHead];
		}

		public float GetResult(in ulong frequency)
		{
			if (!queryContext.IsTimeQuery) { return -1; }

			double result = (double)(queryContext.queryData[indexLast] - queryContext.queryData[indexHead]);
			return (float)math.round(1000 * (result / frequency) * 100) / 100;
		}

		protected override void Release()
        {
			queryContext.ReleaseQueryID(indexHead);

			if(queryContext.IsTimeQuery)
            {
				queryContext.ReleaseQueryID(indexLast);
			}
		}
    }

	internal class FRHIQueryContext : FDisposable
	{
		internal int queryCount;
		internal bool IsReadReady;
		internal ulong[] queryData;
		internal EQueryType queryType;

		internal FRHIFence queryFence;
		internal FRHICommandList cmdList;
		internal ID3D12QueryHeap queryHeap;
		internal ID3D12Resource queryResult;

		private TArray<int> m_QueryMap;
		private Stack<FRHIQuery> m_StackPool;

		public bool IsTimeQuery
		{
			get
			{
				return queryType == EQueryType.Timestamp || queryType == EQueryType.CopyTimestamp;
			}
		}
		public int countAll { get; private set; }
		public int countActive { get { return countAll - countInactive; } }
		public int countInactive { get { return m_StackPool.Count; } }

		public FRHIQueryContext(FRHIDevice device, in EQueryType queryType, in int queryCount)
		{
			this.IsReadReady = true;
			this.queryType = queryType;
			this.queryCount= (int)queryCount;
			this.queryData = new ulong[queryCount];
			this.queryFence = new FRHIFence(device, null);
			this.m_QueryMap = new TArray<int>(queryCount);
			this.m_StackPool = new Stack<FRHIQuery>(64);
			for (int i = 0; i < queryCount; ++i) { this.m_QueryMap.Add(i); }

			QueryHeapDescription queryHeapDesc;
			queryHeapDesc.Type = (QueryHeapType)queryType;
			queryHeapDesc.Count = queryCount;
			queryHeapDesc.NodeMask = 0;
			this.queryHeap = device.nativeDevice.CreateQueryHeap<ID3D12QueryHeap>(queryHeapDesc);

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
			this.cmdList = new FRHICommandList("QueryCommandList", device, queryType == EQueryType.CopyTimestamp ? EContextType.Copy : EContextType.Graphics);
			this.queryResult = device.nativeDevice.CreateCommittedResource<ID3D12Resource>(heapProperties, HeapFlags.None, resourceDesc, ResourceStates.CopyDestination, null);
		}

		public void Submit(FRHICommandContext commandContext)
        {
			if (IsReadReady) 
			{
				cmdList.Clear();
				cmdList.nativeCmdList.ResolveQueryData(queryHeap, queryType.GetNativeQueryType(), 0, queryCount, queryResult, 0);
				commandContext.SignalQueue(queryFence);
				commandContext.ExecuteQueue(cmdList);
			}
		}

		public void GetData()
		{
			if (IsReadReady = queryFence.IsCompleted) 
			{
				IntPtr queryResult_Ptr = queryResult.Map(0);
				queryResult_Ptr.CopyTo(queryData.AsSpan());
				queryResult.Unmap(0);
			}
		}

		public int AllocateQueryID()
        {
			if (m_QueryMap.length != 0)
			{
				int poolIndex = m_QueryMap[0];
				m_QueryMap.RemoveSwapAtIndex(0);
				return poolIndex;
			}
			return -1;
		}

		public void ReleaseQueryID(in int index)
		{
			m_QueryMap.Add(index);
		}

		public FRHIQuery GetTemporary(string name)
		{
			FRHIQuery query;
			if (m_StackPool.Count == 0)
			{
				query = new FRHIQuery(this);
				countAll++;
			} else {
				query = m_StackPool.Pop();
			}
			return query;
		}

		public void ReleaseTemporary(FRHIQuery query)
		{
#if WITH_EDITOR // keep heavy checks in editor
            if (m_CollectionCheck && m_Stack.Count > 0)
            {
                if (m_Stack.Contains(element))
                    Console.WriteLine("Internal error. Trying to destroy object that is already released to pool.");
            }
#endif
			m_StackPool.Push(query);
		}

		protected override void Release()
		{
			foreach (FRHIQuery query in m_StackPool)
			{
				query.Dispose();
			}
			
			queryData = null;
			m_QueryMap = null;
			cmdList?.Dispose();
			queryHeap?.Dispose();
			queryFence?.Dispose();
			queryResult?.Dispose();
		}
	}
}
