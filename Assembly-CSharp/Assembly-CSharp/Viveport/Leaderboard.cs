﻿using System;

namespace Viveport
{
	// Token: 0x02000904 RID: 2308
	public class Leaderboard
	{
		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06003767 RID: 14183 RVA: 0x00105943 File Offset: 0x00103B43
		// (set) Token: 0x06003768 RID: 14184 RVA: 0x0010594B File Offset: 0x00103B4B
		public int Rank { get; set; }

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06003769 RID: 14185 RVA: 0x00105954 File Offset: 0x00103B54
		// (set) Token: 0x0600376A RID: 14186 RVA: 0x0010595C File Offset: 0x00103B5C
		public int Score { get; set; }

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x0600376B RID: 14187 RVA: 0x00105965 File Offset: 0x00103B65
		// (set) Token: 0x0600376C RID: 14188 RVA: 0x0010596D File Offset: 0x00103B6D
		public string UserName { get; set; }
	}
}
