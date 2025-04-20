using System;
using UnityEngine;

// Token: 0x0200082C RID: 2092
internal abstract class TickSystemPreTickMono : MonoBehaviour, ITickSystemPre
{
	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x0600338B RID: 13195 RVA: 0x00051F4C File Offset: 0x0005014C
	// (set) Token: 0x0600338C RID: 13196 RVA: 0x00051F54 File Offset: 0x00050154
	public bool PreTickRunning { get; set; }

	// Token: 0x0600338D RID: 13197 RVA: 0x00051F5D File Offset: 0x0005015D
	public virtual void OnEnable()
	{
		TickSystem<object>.AddPreTickCallback(this);
	}

	// Token: 0x0600338E RID: 13198 RVA: 0x00051F65 File Offset: 0x00050165
	public void OnDisable()
	{
		TickSystem<object>.RemovePreTickCallback(this);
	}

	// Token: 0x0600338F RID: 13199 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void PreTick()
	{
	}
}
