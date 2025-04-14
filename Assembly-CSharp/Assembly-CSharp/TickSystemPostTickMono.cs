using System;
using UnityEngine;

// Token: 0x02000817 RID: 2071
internal abstract class TickSystemPostTickMono : MonoBehaviour, ITickSystemPost
{
	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x060032E8 RID: 13032 RVA: 0x000F3FF8 File Offset: 0x000F21F8
	// (set) Token: 0x060032E9 RID: 13033 RVA: 0x000F4000 File Offset: 0x000F2200
	public bool PostTickRunning { get; set; }

	// Token: 0x060032EA RID: 13034 RVA: 0x000F4009 File Offset: 0x000F2209
	public virtual void OnEnable()
	{
		TickSystem<object>.AddPostTickCallback(this);
	}

	// Token: 0x060032EB RID: 13035 RVA: 0x000C189F File Offset: 0x000BFA9F
	public virtual void OnDisable()
	{
		TickSystem<object>.RemovePostTickCallback(this);
	}

	// Token: 0x060032EC RID: 13036 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void PostTick()
	{
	}
}
