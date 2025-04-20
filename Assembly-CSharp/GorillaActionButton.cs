using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001BA RID: 442
public class GorillaActionButton : GorillaPressableButton
{
	// Token: 0x06000A70 RID: 2672 RVA: 0x00037522 File Offset: 0x00035722
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.onPress.Invoke();
	}

	// Token: 0x04000CCD RID: 3277
	[SerializeField]
	public UnityEvent onPress;
}
