using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BE3 RID: 3043
	[Serializable]
	public struct GuidedRefReceiverArrayInfo
	{
		// Token: 0x06004CA1 RID: 19617 RVA: 0x0006157C File Offset: 0x0005F77C
		public GuidedRefReceiverArrayInfo(bool useRecommendedDefaults)
		{
			this.resolveModes = (useRecommendedDefaults ? (GRef.EResolveModes.Runtime | GRef.EResolveModes.SceneProcessing) : GRef.EResolveModes.None);
			this.targets = Array.Empty<GuidedRefTargetIdSO>();
			this.hubId = null;
			this.fieldId = 0;
			this.resolveCount = 0;
		}

		// Token: 0x04004E5F RID: 20063
		[Tooltip("Controls whether the array should be overridden by the guided refs.")]
		[SerializeField]
		public GRef.EResolveModes resolveModes;

		// Token: 0x04004E60 RID: 20064
		[Tooltip("(Required) Used to filter down which relay the target can belong to. Only one GuidedRefRelayHub will be used.")]
		[SerializeField]
		public GuidedRefHubIdSO hubId;

		// Token: 0x04004E61 RID: 20065
		[SerializeField]
		public GuidedRefTargetIdSO[] targets;

		// Token: 0x04004E62 RID: 20066
		[NonSerialized]
		public int fieldId;

		// Token: 0x04004E63 RID: 20067
		[NonSerialized]
		public int resolveCount;
	}
}
