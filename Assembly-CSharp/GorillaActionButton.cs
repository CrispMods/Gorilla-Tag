using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001AF RID: 431
public class GorillaActionButton : GorillaPressableButton
{
	// Token: 0x06000A24 RID: 2596 RVA: 0x00037937 File Offset: 0x00035B37
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.onPress.Invoke();
	}

	// Token: 0x04000C87 RID: 3207
	[SerializeField]
	public UnityEvent onPress;
}
