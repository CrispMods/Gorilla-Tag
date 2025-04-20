using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000C00 RID: 3072
	[Serializable]
	public struct GuidedRefBasicTargetInfo
	{
		// Token: 0x04004F2A RID: 20266
		[SerializeField]
		public GuidedRefTargetIdSO targetId;

		// Token: 0x04004F2B RID: 20267
		[Tooltip("Used to filter down which relay the target can belong to. If null or empty then all parents with a GuidedRefRelayHub will be used.")]
		[SerializeField]
		public GuidedRefHubIdSO[] hubIds;

		// Token: 0x04004F2C RID: 20268
		[DebugOption]
		[SerializeField]
		public bool hackIgnoreDuplicateRegistration;
	}
}
