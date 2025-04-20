using System;
using System.Collections.Generic;

namespace Viveport
{
	// Token: 0x0200091F RID: 2335
	public class SubscriptionStatus
	{
		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06003833 RID: 14387 RVA: 0x000551E8 File Offset: 0x000533E8
		// (set) Token: 0x06003834 RID: 14388 RVA: 0x000551F0 File Offset: 0x000533F0
		public List<SubscriptionStatus.Platform> Platforms { get; set; }

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06003835 RID: 14389 RVA: 0x000551F9 File Offset: 0x000533F9
		// (set) Token: 0x06003836 RID: 14390 RVA: 0x00055201 File Offset: 0x00053401
		public SubscriptionStatus.TransactionType Type { get; set; }

		// Token: 0x06003837 RID: 14391 RVA: 0x0005520A File Offset: 0x0005340A
		public SubscriptionStatus()
		{
			this.Platforms = new List<SubscriptionStatus.Platform>();
			this.Type = SubscriptionStatus.TransactionType.Unknown;
		}

		// Token: 0x02000920 RID: 2336
		public enum Platform
		{
			// Token: 0x04003B10 RID: 15120
			Windows,
			// Token: 0x04003B11 RID: 15121
			Android
		}

		// Token: 0x02000921 RID: 2337
		public enum TransactionType
		{
			// Token: 0x04003B13 RID: 15123
			Unknown,
			// Token: 0x04003B14 RID: 15124
			Paid,
			// Token: 0x04003B15 RID: 15125
			Redeem,
			// Token: 0x04003B16 RID: 15126
			FreeTrial
		}
	}
}
