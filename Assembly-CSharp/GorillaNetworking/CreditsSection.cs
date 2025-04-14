using System;
using System.Collections.Generic;

namespace GorillaNetworking
{
	// Token: 0x02000AA2 RID: 2722
	[Serializable]
	internal class CreditsSection
	{
		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060043FF RID: 17407 RVA: 0x0014232B File Offset: 0x0014052B
		// (set) Token: 0x06004400 RID: 17408 RVA: 0x00142333 File Offset: 0x00140533
		public string Title { get; set; }

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06004401 RID: 17409 RVA: 0x0014233C File Offset: 0x0014053C
		// (set) Token: 0x06004402 RID: 17410 RVA: 0x00142344 File Offset: 0x00140544
		public List<string> Entries { get; set; }
	}
}
