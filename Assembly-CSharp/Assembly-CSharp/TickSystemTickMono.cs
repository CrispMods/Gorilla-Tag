using System;
using UnityEngine;

// Token: 0x02000816 RID: 2070
internal abstract class TickSystemTickMono : MonoBehaviour, ITickSystemTick
{
	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x060032E2 RID: 13026 RVA: 0x000F3FE7 File Offset: 0x000F21E7
	// (set) Token: 0x060032E3 RID: 13027 RVA: 0x000F3FEF File Offset: 0x000F21EF
	public bool TickRunning { get; set; }

	// Token: 0x060032E4 RID: 13028 RVA: 0x0002B7B9 File Offset: 0x000299B9
	public virtual void OnEnable()
	{
		TickSystem<object>.AddTickCallback(this);
	}

	// Token: 0x060032E5 RID: 13029 RVA: 0x0002B7C1 File Offset: 0x000299C1
	public virtual void OnDisable()
	{
		TickSystem<object>.RemoveTickCallback(this);
	}

	// Token: 0x060032E6 RID: 13030 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void Tick()
	{
	}
}
