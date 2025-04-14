using System;
using System.Collections.Generic;

namespace GorillaTag.GuidedRefs.Internal
{
	// Token: 0x02000BE6 RID: 3046
	public class RelayInfo
	{
		// Token: 0x04004E6A RID: 20074
		[NonSerialized]
		public IGuidedRefTargetMono targetMono;

		// Token: 0x04004E6B RID: 20075
		[NonSerialized]
		public List<RegisteredReceiverFieldInfo> registeredFields;

		// Token: 0x04004E6C RID: 20076
		[NonSerialized]
		public List<RegisteredReceiverFieldInfo> resolvedFields;
	}
}
