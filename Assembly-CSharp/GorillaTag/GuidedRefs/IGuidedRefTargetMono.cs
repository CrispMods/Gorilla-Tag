using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000C0D RID: 3085
	public interface IGuidedRefTargetMono : IGuidedRefMonoBehaviour, IGuidedRefObject
	{
		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06004DDE RID: 19934
		// (set) Token: 0x06004DDF RID: 19935
		GuidedRefBasicTargetInfo GRefTargetInfo { get; set; }

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06004DE0 RID: 19936
		UnityEngine.Object GuidedRefTargetObject { get; }
	}
}
