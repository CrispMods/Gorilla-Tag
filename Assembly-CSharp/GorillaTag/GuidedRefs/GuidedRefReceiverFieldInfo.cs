using System;
using UnityEngine;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000C07 RID: 3079
	[Serializable]
	public struct GuidedRefReceiverFieldInfo
	{
		// Token: 0x06004DBC RID: 19900 RVA: 0x00062E87 File Offset: 0x00061087
		public GuidedRefReceiverFieldInfo(bool useRecommendedDefaults)
		{
			this.resolveModes = (useRecommendedDefaults ? (GRef.EResolveModes.Runtime | GRef.EResolveModes.SceneProcessing) : GRef.EResolveModes.None);
			this.targetId = null;
			this.hubId = null;
			this.fieldId = 0;
		}

		// Token: 0x04004F3B RID: 20283
		[SerializeField]
		public GRef.EResolveModes resolveModes;

		// Token: 0x04004F3C RID: 20284
		[SerializeField]
		public GuidedRefTargetIdSO targetId;

		// Token: 0x04004F3D RID: 20285
		[Tooltip("(Required) Used to filter down which relay the target can belong to. Only one GuidedRefRelayHub will be used.")]
		[SerializeField]
		public GuidedRefHubIdSO hubId;

		// Token: 0x04004F3E RID: 20286
		[NonSerialized]
		public int fieldId;
	}
}
