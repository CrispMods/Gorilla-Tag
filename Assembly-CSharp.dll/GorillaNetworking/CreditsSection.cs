using System;
using System.Collections.Generic;

namespace GorillaNetworking
{
	// Token: 0x02000AA5 RID: 2725
	[Serializable]
	internal class CreditsSection
	{
		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x0600440B RID: 17419 RVA: 0x0005B888 File Offset: 0x00059A88
		// (set) Token: 0x0600440C RID: 17420 RVA: 0x0005B890 File Offset: 0x00059A90
		public string Title { get; set; }

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x0600440D RID: 17421 RVA: 0x0005B899 File Offset: 0x00059A99
		// (set) Token: 0x0600440E RID: 17422 RVA: 0x0005B8A1 File Offset: 0x00059AA1
		public List<string> Entries { get; set; }
	}
}
