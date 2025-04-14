using System;
using UnityEngine;

// Token: 0x02000814 RID: 2068
internal abstract class TickSystemMono : MonoBehaviour, ITickSystem, ITickSystemPre, ITickSystemTick, ITickSystemPost
{
	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x060032D0 RID: 13008 RVA: 0x000F3F83 File Offset: 0x000F2183
	// (set) Token: 0x060032D1 RID: 13009 RVA: 0x000F3F8B File Offset: 0x000F218B
	public bool PreTickRunning { get; set; }

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x060032D2 RID: 13010 RVA: 0x000F3F94 File Offset: 0x000F2194
	// (set) Token: 0x060032D3 RID: 13011 RVA: 0x000F3F9C File Offset: 0x000F219C
	public bool TickRunning { get; set; }

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x060032D4 RID: 13012 RVA: 0x000F3FA5 File Offset: 0x000F21A5
	// (set) Token: 0x060032D5 RID: 13013 RVA: 0x000F3FAD File Offset: 0x000F21AD
	public bool PostTickRunning { get; set; }

	// Token: 0x060032D6 RID: 13014 RVA: 0x000F3FB6 File Offset: 0x000F21B6
	public virtual void OnEnable()
	{
		TickSystem<object>.AddTickSystemCallBack(this);
	}

	// Token: 0x060032D7 RID: 13015 RVA: 0x000F3FBE File Offset: 0x000F21BE
	public virtual void OnDisable()
	{
		TickSystem<object>.RemoveTickSystemCallback(this);
	}

	// Token: 0x060032D8 RID: 13016 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void PreTick()
	{
	}

	// Token: 0x060032D9 RID: 13017 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void Tick()
	{
	}

	// Token: 0x060032DA RID: 13018 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void PostTick()
	{
	}
}
