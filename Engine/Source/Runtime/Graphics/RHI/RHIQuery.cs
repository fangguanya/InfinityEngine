using InfinityEngine.Core.Object;

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

    public class FRHIQuery : FDisposable
	{
		internal int indexHead;
		internal int indexLast;

		internal FRHIQuery(FRHIQueryContext context) { }

		public virtual int GetResult() { return -1; }
		public virtual float GetResult(in ulong frequency) { return -1; }
	}

	internal class FRHIQueryContext : FDisposable
	{
		internal int queryCount;
		internal bool IsReadReady;
		internal ulong[] queryData;
		internal EQueryType queryType;

		public virtual int countActive => -1;
		public virtual int countInactive => -1;
		public virtual bool IsTimeQuery => false;

		public FRHIQueryContext(FRHIDevice device, in EQueryType queryType, in int queryCount) { }

		public virtual void Submit(FRHICommandContext commandContext) { }
		public virtual void GetData() { }
		public virtual int AllocateQueryID() { return -1; }
		public virtual void ReleaseQueryID(in int index) { }
		public virtual FRHIQuery GetTemporary(string name = null) { return null; }
		public virtual void ReleaseTemporary(FRHIQuery query) { }
	}
}
