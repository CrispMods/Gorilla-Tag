using System;
using UnityEngine;

// Token: 0x0200082B RID: 2091
internal abstract class TickSystemMono : MonoBehaviour, ITickSystem, ITickSystemPre, ITickSystemTick, ITickSystemPost
{
	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x0600337F RID: 13183 RVA: 0x00051F09 File Offset: 0x00050109
	// (set) Token: 0x06003380 RID: 13184 RVA: 0x00051F11 File Offset: 0x00050111
	public bool PreTickRunning { get; set; }

	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x06003381 RID: 13185 RVA: 0x00051F1A File Offset: 0x0005011A
	// (set) Token: 0x06003382 RID: 13186 RVA: 0x00051F22 File Offset: 0x00050122
	public bool TickRunning { get; set; }

	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x06003383 RID: 13187 RVA: 0x00051F2B File Offset: 0x0005012B
	// (set) Token: 0x06003384 RID: 13188 RVA: 0x00051F33 File Offset: 0x00050133
	public bool PostTickRunning { get; set; }

	// Token: 0x06003385 RID: 13189 RVA: 0x00051F3C File Offset: 0x0005013C
	public virtual void OnEnable()
	{
		TickSystem<object>.AddTickSystemCallBack(this);
	}

	// Token: 0x06003386 RID: 13190 RVA: 0x00051F44 File Offset: 0x00050144
	public virtual void OnDisable()
	{
		TickSystem<object>.RemoveTickSystemCallback(this);
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void PreTick()
	{
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void Tick()
	{
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void PostTick()
	{
	}
}
