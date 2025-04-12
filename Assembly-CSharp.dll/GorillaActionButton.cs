using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001AF RID: 431
public class GorillaActionButton : GorillaPressableButton
{
	// Token: 0x06000A26 RID: 2598 RVA: 0x00036262 File Offset: 0x00034462
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.onPress.Invoke();
	}

	// Token: 0x04000C88 RID: 3208
	[SerializeField]
	public UnityEvent onPress;
}
