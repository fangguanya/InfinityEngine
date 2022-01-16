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

				case EQueryType.Statistic:
					outType = D3D12_QUERY_TYPE.D3D12_QUERY_TYPE_PIPELINE_STATISTICS;
					break;

				case EQueryType.CopyTimestamp:
					outType = D3D12_QUERY_TYPE.D3D12_QUERY_TYPE_TIMESTAMP;
					break;

				case EQueryType.GenericTimestamp:
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
			this.indexHead = queryContext.Allocate();
			this.indexLast = queryContext.IsTimeQuery ? queryContext.Allocate() : -1;
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
			queryContext.Free(indexHead);

			if(queryContext.IsTimeQuery) {
				queryContext.Free(indexLast);
			}
		}
    }

	internal unsafe class FD3DQueryContext : FRHIQueryContext
	{
		internal ID3D12QueryHeap* queryHeap;
		private TArray<int> m_QueryMap;
		private Stack<FD3DQuery> m_StackPool;
		private FD3DFence m_QueryFence;
		private ID3D12Resource* m_QueryResult;
		private FD3DCommandBuffer m_CmdBuffer;
		public int countAll { get; private set; }
		public override int countActive => (countAll - countInactive);
		public override int countInactive => m_StackPool.Count;
		public override bool IsReady => m_QueryFence.IsCompleted;
		public override bool IsTimeQuery => (queryType == EQueryType.CopyTimestamp || queryType == EQueryType.GenericTimestamp);

		public FD3DQueryContext(FRHIDevice device, FRHICommandContext cmdContext, in EQueryType queryType, in int queryCount, string name) : base(device, queryType, queryCount, name)
		{
			FD3DDevice d3dDevice = (FD3DDevice)device;

			this.queryType = queryType;
			this.m_QueryCount = queryCount;
			this.m_QueryData = new ulong[queryCount];
			this.m_QueryFence = new FD3DFence(device, name);
			this.m_QueryMap = new TArray<int>(queryCount);
			this.m_StackPool = new Stack<FD3DQuery>(64);
			for (int i = 0; i < queryCount; ++i) { this.m_QueryMap.Add(i); }

			ID3D12QueryHeap* heapPtr;
			D3D12_QUERY_HEAP_DESC queryHeapDesc;
			queryHeapDesc.Type = (D3D12_QUERY_HEAP_TYPE)queryType;
			queryHeapDesc.Count = (uint)queryCount;
			queryHeapDesc.NodeMask = 0;
			d3dDevice.nativeDevice->CreateQueryHeap(&queryHeapDesc, Windows.__uuidof<ID3D12QueryHeap>(), (void**)&heapPtr);
			fixed (char* namePtr = name + "_QueryHeap")
			{
				heapPtr->SetName((ushort*)namePtr);
			}
			queryHeap = heapPtr;

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
			ID3D12Resource* resultPtr;
			m_CmdBuffer = new FD3DCommandBuffer(name, device, cmdContext, queryType == EQueryType.CopyTimestamp ? EContextType.Copy : EContextType.Graphics);
			d3dDevice.nativeDevice->CreateCommittedResource(&heapProperties, D3D12_HEAP_FLAGS.D3D12_HEAP_FLAG_NONE, &resourceDesc, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_DEST, null, Windows.__uuidof<ID3D12Resource>(), (void**)&resultPtr);
			fixed (char* namePtr = name + "_QueryResult")
			{
				resultPtr->SetName((ushort*)namePtr);
			}
			m_QueryResult = resultPtr;
		}

		public override void Submit(FRHICommandContext commandContext)
        {
			if (IsReady) 
			{
				m_CmdBuffer.Clear();
				m_CmdBuffer.BeginEvent("ResolveQueryData");
				m_CmdBuffer.nativeCmdList->ResolveQueryData(queryHeap, queryType.GetNativeQueryType(), 0, (uint)m_QueryCount, m_QueryResult, 0);
				m_CmdBuffer.EndEvent();
				commandContext.ExecuteQueue(m_CmdBuffer);
				commandContext.SignalQueue(m_QueryFence);
			}
		}

		public override void ResolveData()
		{
			if (IsReady) 
			{
				void* queryResultPtr;
				D3D12_RANGE range = new D3D12_RANGE(0, 0);
				m_QueryResult->Map(0, &range, &queryResultPtr);
				new IntPtr(queryResultPtr).CopyTo(m_QueryData.AsSpan());
				m_QueryResult->Unmap(0, null);
			}
		}

		internal override int Allocate()
        {
			if (m_QueryMap.length != 0)
			{
				int poolIndex = m_QueryMap[0];
				m_QueryMap.RemoveSwapAtIndex(0);
				return poolIndex;
			}
			return -1;
		}

		internal override void Free(in int index)
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

			m_QueryMap = null;
			m_QueryData = null;
			queryHeap->Release();
			m_CmdBuffer?.Dispose();
			m_QueryFence?.Dispose();
			m_QueryResult->Release();
		}
	}
}
