using System;
using System.Collections.Generic;

namespace Viveport
{
	// Token: 0x02000902 RID: 2306
	public class SubscriptionStatus
	{
		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06003762 RID: 14178 RVA: 0x001053AE File Offset: 0x001035AE
		// (set) Token: 0x06003763 RID: 14179 RVA: 0x001053B6 File Offset: 0x001035B6
		public List<SubscriptionStatus.Platform> Platforms { get; set; }

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06003764 RID: 14180 RVA: 0x001053BF File Offset: 0x001035BF
		// (set) Token: 0x06003765 RID: 14181 RVA: 0x001053C7 File Offset: 0x001035C7
		public SubscriptionStatus.TransactionType Type { get; set; }

		// Token: 0x06003766 RID: 14182 RVA: 0x001053D0 File Offset: 0x001035D0
		public SubscriptionStatus()
		{
			this.Platforms = new List<SubscriptionStatus.Platform>();
			this.Type = SubscriptionStatus.TransactionType.Unknown;
		}

		// Token: 0x02000903 RID: 2307
		public enum Platform
		{
			// Token: 0x04003A4B RID: 14923
			Windows,
			// Token: 0x04003A4C RID: 14924
			Android
		}

		// Token: 0x02000904 RID: 2308
		public enum TransactionType
		{
			// Token: 0x04003A4E RID: 14926
			Unknown,
			// Token: 0x04003A4F RID: 14927
			Paid,
			// Token: 0x04003A50 RID: 14928
			Redeem,
			// Token: 0x04003A51 RID: 14929
			FreeTrial
		}
	}
}
