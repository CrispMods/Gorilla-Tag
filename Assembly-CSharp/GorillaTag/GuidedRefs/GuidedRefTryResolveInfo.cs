using System;
using UnityEngine.Serialization;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BE2 RID: 3042
	public struct GuidedRefTryResolveInfo
	{
		// Token: 0x04004E55 RID: 20053
		public int fieldId;

		// Token: 0x04004E56 RID: 20054
		public int index;

		// Token: 0x04004E57 RID: 20055
		[FormerlySerializedAs("target")]
		public IGuidedRefTargetMono targetMono;
	}
}
