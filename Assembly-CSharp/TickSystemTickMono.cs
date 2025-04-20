using System;
using UnityEngine;

// Token: 0x0200082D RID: 2093
internal abstract class TickSystemTickMono : MonoBehaviour, ITickSystemTick
{
	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x06003391 RID: 13201 RVA: 0x00051F6D File Offset: 0x0005016D
	// (set) Token: 0x06003392 RID: 13202 RVA: 0x00051F75 File Offset: 0x00050175
	public bool TickRunning { get; set; }

	// Token: 0x06003393 RID: 13203 RVA: 0x00035BD9 File Offset: 0x00033DD9
	public virtual void OnEnable()
	{
		TickSystem<object>.AddTickCallback(this);
	}

	// Token: 0x06003394 RID: 13204 RVA: 0x00035BE1 File Offset: 0x00033DE1
	public virtual void OnDisable()
	{
		TickSystem<object>.RemoveTickCallback(this);
	}

	// Token: 0x06003395 RID: 13205 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void Tick()
	{
	}
}
