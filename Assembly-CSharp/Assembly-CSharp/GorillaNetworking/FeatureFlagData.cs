using System;
using System.Collections.Generic;

namespace GorillaNetworking
{
	// Token: 0x02000ACF RID: 2767
	[Serializable]
	internal class FeatureFlagData
	{
		// Token: 0x040046B4 RID: 18100
		public string name;

		// Token: 0x040046B5 RID: 18101
		public int value;

		// Token: 0x040046B6 RID: 18102
		public string valueType;

		// Token: 0x040046B7 RID: 18103
		public List<string> alwaysOnForUsers;
	}
}
