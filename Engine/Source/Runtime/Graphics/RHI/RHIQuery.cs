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

    public abstract class FRHIQuery : FDisposal
	{
		internal int indexHead;
		internal int indexLast;

		internal FRHIQuery(FRHIQueryContext queryContext) { }
		public abstract int GetResult();
		public abstract float GetResult(in ulong frequency);
	}

	internal abstract class FRHIQueryContext : FDisposal
	{
		public int queryCount;
		public bool IsReady;
		public ulong[] queryData;
		public EQueryType queryType;
		public virtual int countActive => -1;
		public virtual int countInactive => -1;
		public virtual bool IsTimeQuery => false;

		public FRHIQueryContext(FRHIDevice device, in EQueryType queryType, in int queryCount) { }
		public abstract void Submit(FRHICommandContext commandContext);
		public abstract void GetData();
		internal abstract int AllocateUnuseIndex();
		internal abstract void ReleaseUnuseIndex(in int index);
		public abstract FRHIQuery GetTemporary(string name = null);
		public abstract void ReleaseTemporary(FRHIQuery query);
	}
}
