using System;
using UnityEngine;

// Token: 0x020003C5 RID: 965
[RequireComponent(typeof(RigContainer))]
public class VRRigEvents : MonoBehaviour, IPreDisable
{
	// Token: 0x06001751 RID: 5969 RVA: 0x0003FD34 File Offset: 0x0003DF34
	public void PreDisable()
	{
		Action<RigContainer> action = this.disableEvent;
		if (action == null)
		{
			return;
		}
		action(this.rigRef);
	}

	// Token: 0x040019F8 RID: 6648
	[SerializeField]
	private RigContainer rigRef;

	// Token: 0x040019F9 RID: 6649
	public Action<RigContainer> disableEvent;
}
