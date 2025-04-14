using System;
using UnityEngine;

// Token: 0x02000815 RID: 2069
internal abstract class TickSystemPreTickMono : MonoBehaviour, ITickSystemPre
{
	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x060032DC RID: 13020 RVA: 0x000F3FC6 File Offset: 0x000F21C6
	// (set) Token: 0x060032DD RID: 13021 RVA: 0x000F3FCE File Offset: 0x000F21CE
	public bool PreTickRunning { get; set; }

	// Token: 0x060032DE RID: 13022 RVA: 0x000F3FD7 File Offset: 0x000F21D7
	public virtual void OnEnable()
	{
		TickSystem<object>.AddPreTickCallback(this);
	}

	// Token: 0x060032DF RID: 13023 RVA: 0x000F3FDF File Offset: 0x000F21DF
	public void OnDisable()
	{
		TickSystem<object>.RemovePreTickCallback(this);
	}

	// Token: 0x060032E0 RID: 13024 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void PreTick()
	{
	}
}
