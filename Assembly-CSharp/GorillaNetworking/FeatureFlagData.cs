using System;
using System.Collections.Generic;

namespace GorillaNetworking
{
	// Token: 0x02000ACC RID: 2764
	[Serializable]
	internal class FeatureFlagData
	{
		// Token: 0x040046A2 RID: 18082
		public string name;

		// Token: 0x040046A3 RID: 18083
		public int value;

		// Token: 0x040046A4 RID: 18084
		public string valueType;

		// Token: 0x040046A5 RID: 18085
		public List<string> alwaysOnForUsers;
	}
}
