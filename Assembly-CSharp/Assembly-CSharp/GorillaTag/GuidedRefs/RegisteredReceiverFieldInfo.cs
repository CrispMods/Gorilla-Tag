using System;
using UnityEngine.Serialization;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BE4 RID: 3044
	public struct RegisteredReceiverFieldInfo
	{
		// Token: 0x04004E64 RID: 20068
		[FormerlySerializedAs("receiver")]
		public IGuidedRefReceiverMono receiverMono;

		// Token: 0x04004E65 RID: 20069
		public int fieldId;

		// Token: 0x04004E66 RID: 20070
		public int index;
	}
}
