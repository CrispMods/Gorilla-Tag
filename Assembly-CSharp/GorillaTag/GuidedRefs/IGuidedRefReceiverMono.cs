using System;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000C0C RID: 3084
	public interface IGuidedRefReceiverMono : IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x06004DD9 RID: 19929
		bool GuidedRefTryResolveReference(GuidedRefTryResolveInfo target);

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06004DDA RID: 19930
		// (set) Token: 0x06004DDB RID: 19931
		int GuidedRefsWaitingToResolveCount { get; set; }

		// Token: 0x06004DDC RID: 19932
		void OnAllGuidedRefsResolved();

		// Token: 0x06004DDD RID: 19933
		void OnGuidedRefTargetDestroyed(int fieldId);
	}
}
