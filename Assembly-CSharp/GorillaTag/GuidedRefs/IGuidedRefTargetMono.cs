using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BDF RID: 3039
	public interface IGuidedRefTargetMono : IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06004C92 RID: 19602
		// (set) Token: 0x06004C93 RID: 19603
		GuidedRefBasicTargetInfo GRefTargetInfo { get; set; }

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06004C94 RID: 19604
		Object GuidedRefTargetObject { get; }
	}
}
