using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000C0E RID: 3086
	[Serializable]
	public struct GuidedRefReceiverArrayInfo
	{
		// Token: 0x06004DE1 RID: 19937 RVA: 0x00062F3D File Offset: 0x0006113D
		public GuidedRefReceiverArrayInfo(bool useRecommendedDefaults)
		{
			this.resolveModes = (useRecommendedDefaults ? (GRef.EResolveModes.Runtime | GRef.EResolveModes.SceneProcessing) : GRef.EResolveModes.None);
			this.targets = Array.Empty<GuidedRefTargetIdSO>();
			this.hubId = null;
			this.fieldId = 0;
			this.resolveCount = 0;
		}

		// Token: 0x04004F43 RID: 20291
		[Tooltip("Controls whether the array should be overridden by the guided refs.")]
		[SerializeField]
		public GRef.EResolveModes resolveModes;

		// Token: 0x04004F44 RID: 20292
		[Tooltip("(Required) Used to filter down which relay the target can belong to. Only one GuidedRefRelayHub will be used.")]
		[SerializeField]
		public GuidedRefHubIdSO hubId;

		// Token: 0x04004F45 RID: 20293
		[SerializeField]
		public GuidedRefTargetIdSO[] targets;

		// Token: 0x04004F46 RID: 20294
		[NonSerialized]
		public int fieldId;

		// Token: 0x04004F47 RID: 20295
		[NonSerialized]
		public int resolveCount;
	}
}
