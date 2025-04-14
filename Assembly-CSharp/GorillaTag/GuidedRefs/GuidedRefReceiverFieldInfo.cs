using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BD9 RID: 3033
	[Serializable]
	public struct GuidedRefReceiverFieldInfo
	{
		// Token: 0x06004C70 RID: 19568 RVA: 0x00174774 File Offset: 0x00172974
		public GuidedRefReceiverFieldInfo(bool useRecommendedDefaults)
		{
			this.resolveModes = (useRecommendedDefaults ? (GRef.EResolveModes.Runtime | GRef.EResolveModes.SceneProcessing) : GRef.EResolveModes.None);
			this.targetId = null;
			this.hubId = null;
			this.fieldId = 0;
		}

		// Token: 0x04004E45 RID: 20037
		[SerializeField]
		public GRef.EResolveModes resolveModes;

		// Token: 0x04004E46 RID: 20038
		[SerializeField]
		public GuidedRefTargetIdSO targetId;

		// Token: 0x04004E47 RID: 20039
		[Tooltip("(Required) Used to filter down which relay the target can belong to. Only one GuidedRefRelayHub will be used.")]
		[SerializeField]
		public GuidedRefHubIdSO hubId;

		// Token: 0x04004E48 RID: 20040
		[NonSerialized]
		public int fieldId;
	}
}
