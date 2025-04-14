using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BE0 RID: 3040
	[Serializable]
	public struct GuidedRefReceiverArrayInfo
	{
		// Token: 0x06004C95 RID: 19605 RVA: 0x0017482A File Offset: 0x00172A2A
		public GuidedRefReceiverArrayInfo(bool useRecommendedDefaults)
		{
			this.resolveModes = (useRecommendedDefaults ? (GRef.EResolveModes.Runtime | GRef.EResolveModes.SceneProcessing) : GRef.EResolveModes.None);
			this.targets = Array.Empty<GuidedRefTargetIdSO>();
			this.hubId = null;
			this.fieldId = 0;
			this.resolveCount = 0;
		}

		// Token: 0x04004E4D RID: 20045
		[Tooltip("Controls whether the array should be overridden by the guided refs.")]
		[SerializeField]
		public GRef.EResolveModes resolveModes;

		// Token: 0x04004E4E RID: 20046
		[Tooltip("(Required) Used to filter down which relay the target can belong to. Only one GuidedRefRelayHub will be used.")]
		[SerializeField]
		public GuidedRefHubIdSO hubId;

		// Token: 0x04004E4F RID: 20047
		[SerializeField]
		public GuidedRefTargetIdSO[] targets;

		// Token: 0x04004E50 RID: 20048
		[NonSerialized]
		public int fieldId;

		// Token: 0x04004E51 RID: 20049
		[NonSerialized]
		public int resolveCount;
	}
}
