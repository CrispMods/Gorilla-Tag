using System;
using UnityEngine;

// Token: 0x0200082E RID: 2094
internal abstract class TickSystemPostTickMono : MonoBehaviour, ITickSystemPost
{
	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x06003397 RID: 13207 RVA: 0x00051F7E File Offset: 0x0005017E
	// (set) Token: 0x06003398 RID: 13208 RVA: 0x00051F86 File Offset: 0x00050186
	public bool PostTickRunning { get; set; }

	// Token: 0x06003399 RID: 13209 RVA: 0x00051F8F File Offset: 0x0005018F
	public virtual void OnEnable()
	{
		TickSystem<object>.AddPostTickCallback(this);
	}

	// Token: 0x0600339A RID: 13210 RVA: 0x0004A56E File Offset: 0x0004876E
	public virtual void OnDisable()
	{
		TickSystem<object>.RemovePostTickCallback(this);
	}

	// Token: 0x0600339B RID: 13211 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void PostTick()
	{
	}
}
