using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BDC RID: 3036
	[Serializable]
	public struct GuidedRefReceiverFieldInfo
	{
		// Token: 0x06004C7C RID: 19580 RVA: 0x00174D3C File Offset: 0x00172F3C
		public GuidedRefReceiverFieldInfo(bool useRecommendedDefaults)
		{
			this.resolveModes = (useRecommendedDefaults ? (GRef.EResolveModes.Runtime | GRef.EResolveModes.SceneProcessing) : GRef.EResolveModes.None);
			this.targetId = null;
			this.hubId = null;
			this.fieldId = 0;
		}

		// Token: 0x04004E57 RID: 20055
		[SerializeField]
		public GRef.EResolveModes resolveModes;

		// Token: 0x04004E58 RID: 20056
		[SerializeField]
		public GuidedRefTargetIdSO targetId;

		// Token: 0x04004E59 RID: 20057
		[Tooltip("(Required) Used to filter down which relay the target can belong to. Only one GuidedRefRelayHub will be used.")]
		[SerializeField]
		public GuidedRefHubIdSO hubId;

		// Token: 0x04004E5A RID: 20058
		[NonSerialized]
		public int fieldId;
	}
}
