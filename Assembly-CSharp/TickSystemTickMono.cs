using System;
using UnityEngine;

// Token: 0x02000813 RID: 2067
internal abstract class TickSystemTickMono : MonoBehaviour, ITickSystemTick
{
	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x060032D6 RID: 13014 RVA: 0x000F3A1F File Offset: 0x000F1C1F
	// (set) Token: 0x060032D7 RID: 13015 RVA: 0x000F3A27 File Offset: 0x000F1C27
	public bool TickRunning { get; set; }

	// Token: 0x060032D8 RID: 13016 RVA: 0x0002B495 File Offset: 0x00029695
	public virtual void OnEnable()
	{
		TickSystem<object>.AddTickCallback(this);
	}

	// Token: 0x060032D9 RID: 13017 RVA: 0x0002B49D File Offset: 0x0002969D
	public virtual void OnDisable()
	{
		TickSystem<object>.RemoveTickCallback(this);
	}

	// Token: 0x060032DA RID: 13018 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void Tick()
	{
	}
}
