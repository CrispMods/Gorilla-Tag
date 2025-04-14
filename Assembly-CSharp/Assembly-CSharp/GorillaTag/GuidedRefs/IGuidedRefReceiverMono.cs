using System;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BE1 RID: 3041
	public interface IGuidedRefReceiverMono : IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x06004C99 RID: 19609
		bool GuidedRefTryResolveReference(GuidedRefTryResolveInfo target);

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06004C9A RID: 19610
		// (set) Token: 0x06004C9B RID: 19611
		int GuidedRefsWaitingToResolveCount { get; set; }

		// Token: 0x06004C9C RID: 19612
		void OnAllGuidedRefsResolved();

		// Token: 0x06004C9D RID: 19613
		void OnGuidedRefTargetDestroyed(int fieldId);
	}
}
