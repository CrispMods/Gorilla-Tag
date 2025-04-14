using System;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BDE RID: 3038
	public interface IGuidedRefReceiverMono : IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x06004C8D RID: 19597
		bool GuidedRefTryResolveReference(GuidedRefTryResolveInfo target);

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06004C8E RID: 19598
		// (set) Token: 0x06004C8F RID: 19599
		int GuidedRefsWaitingToResolveCount { get; set; }

		// Token: 0x06004C90 RID: 19600
		void OnAllGuidedRefsResolved();

		// Token: 0x06004C91 RID: 19601
		void OnGuidedRefTargetDestroyed(int fieldId);
	}
}
