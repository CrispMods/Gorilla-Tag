using System;
using UnityEngine;

// Token: 0x02000815 RID: 2069
internal abstract class TickSystemPreTickMono : MonoBehaviour, ITickSystemPre
{
	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x060032DC RID: 13020 RVA: 0x00050B3E File Offset: 0x0004ED3E
	// (set) Token: 0x060032DD RID: 13021 RVA: 0x00050B46 File Offset: 0x0004ED46
	public bool PreTickRunning { get; set; }

	// Token: 0x060032DE RID: 13022 RVA: 0x00050B4F File Offset: 0x0004ED4F
	public virtual void OnEnable()
	{
		TickSystem<object>.AddPreTickCallback(this);
	}

	// Token: 0x060032DF RID: 13023 RVA: 0x00050B57 File Offset: 0x0004ED57
	public void OnDisable()
	{
		TickSystem<object>.RemovePreTickCallback(this);
	}

	// Token: 0x060032E0 RID: 13024 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void PreTick()
	{
	}
}
