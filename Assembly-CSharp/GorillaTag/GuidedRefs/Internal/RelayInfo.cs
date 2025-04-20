using System;
using System.Collections.Generic;

namespace GorillaTag.GuidedRefs.Internal
{
	// Token: 0x02000C11 RID: 3089
	public class RelayInfo
	{
		// Token: 0x04004F4E RID: 20302
		[NonSerialized]
		public IGuidedRefTargetMono targetMono;

		// Token: 0x04004F4F RID: 20303
		[NonSerialized]
		public List<RegisteredReceiverFieldInfo> registeredFields;

		// Token: 0x04004F50 RID: 20304
		[NonSerialized]
		public List<RegisteredReceiverFieldInfo> resolvedFields;
	}
}
