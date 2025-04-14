using System;
using UnityEngine;

// Token: 0x020003BA RID: 954
[RequireComponent(typeof(RigContainer))]
public class VRRigEvents : MonoBehaviour, IPreDisable
{
	// Token: 0x06001704 RID: 5892 RVA: 0x00070ADD File Offset: 0x0006ECDD
	public void PreDisable()
	{
		Action<RigContainer> action = this.disableEvent;
		if (action == null)
		{
			return;
		}
		action(this.rigRef);
	}

	// Token: 0x040019AF RID: 6575
	[SerializeField]
	private RigContainer rigRef;

	// Token: 0x040019B0 RID: 6576
	public Action<RigContainer> disableEvent;
}
