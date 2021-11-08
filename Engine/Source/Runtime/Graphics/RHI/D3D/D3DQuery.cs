using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using System.Collections.Generic;
using InfinityEngine.Core.Memory;
using InfinityEngine.Core.Container;
using InfinityEngine.Core.Mathmatics;

namespace InfinityEngine.Graphics.RHI.D3D
{
	internal static class FD3DQueryUtility
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

	public class FD3DQuery : FRHIQuery
	{
		internal FD3DQueryContext context;

		internal FD3DQuery(FRHIQueryContext context) : base(context)
		{
			this.context = (FD3DQueryContext)context;
			this.indexHead = context.AllocateUnuseIndex();
			this.indexLast = context.IsTimeQuery ? context.AllocateUnuseIndex() : -1;
		}

		public override int GetResult()
		{
			return (int)context.queryData[indexHead];
		}

		public override float GetResult(in ulong frequency)
		{
			if (!context.IsTimeQuery) { return -1; }

			double result = (double)(context.queryData[indexLast] - context.queryData[indexHead]);
			return (float)math.round(1000 * (result / frequency) * 100) / 100;
		}

		protected override void Release()
        {
			context.ReleaseUnuseIndex(indexHead);

			if(context.IsTimeQuery)
            {
				context.ReleaseUnuseIndex(indexLast);
			}
		}
    }

	internal class FD3DQueryContext : FRHIQueryContext
	{
		internal FD3DFence queryFence;
		internal ID3D12QueryHeap queryHeap;
		internal ID3D12Resource queryResult;
		internal FD3DCommandBuffer cmdBuffer;

		private TArray<int> m_QueryMap;
		private Stack<FD3DQuery> m_StackPool;

		public override bool IsTimeQuery
		{
			get
			{
				return queryType == EQueryType.Timestamp || queryType == EQueryType.CopyTimestamp;
			}
		}
		public int countAll { get; private set; }
		public override int countActive { get { return countAll - countInactive; } }
		public override int countInactive { get { return m_StackPool.Count; } }

		public FD3DQueryContext(FRHIDevice device, in EQueryType queryType, in int queryCount) : base(device, queryType, queryCount)
		{
			FD3DDevice d3dDevice = (FD3DDevice)device;

			this.IsReadReady = true;
			this.queryType = queryType;
			this.queryCount= (int)queryCount;
			this.queryData = new ulong[queryCount];
			this.queryFence = new FD3DFence(device, null);
			this.m_QueryMap = new TArray<int>(queryCount);
			this.m_StackPool = new Stack<FD3DQuery>(64);
			for (int i = 0; i < queryCount; ++i) { this.m_QueryMap.Add(i); }

			QueryHeapDescription queryHeapDesc;
			queryHeapDesc.Type = (QueryHeapType)queryType;
			queryHeapDesc.Count = queryCount;
			queryHeapDesc.NodeMask = 0;
			this.queryHeap = d3dDevice.nativeDevice.CreateQueryHeap<ID3D12QueryHeap>(queryHeapDesc);

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
			this.cmdBuffer = new FD3DCommandBuffer("QueryCmdBuffer", device, queryType == EQueryType.CopyTimestamp ? EContextType.Copy : EContextType.Graphics);
			this.queryResult = d3dDevice.nativeDevice.CreateCommittedResource<ID3D12Resource>(heapProperties, HeapFlags.None, resourceDesc, ResourceStates.CopyDestination, null);
		}

		public override void Submit(FRHICommandContext commandContext)
        {
			if (IsReadReady) 
			{
				cmdBuffer.Clear();
				cmdBuffer.nativeCmdList.ResolveQueryData(queryHeap, queryType.GetNativeQueryType(), 0, queryCount, queryResult, 0);
				commandContext.SignalQueue(queryFence);
				commandContext.ExecuteQueue(cmdBuffer);
			}
		}

		public override void GetData()
		{
			if (IsReadReady = queryFence.IsCompleted) 
			{
				IntPtr queryResult_Ptr = queryResult.Map(0);
				queryResult_Ptr.CopyTo(queryData.AsSpan());
				queryResult.Unmap(0);
			}
		}

		internal override int AllocateUnuseIndex()
        {
			if (m_QueryMap.length != 0)
			{
				int poolIndex = m_QueryMap[0];
				m_QueryMap.RemoveSwapAtIndex(0);
				return poolIndex;
			}
			return -1;
		}

		internal override void ReleaseUnuseIndex(in int index)
		{
			m_QueryMap.Add(index);
		}

		public override FRHIQuery GetTemporary(string name)
		{
			FD3DQuery query;
			if (m_StackPool.Count == 0)
			{
				query = new FD3DQuery(this);
				countAll++;
			} else {
				query = m_StackPool.Pop();
			}
			return query;
		}

		public override void ReleaseTemporary(FRHIQuery query)
		{
			m_StackPool.Push((FD3DQuery)query);
		}

		protected override void Release()
		{
			foreach (FD3DQuery query in m_StackPool)
			{
				query.Dispose();
			}
			
			queryData = null;
			m_QueryMap = null;
			cmdBuffer?.Dispose();
			queryHeap?.Dispose();
			queryFence?.Dispose();
			queryResult?.Dispose();
		}
	}
}
