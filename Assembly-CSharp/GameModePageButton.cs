using System;
using UnityEngine;

// Token: 0x02000469 RID: 1129
public class GameModePageButton : GorillaPressableButton
{
	// Token: 0x06001BA5 RID: 7077 RVA: 0x00087570 File Offset: 0x00085770
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.selector.ChangePage(this.left);
	}

	// Token: 0x04001E87 RID: 7815
	[SerializeField]
	private GameModePages selector;

	// Token: 0x04001E88 RID: 7816
	[SerializeField]
	private bool left;
}
