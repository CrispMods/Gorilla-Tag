using System;
using System.Collections.Generic;

namespace GorillaNetworking
{
	// Token: 0x02000AA5 RID: 2725
	[Serializable]
	internal class CreditsSection
	{
		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x0600440B RID: 17419 RVA: 0x001428F3 File Offset: 0x00140AF3
		// (set) Token: 0x0600440C RID: 17420 RVA: 0x001428FB File Offset: 0x00140AFB
		public string Title { get; set; }

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x0600440D RID: 17421 RVA: 0x00142904 File Offset: 0x00140B04
		// (set) Token: 0x0600440E RID: 17422 RVA: 0x0014290C File Offset: 0x00140B0C
		public List<string> Entries { get; set; }
	}
}
