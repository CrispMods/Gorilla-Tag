using System;
using UnityEngine;

// Token: 0x02000817 RID: 2071
internal abstract class TickSystemPostTickMono : MonoBehaviour, ITickSystemPost
{
	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x060032E8 RID: 13032 RVA: 0x00050B70 File Offset: 0x0004ED70
	// (set) Token: 0x060032E9 RID: 13033 RVA: 0x00050B78 File Offset: 0x0004ED78
	public bool PostTickRunning { get; set; }

	// Token: 0x060032EA RID: 13034 RVA: 0x00050B81 File Offset: 0x0004ED81
	public virtual void OnEnable()
	{
		TickSystem<object>.AddPostTickCallback(this);
	}

	// Token: 0x060032EB RID: 13035 RVA: 0x00049FD9 File Offset: 0x000481D9
	public virtual void OnDisable()
	{
		TickSystem<object>.RemovePostTickCallback(this);
	}

	// Token: 0x060032EC RID: 13036 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void PostTick()
	{
	}
}
