using System;
using UnityEngine;

// Token: 0x020003BA RID: 954
[RequireComponent(typeof(RigContainer))]
public class VRRigEvents : MonoBehaviour, IPreDisable
{
	// Token: 0x06001707 RID: 5895 RVA: 0x0003EA4A File Offset: 0x0003CC4A
	public void PreDisable()
	{
		Action<RigContainer> action = this.disableEvent;
		if (action == null)
		{
			return;
		}
		action(this.rigRef);
	}

	// Token: 0x040019B0 RID: 6576
	[SerializeField]
	private RigContainer rigRef;

	// Token: 0x040019B1 RID: 6577
	public Action<RigContainer> disableEvent;
}
