using System;
using UnityEngine;

// Token: 0x02000812 RID: 2066
internal abstract class TickSystemPreTickMono : MonoBehaviour, ITickSystemPre
{
	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x060032D0 RID: 13008 RVA: 0x000F39FE File Offset: 0x000F1BFE
	// (set) Token: 0x060032D1 RID: 13009 RVA: 0x000F3A06 File Offset: 0x000F1C06
	public bool PreTickRunning { get; set; }

	// Token: 0x060032D2 RID: 13010 RVA: 0x000F3A0F File Offset: 0x000F1C0F
	public virtual void OnEnable()
	{
		TickSystem<object>.AddPreTickCallback(this);
	}

	// Token: 0x060032D3 RID: 13011 RVA: 0x000F3A17 File Offset: 0x000F1C17
	public void OnDisable()
	{
		TickSystem<object>.RemovePreTickCallback(this);
	}

	// Token: 0x060032D4 RID: 13012 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void PreTick()
	{
	}
}
