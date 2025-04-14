using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BE2 RID: 3042
	public interface IGuidedRefTargetMono : IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06004C9E RID: 19614
		// (set) Token: 0x06004C9F RID: 19615
		GuidedRefBasicTargetInfo GRefTargetInfo { get; set; }

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06004CA0 RID: 19616
		Object GuidedRefTargetObject { get; }
	}
}
