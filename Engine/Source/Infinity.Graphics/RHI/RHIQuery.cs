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
		private ID3D12Resource Timestamp_GPUResult;
		private ID3D12Resource Timestamp_CPUResult;

		private ID3D12GraphicsCommandList6 NativeCmdList;

		public FRHITimeQuery(ID3D12Device6 InNativeDevice, ID3D12GraphicsCommandList6 InNativeCmdList) : base()
		{
			NativeCmdList = InNativeCmdList;

			QueryHeapDescription QueryHeapDesc;
			QueryHeapDesc.Type = QueryHeapType.Timestamp;
			QueryHeapDesc.Count = 2;
			QueryHeapDesc.NodeMask = 0;
			Timestamp_Heap = InNativeDevice.CreateQueryHeap<ID3D12QueryHeap>(QueryHeapDesc);


            HeapProperties GPUResultProperties;
            {
				GPUResultProperties.Type = HeapType.Default;
				GPUResultProperties.CPUPageProperty = CpuPageProperty.Unknown;
				GPUResultProperties.MemoryPoolPreference = MemoryPool.Unknown;
				GPUResultProperties.CreationNodeMask = 0;
				GPUResultProperties.VisibleNodeMask = 0;
            }
            ResourceDescription GPUResultDesc;
            {
				GPUResultDesc.Dimension = ResourceDimension.Buffer;
				GPUResultDesc.Alignment = 0;
				GPUResultDesc.Width = sizeof(long) * 2;
				GPUResultDesc.Height = 1;
				GPUResultDesc.DepthOrArraySize = 1;
				GPUResultDesc.MipLevels = 1;
				GPUResultDesc.Format = Format.Unknown;
				GPUResultDesc.SampleDescription.Count = 1;
				GPUResultDesc.SampleDescription.Quality = 0;
				GPUResultDesc.Layout = TextureLayout.RowMajor;
				GPUResultDesc.Flags = ResourceFlags.None;
            }
            Timestamp_GPUResult = InNativeDevice.CreateCommittedResource(GPUResultProperties, HeapFlags.None, GPUResultDesc, ResourceStates.Predication, null);


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
            Timestamp_CPUResult = InNativeDevice.CreateCommittedResource(CPUResultProperties, HeapFlags.None, CPUResultDesc, ResourceStates.CopyDestination, null);
        }

		public void Begin()
		{
			NativeCmdList.EndQuery(Timestamp_Heap, QueryType.Timestamp, 0);
		}

		public void End()
		{
			NativeCmdList.EndQuery(Timestamp_Heap, QueryType.Timestamp, 1);

			NativeCmdList.ResourceBarrierTransition(Timestamp_GPUResult, ResourceStates.Predication, ResourceStates.CopyDestination, 0);
			NativeCmdList.ResolveQueryData(Timestamp_Heap, QueryType.Timestamp, 0, 2, Timestamp_GPUResult, 0);
			NativeCmdList.ResourceBarrierTransition(Timestamp_GPUResult, ResourceStates.CopyDestination, ResourceStates.CopySource, 0);
			NativeCmdList.CopyResource(Timestamp_CPUResult, Timestamp_GPUResult);
			NativeCmdList.ResourceBarrierTransition(Timestamp_GPUResult, ResourceStates.CopySource, ResourceStates.Predication, 0);
		}

		public float GetQueryResult(float TimestampFrequency)
		{
			// GetData
			float[] Timestamp = new float[2];
			IntPtr Timeesult_Ptr = Timestamp_CPUResult.Map(0);
			Timeesult_Ptr.CopyTo(Timestamp.AsSpan());
			TimeResult = (Timestamp[1] - Timestamp[0]) / TimestampFrequency;
			Timestamp_CPUResult.Unmap(0);

			return TimeResult;
		}

		protected override void DisposeManaged()
		{

		}

		protected override void DisposeUnManaged()
		{
			Timestamp_Heap.Release();
			Timestamp_CPUResult.Release();
			Timestamp_GPUResult.Release();

			Timestamp_Heap.Dispose();
			Timestamp_CPUResult.Dispose();
			Timestamp_GPUResult.Dispose();
		}
	}

    public class FRHIStatisticsQuery : UObject
    {
        public FRHIStatisticsQuery(ID3D12Device6 InNativeDevice, ID3D12GraphicsCommandList6 InNativeCmdList) : base()
        {

        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {

        }
    }

    public class FRHIOcclusionQuery : UObject
	{
		private int OcclusinResult;
		private ID3D12QueryHeap Occlusion_Heap;
		private ID3D12Resource Occlusion_CPUResult;
		private ID3D12Resource Occlusion_GPUResult;

		private ID3D12GraphicsCommandList6 NativeCmdList;

		public FRHIOcclusionQuery(ID3D12Device6 InNativeDevice, ID3D12GraphicsCommandList6 InNativeCmdList) : base()
		{
			NativeCmdList = InNativeCmdList;

			QueryHeapDescription QueryHeapDesc;
			QueryHeapDesc.Type = QueryHeapType.Occlusion;
			QueryHeapDesc.Count = 1;
			QueryHeapDesc.NodeMask = 0;
			Occlusion_Heap = InNativeDevice.CreateQueryHeap<ID3D12QueryHeap>(QueryHeapDesc);


			HeapProperties GPUResultProperties;
			{
				GPUResultProperties.Type = HeapType.Default;
				GPUResultProperties.CPUPageProperty = CpuPageProperty.Unknown;
				GPUResultProperties.MemoryPoolPreference = MemoryPool.Unknown;
				GPUResultProperties.CreationNodeMask = 0;
				GPUResultProperties.VisibleNodeMask = 0;
			}
			ResourceDescription GPUResultDesc;
			{
				GPUResultDesc.Dimension = ResourceDimension.Buffer;
				GPUResultDesc.Alignment = 0;
				GPUResultDesc.Width = sizeof(long);
				GPUResultDesc.Height = 1;
				GPUResultDesc.DepthOrArraySize = 1;
				GPUResultDesc.MipLevels = 1;
				GPUResultDesc.Format = Format.Unknown;
				GPUResultDesc.SampleDescription.Count = 1;
				GPUResultDesc.SampleDescription.Quality = 0;
				GPUResultDesc.Layout = TextureLayout.RowMajor;
				GPUResultDesc.Flags = ResourceFlags.None;
			}
			Occlusion_GPUResult = InNativeDevice.CreateCommittedResource(GPUResultProperties, HeapFlags.None, GPUResultDesc, ResourceStates.Predication, null);


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
			Occlusion_CPUResult = InNativeDevice.CreateCommittedResource(CPUResultProperties, HeapFlags.None, CPUResultDesc, ResourceStates.CopyDestination, null);
        }

		public void Begin()
		{
			NativeCmdList.BeginQuery(Occlusion_Heap, QueryType.Occlusion, 0);
		}

		public void End()
		{
			NativeCmdList.EndQuery(Occlusion_Heap, QueryType.Timestamp, 0);


            NativeCmdList.ResourceBarrierTransition(Occlusion_GPUResult, ResourceStates.Predication, ResourceStates.CopyDestination, 0);
			NativeCmdList.ResolveQueryData(Occlusion_Heap, QueryType.Occlusion, 0, 1, Occlusion_GPUResult, 0);
			NativeCmdList.ResourceBarrierTransition(Occlusion_GPUResult, ResourceStates.CopyDestination, ResourceStates.CopySource, 0);
            NativeCmdList.CopyResource(Occlusion_CPUResult, Occlusion_GPUResult);
            NativeCmdList.ResourceBarrierTransition(Occlusion_GPUResult, ResourceStates.CopySource, ResourceStates.Predication, 0);
        }

		public int GetQueryResult()
		{
			// ReadData	
			int[] OcclusinValue = new int[1];
			IntPtr Timeesult_Ptr = Occlusion_CPUResult.Map(0);
			Timeesult_Ptr.CopyTo(OcclusinValue.AsSpan());
			OcclusinResult = OcclusinValue[0];
			Occlusion_CPUResult.Unmap(0);

			return OcclusinResult;
		}

		protected override void DisposeManaged()
		{

		}

		protected override void DisposeUnManaged()
		{
			Occlusion_Heap.Release();
			Occlusion_CPUResult.Release();
			Occlusion_GPUResult.Release();

			Occlusion_Heap.Dispose();
			Occlusion_CPUResult.Dispose();
			Occlusion_GPUResult.Dispose();
        }
	}

	/*public class D3D12TimeQuery : ManagedObject
{
	private bool IsReadReady;
	private float TimeResult;
	private D3D12Fence Fence;
	private ID3D12QueryHeap Timestamp_Heap;
	private ID3D12Resource Timestamp_Result;

	public D3D12TimeQuery(ID3D12Device6 D3D12Device, D3D12Fence GPUFence) : base()
	{
		IsReadReady = true;
		Fence = GPUFence;

		QueryHeapDescription QueryHeapDesc;
		QueryHeapDesc.Type = QueryHeapType.Timestamp;
		QueryHeapDesc.Count = 2;
		QueryHeapDesc.NodeMask = 0;
		Timestamp_Heap = D3D12Device.CreateQueryHeap<ID3D12QueryHeap>(QueryHeapDesc);

		HeapProperties ResultBufferHeapProperties;
		{
			ResultBufferHeapProperties.Type = HeapType.Readback;
			ResultBufferHeapProperties.CPUPageProperty = CpuPageProperty.Unknown;
			ResultBufferHeapProperties.MemoryPoolPreference = MemoryPool.Unknown;
			ResultBufferHeapProperties.CreationNodeMask = 0;
			ResultBufferHeapProperties.VisibleNodeMask = 0;
		}
		ResourceDescription ResultBufferDesc;
		{
			ResultBufferDesc.Dimension = ResourceDimension.Buffer;
			ResultBufferDesc.Alignment = 0;
			ResultBufferDesc.Width = sizeof(long) * 2;
			ResultBufferDesc.Height = 1;
			ResultBufferDesc.DepthOrArraySize = 1;
			ResultBufferDesc.MipLevels = 1;
			ResultBufferDesc.Format = Format.Unknown;
			ResultBufferDesc.SampleDescription.Count = 1;
			ResultBufferDesc.SampleDescription.Quality = 0;
			ResultBufferDesc.Layout = TextureLayout.RowMajor;
			ResultBufferDesc.Flags = ResourceFlags.None;
		}
		Timestamp_Result = D3D12Device.CreateCommittedResource(ResultBufferHeapProperties, HeapFlags.None, ResultBufferDesc, ResourceStates.CopyDestination, null);
	}

	public void Begin(ID3D12GraphicsCommandList6 CmdList)
	{
		if (!IsReadReady)
			return;

		CmdList.EndQuery(Timestamp_Heap, QueryType.Timestamp, 0);
	}

	public void End(ID3D12GraphicsCommandList6 CmdList)
	{
		if (!IsReadReady)
			return;

		CmdList.EndQuery(Timestamp_Heap, QueryType.Timestamp, 1);

		// GetData
		CmdList.ResolveQueryData(Timestamp_Heap, QueryType.Timestamp, 0, 2, Timestamp_Result, 0);

		// SyncResource
		ResourceTransitionBarrier CopyTransitionState = new ResourceTransitionBarrier(Timestamp_Result, ResourceStates.CopyDestination, ResourceStates.CopySource, 0);
		ResourceBarrier CopyBarrierInfo = new ResourceBarrier(CopyTransitionState);
		CmdList.ResourceBarrier(CopyBarrierInfo);

		Fence.Signal();
	}

	public float GetQueryResult(float TimestampFrequency)
	{
		IsReadReady = Fence.Completed();

		if (IsReadReady) 
		{ 
			// ReadData
			float[] Timestamp = new float[2];
			IntPtr Timeesult_Ptr = Timestamp_Result.Map(0);
			Timeesult_Ptr.CopyTo(Timestamp.AsSpan());
			TimeResult = (Timestamp[1] - Timestamp[0]) / TimestampFrequency;
			Timestamp_Result.Unmap(0);
		}
		return TimeResult;
	}

	protected override void DisposeManaged()
	{
		Timestamp_Heap.Dispose();
		Timestamp_Result.Dispose();
	}

	protected override void DisposeUnManaged()
	{

	}
}*/
}
