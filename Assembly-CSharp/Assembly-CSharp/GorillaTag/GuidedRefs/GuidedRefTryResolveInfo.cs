using System;
using UnityEngine.Serialization;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BE5 RID: 3045
	public struct GuidedRefTryResolveInfo
	{
		// Token: 0x04004E67 RID: 20071
		public int fieldId;

		// Token: 0x04004E68 RID: 20072
		public int index;

		// Token: 0x04004E69 RID: 20073
		[FormerlySerializedAs("target")]
		public IGuidedRefTargetMono targetMono;
	}
}
