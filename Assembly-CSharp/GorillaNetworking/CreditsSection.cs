using System;
using System.Collections.Generic;

namespace GorillaNetworking
{
	// Token: 0x02000ACE RID: 2766
	[Serializable]
	internal class CreditsSection
	{
		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06004542 RID: 17730 RVA: 0x0005D25F File Offset: 0x0005B45F
		// (set) Token: 0x06004543 RID: 17731 RVA: 0x0005D267 File Offset: 0x0005B467
		public string Title { get; set; }

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06004544 RID: 17732 RVA: 0x0005D270 File Offset: 0x0005B470
		// (set) Token: 0x06004545 RID: 17733 RVA: 0x0005D278 File Offset: 0x0005B478
		public List<string> Entries { get; set; }
	}
}
