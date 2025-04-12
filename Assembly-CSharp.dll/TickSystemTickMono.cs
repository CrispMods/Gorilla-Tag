using System;
using UnityEngine;

// Token: 0x02000816 RID: 2070
internal abstract class TickSystemTickMono : MonoBehaviour, ITickSystemTick
{
	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x060032E2 RID: 13026 RVA: 0x00050B5F File Offset: 0x0004ED5F
	// (set) Token: 0x060032E3 RID: 13027 RVA: 0x00050B67 File Offset: 0x0004ED67
	public bool TickRunning { get; set; }

	// Token: 0x060032E4 RID: 13028 RVA: 0x00034963 File Offset: 0x00032B63
	public virtual void OnEnable()
	{
		TickSystem<object>.AddTickCallback(this);
	}

	// Token: 0x060032E5 RID: 13029 RVA: 0x0003496B File Offset: 0x00032B6B
	public virtual void OnDisable()
	{
		TickSystem<object>.RemoveTickCallback(this);
	}

	// Token: 0x060032E6 RID: 13030 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void Tick()
	{
	}
}
