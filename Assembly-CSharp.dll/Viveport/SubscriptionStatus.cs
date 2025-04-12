using System;
using System.Collections.Generic;

namespace Viveport
{
	// Token: 0x02000905 RID: 2309
	public class SubscriptionStatus
	{
		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x0600376E RID: 14190 RVA: 0x00053C46 File Offset: 0x00051E46
		// (set) Token: 0x0600376F RID: 14191 RVA: 0x00053C4E File Offset: 0x00051E4E
		public List<SubscriptionStatus.Platform> Platforms { get; set; }

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06003770 RID: 14192 RVA: 0x00053C57 File Offset: 0x00051E57
		// (set) Token: 0x06003771 RID: 14193 RVA: 0x00053C5F File Offset: 0x00051E5F
		public SubscriptionStatus.TransactionType Type { get; set; }

		// Token: 0x06003772 RID: 14194 RVA: 0x00053C68 File Offset: 0x00051E68
		public SubscriptionStatus()
		{
			this.Platforms = new List<SubscriptionStatus.Platform>();
			this.Type = SubscriptionStatus.TransactionType.Unknown;
		}

		// Token: 0x02000906 RID: 2310
		public enum Platform
		{
			// Token: 0x04003A5D RID: 14941
			Windows,
			// Token: 0x04003A5E RID: 14942
			Android
		}

		// Token: 0x02000907 RID: 2311
		public enum TransactionType
		{
			// Token: 0x04003A60 RID: 14944
			Unknown,
			// Token: 0x04003A61 RID: 14945
			Paid,
			// Token: 0x04003A62 RID: 14946
			Redeem,
			// Token: 0x04003A63 RID: 14947
			FreeTrial
		}
	}
}
