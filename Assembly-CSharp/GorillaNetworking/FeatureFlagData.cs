using System;
using System.Collections.Generic;

namespace GorillaNetworking
{
	// Token: 0x02000AF9 RID: 2809
	[Serializable]
	internal class FeatureFlagData
	{
		// Token: 0x04004799 RID: 18329
		public string name;

		// Token: 0x0400479A RID: 18330
		public int value;

		// Token: 0x0400479B RID: 18331
		public string valueType;

		// Token: 0x0400479C RID: 18332
		public List<string> alwaysOnForUsers;
	}
}
