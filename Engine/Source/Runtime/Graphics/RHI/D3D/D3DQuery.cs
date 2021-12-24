using System;
using TerraFX.Interop.Windows;
using TerraFX.Interop.DirectX;
using System.Collections.Generic;
using InfinityEngine.Core.Memory;
using InfinityEngine.Core.Container;
using InfinityEngine.Core.Mathmatics;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Graphics.RHI.D3D
{
	internal static class FD3DQueryUtility
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static D3D12_QUERY_TYPE GetNativeQueryType(this EQueryType queryType)
		{
			D3D12_QUERY_TYPE outType = default;
			switch (queryType)
			{
				case EQueryType.Occlusion:
					outType = D3D12_QUERY_TYPE.D3D12_QUERY_TYPE_OCCLUSION;
					break;
				case EQueryType.Timestamp:
					outType = D3D12_QUERY_TYPE.D3D12_QUERY_TYPE_TIMESTAMP;
					break;
				case EQueryType.Statistics:
					outType = D3D12_QUERY_TYPE.D3D12_QUERY_TYPE_PIPELINE_STATISTICS;
					break;
				case EQueryType.CopyTimestamp:
					outType = D3D12_QUERY_TYPE.D3D12_QUERY_TYPE_TIMESTAMP;
					break;
			}
			return outType;
		}
	}

	public class FD3DQuery : FRHIQuery
	{
		internal FD3DQueryContext queryContext;

		internal FD3DQuery(FRHIQueryContext queryContext) : base(queryContext)
		{
			this.queryContext = (FD3DQueryContext)queryContext;
			this.indexHead = queryContext.AllocateUnuseIndex();
			this.indexLast = queryContext.IsTimeQuery ? queryContext.AllocateUnuseIndex() : -1;
		}

		public override int GetResult()
		{
			return (int)queryContext.queryData[indexHead];
		}

		public override float GetResult(in ulong frequency)
		{
			if (!queryContext.IsTimeQuery) { return -1; }

			double result = (double)(queryContext.queryData[indexLast] - queryContext.queryData[indexHead]);
			return (float)math.round(1000 * (result / frequency) * 100) / 100;
		}

		protected override void Release()
        {
			queryContext.ReleaseUnuseIndex(indexHead);

			if(queryContext.IsTimeQuery) {
				queryContext.ReleaseUnuseIndex(indexLast);
			}
		}
    }

	internal unsafe class FD3DQueryContext : FRHIQueryContext
	{
		internal FD3DFence queryFence;
		internal ID3D12QueryHeap* queryHeap;
		internal ID3D12Resource* queryResult;
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

			this.IsReady = true;
			this.queryType = queryType;
			this.queryCount= queryCount;
			this.queryData = new ulong[queryCount];
			this.queryFence = new FD3DFence(device);
			this.m_QueryMap = new TArray<int>(queryCount);
			this.m_StackPool = new Stack<FD3DQuery>(64);
			for (int i = 0; i < queryCount; ++i) { this.m_QueryMap.Add(i); }

			ID3D12QueryHeap* heapPtr = null;
			D3D12_QUERY_HEAP_DESC queryHeapDesc;
			queryHeapDesc.Type = (D3D12_QUERY_HEAP_TYPE)queryType;
			queryHeapDesc.Count = (uint)queryCount;
			queryHeapDesc.NodeMask = 0;
			d3dDevice.nativeDevice->CreateQueryHeap(&queryHeapDesc, Windows.__uuidof<ID3D12QueryHeap>(), (void**)&heapPtr);
			this.queryHeap = heapPtr;

			D3D12_HEAP_PROPERTIES heapProperties;
            {
				heapProperties.Type = D3D12_HEAP_TYPE.D3D12_HEAP_TYPE_READBACK;
				heapProperties.CPUPageProperty = D3D12_CPU_PAGE_PROPERTY.D3D12_CPU_PAGE_PROPERTY_UNKNOWN;
				heapProperties.MemoryPoolPreference = D3D12_MEMORY_POOL.D3D12_MEMORY_POOL_UNKNOWN;
				heapProperties.VisibleNodeMask = 0;
				heapProperties.CreationNodeMask = 0;
            }
			D3D12_RESOURCE_DESC resourceDesc;
            {
				resourceDesc.Alignment = 0;
				resourceDesc.Dimension = D3D12_RESOURCE_DIMENSION.D3D12_RESOURCE_DIMENSION_BUFFER;
				resourceDesc.Width = sizeof(ulong) * (ulong)queryCount;
				resourceDesc.Height = 1;
				resourceDesc.DepthOrArraySize = 1;
				resourceDesc.MipLevels = 1;
				resourceDesc.SampleDesc.Count = 1;
				resourceDesc.SampleDesc.Quality = 0;
				resourceDesc.Format = DXGI_FORMAT.DXGI_FORMAT_UNKNOWN;
				resourceDesc.Flags = D3D12_RESOURCE_FLAGS.D3D12_RESOURCE_FLAG_NONE;
				resourceDesc.Layout = D3D12_TEXTURE_LAYOUT.D3D12_TEXTURE_LAYOUT_ROW_MAJOR;
            }
			ID3D12Resource* resultPtr = null;
			this.cmdBuffer = new FD3DCommandBuffer("QueryCmdBuffer", device, queryType == EQueryType.CopyTimestamp ? EContextType.Copy : EContextType.Render);
			d3dDevice.nativeDevice->CreateCommittedResource(&heapProperties, D3D12_HEAP_FLAGS.D3D12_HEAP_FLAG_NONE, &resourceDesc, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_DEST, null, Windows.__uuidof<ID3D12Resource>(), (void**)&resultPtr);
			this.queryResult = resultPtr;
		}

		public override void Submit(FRHICommandContext commandContext)
        {
			if (IsReady) 
			{
				cmdBuffer.Clear();
				cmdBuffer.nativeCmdList->ResolveQueryData(queryHeap, queryType.GetNativeQueryType(), 0, (uint)queryCount, queryResult, 0);
				commandContext.SignalQueue(queryFence);
				commandContext.ExecuteQueue(cmdBuffer);
			}
		}

		public override void GetData()
		{
			if (IsReady = queryFence.IsCompleted) 
			{
				void* queryResult_Ptr = null;
				D3D12_RANGE range = new D3D12_RANGE(0, 0);
				queryResult->Map(0, &range, &queryResult_Ptr);
				new IntPtr(queryResult_Ptr).CopyTo(queryData.AsSpan());
				queryResult->Unmap(0, null);
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
			queryHeap->Release();
			queryFence?.Dispose();
			queryResult->Release();
		}
	}
}
