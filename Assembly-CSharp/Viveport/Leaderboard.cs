using System;

namespace Viveport
{
	// Token: 0x0200091E RID: 2334
	public class Leaderboard
	{
		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x0600382C RID: 14380 RVA: 0x000551B5 File Offset: 0x000533B5
		// (set) Token: 0x0600382D RID: 14381 RVA: 0x000551BD File Offset: 0x000533BD
		public int Rank { get; set; }

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x0600382E RID: 14382 RVA: 0x000551C6 File Offset: 0x000533C6
		// (set) Token: 0x0600382F RID: 14383 RVA: 0x000551CE File Offset: 0x000533CE
		public int Score { get; set; }

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06003830 RID: 14384 RVA: 0x000551D7 File Offset: 0x000533D7
		// (set) Token: 0x06003831 RID: 14385 RVA: 0x000551DF File Offset: 0x000533DF
		public string UserName { get; set; }
	}
}
