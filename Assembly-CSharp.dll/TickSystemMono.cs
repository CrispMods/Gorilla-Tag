using System;
using UnityEngine;

// Token: 0x02000814 RID: 2068
internal abstract class TickSystemMono : MonoBehaviour, ITickSystem, ITickSystemPre, ITickSystemTick, ITickSystemPost
{
	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x060032D0 RID: 13008 RVA: 0x00050AFB File Offset: 0x0004ECFB
	// (set) Token: 0x060032D1 RID: 13009 RVA: 0x00050B03 File Offset: 0x0004ED03
	public bool PreTickRunning { get; set; }

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x060032D2 RID: 13010 RVA: 0x00050B0C File Offset: 0x0004ED0C
	// (set) Token: 0x060032D3 RID: 13011 RVA: 0x00050B14 File Offset: 0x0004ED14
	public bool TickRunning { get; set; }

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x060032D4 RID: 13012 RVA: 0x00050B1D File Offset: 0x0004ED1D
	// (set) Token: 0x060032D5 RID: 13013 RVA: 0x00050B25 File Offset: 0x0004ED25
	public bool PostTickRunning { get; set; }

	// Token: 0x060032D6 RID: 13014 RVA: 0x00050B2E File Offset: 0x0004ED2E
	public virtual void OnEnable()
	{
		TickSystem<object>.AddTickSystemCallBack(this);
	}

	// Token: 0x060032D7 RID: 13015 RVA: 0x00050B36 File Offset: 0x0004ED36
	public virtual void OnDisable()
	{
		TickSystem<object>.RemoveTickSystemCallback(this);
	}

	// Token: 0x060032D8 RID: 13016 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void PreTick()
	{
	}

	// Token: 0x060032D9 RID: 13017 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void Tick()
	{
	}

	// Token: 0x060032DA RID: 13018 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void PostTick()
	{
	}
}
