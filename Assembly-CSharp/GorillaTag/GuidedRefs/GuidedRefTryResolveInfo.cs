using System;
using UnityEngine.Serialization;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000C10 RID: 3088
	public struct GuidedRefTryResolveInfo
	{
		// Token: 0x04004F4B RID: 20299
		public int fieldId;

		// Token: 0x04004F4C RID: 20300
		public int index;

		// Token: 0x04004F4D RID: 20301
		[FormerlySerializedAs("target")]
		public IGuidedRefTargetMono targetMono;
	}
}
