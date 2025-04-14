using System;
using UnityEngine;

// Token: 0x02000814 RID: 2068
internal abstract class TickSystemPostTickMono : MonoBehaviour, ITickSystemPost
{
	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x060032DC RID: 13020 RVA: 0x000F3A30 File Offset: 0x000F1C30
	// (set) Token: 0x060032DD RID: 13021 RVA: 0x000F3A38 File Offset: 0x000F1C38
	public bool PostTickRunning { get; set; }

	// Token: 0x060032DE RID: 13022 RVA: 0x000F3A41 File Offset: 0x000F1C41
	public virtual void OnEnable()
	{
		TickSystem<object>.AddPostTickCallback(this);
	}

	// Token: 0x060032DF RID: 13023 RVA: 0x000C141F File Offset: 0x000BF61F
	public virtual void OnDisable()
	{
		TickSystem<object>.RemovePostTickCallback(this);
	}

	// Token: 0x060032E0 RID: 13024 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void PostTick()
	{
	}
}
