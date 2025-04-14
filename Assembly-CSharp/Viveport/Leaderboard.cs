using System;

namespace Viveport
{
	// Token: 0x02000901 RID: 2305
	public class Leaderboard
	{
		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x0600375B RID: 14171 RVA: 0x0010537B File Offset: 0x0010357B
		// (set) Token: 0x0600375C RID: 14172 RVA: 0x00105383 File Offset: 0x00103583
		public int Rank { get; set; }

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x0600375D RID: 14173 RVA: 0x0010538C File Offset: 0x0010358C
		// (set) Token: 0x0600375E RID: 14174 RVA: 0x00105394 File Offset: 0x00103594
		public int Score { get; set; }

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x0600375F RID: 14175 RVA: 0x0010539D File Offset: 0x0010359D
		// (set) Token: 0x06003760 RID: 14176 RVA: 0x001053A5 File Offset: 0x001035A5
		public string UserName { get; set; }
	}
}
