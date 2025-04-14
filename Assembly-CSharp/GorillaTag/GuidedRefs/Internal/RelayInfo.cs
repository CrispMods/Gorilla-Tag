using System;
using System.Collections.Generic;

namespace GorillaTag.GuidedRefs.Internal
{
	// Token: 0x02000BE3 RID: 3043
	public class RelayInfo
	{
		// Token: 0x04004E58 RID: 20056
		[NonSerialized]
		public IGuidedRefTargetMono targetMono;

		// Token: 0x04004E59 RID: 20057
		[NonSerialized]
		public List<RegisteredReceiverFieldInfo> registeredFields;

		// Token: 0x04004E5A RID: 20058
		[NonSerialized]
		public List<RegisteredReceiverFieldInfo> resolvedFields;
	}
}
