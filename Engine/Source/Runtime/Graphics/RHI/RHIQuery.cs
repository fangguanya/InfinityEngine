using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
	public enum EQueryType
    {
		Occlusion = 0,
		Timestamp = 1,
		Statistics = 2,
		CopyTimestamp = 5
	}

    public class FRHIQuery : FDisposal
	{
		internal int indexHead;
		internal int indexLast;

		internal FRHIQuery(FRHIQueryContext queryContext) { }
		public virtual int GetResult() { return -1; }
		public virtual float GetResult(in ulong frequency) { return -1; }
	}

	internal class FRHIQueryContext : FDisposal
	{
		public int queryCount;
		public bool IsReady;
		public ulong[] queryData;
		public EQueryType queryType;
		public virtual int countActive => -1;
		public virtual int countInactive => -1;
		public virtual bool IsTimeQuery => false;

		public FRHIQueryContext(FRHIDevice device, in EQueryType queryType, in int queryCount) { }
		public virtual void Submit(FRHICommandContext commandContext) { }
		public virtual void GetData() { }
		internal virtual int AllocateUnuseIndex() { return -1; }
		internal virtual void ReleaseUnuseIndex(in int index) { }
		public virtual FRHIQuery GetTemporary(string name = null) { return null; }
		public virtual void ReleaseTemporary(FRHIQuery query) { }
	}
}
