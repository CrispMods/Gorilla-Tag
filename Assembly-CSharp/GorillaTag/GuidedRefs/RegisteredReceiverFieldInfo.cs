using System;
using UnityEngine.Serialization;

namespace GorillaTag.GuidedRefs
{
	// Token: 0x02000BE1 RID: 3041
	public struct RegisteredReceiverFieldInfo
	{
		// Token: 0x04004E52 RID: 20050
		[FormerlySerializedAs("receiver")]
		public IGuidedRefReceiverMono receiverMono;

		// Token: 0x04004E53 RID: 20051
		public int fieldId;

		// Token: 0x04004E54 RID: 20052
		public int index;
	}
}
