using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BD5 RID: 3029
	[Serializable]
	public struct GuidedRefBasicTargetInfo
	{
		// Token: 0x04004E46 RID: 20038
		[SerializeField]
		public GuidedRefTargetIdSO targetId;

		// Token: 0x04004E47 RID: 20039
		[Tooltip("Used to filter down which relay the target can belong to. If null or empty then all parents with a GuidedRefRelayHub will be used.")]
		[SerializeField]
		public GuidedRefHubIdSO[] hubIds;

		// Token: 0x04004E48 RID: 20040
		[DebugOption]
		[SerializeField]
		public bool hackIgnoreDuplicateRegistration;
	}
}
