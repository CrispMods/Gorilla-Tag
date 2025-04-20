using System;
using UnityEngine.Serialization;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000C0F RID: 3087
	public struct RegisteredReceiverFieldInfo
	{
		// Token: 0x04004F48 RID: 20296
		[FormerlySerializedAs("receiver")]
		public IGuidedRefReceiverMono receiverMono;

		// Token: 0x04004F49 RID: 20297
		public int fieldId;

		// Token: 0x04004F4A RID: 20298
		public int index;
	}
}
