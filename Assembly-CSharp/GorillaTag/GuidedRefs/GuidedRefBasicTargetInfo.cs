using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BD2 RID: 3026
	[Serializable]
	public struct GuidedRefBasicTargetInfo
	{
		// Token: 0x04004E34 RID: 20020
		[SerializeField]
		public GuidedRefTargetIdSO targetId;

		// Token: 0x04004E35 RID: 20021
		[Tooltip("Used to filter down which relay the target can belong to. If null or empty then all parents with a GuidedRefRelayHub will be used.")]
		[SerializeField]
		public GuidedRefHubIdSO[] hubIds;

		// Token: 0x04004E36 RID: 20022
		[DebugOption]
		[SerializeField]
		public bool hackIgnoreDuplicateRegistration;
	}
}
