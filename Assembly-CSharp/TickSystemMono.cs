using System;
using UnityEngine;

// Token: 0x02000811 RID: 2065
internal abstract class TickSystemMono : MonoBehaviour, ITickSystem, ITickSystemPre, ITickSystemTick, ITickSystemPost
{
	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x060032C4 RID: 12996 RVA: 0x000F39BB File Offset: 0x000F1BBB
	// (set) Token: 0x060032C5 RID: 12997 RVA: 0x000F39C3 File Offset: 0x000F1BC3
	public bool PreTickRunning { get; set; }

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x060032C6 RID: 12998 RVA: 0x000F39CC File Offset: 0x000F1BCC
	// (set) Token: 0x060032C7 RID: 12999 RVA: 0x000F39D4 File Offset: 0x000F1BD4
	public bool TickRunning { get; set; }

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x060032C8 RID: 13000 RVA: 0x000F39DD File Offset: 0x000F1BDD
	// (set) Token: 0x060032C9 RID: 13001 RVA: 0x000F39E5 File Offset: 0x000F1BE5
	public bool PostTickRunning { get; set; }

	// Token: 0x060032CA RID: 13002 RVA: 0x000F39EE File Offset: 0x000F1BEE
	public virtual void OnEnable()
	{
		TickSystem<object>.AddTickSystemCallBack(this);
	}

	// Token: 0x060032CB RID: 13003 RVA: 0x000F39F6 File Offset: 0x000F1BF6
	public virtual void OnDisable()
	{
		TickSystem<object>.RemoveTickSystemCallback(this);
	}

	// Token: 0x060032CC RID: 13004 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void PreTick()
	{
	}

	// Token: 0x060032CD RID: 13005 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void Tick()
	{
	}

	// Token: 0x060032CE RID: 13006 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void PostTick()
	{
	}
}
