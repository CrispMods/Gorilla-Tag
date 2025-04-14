using System;
using UnityEngine;

// Token: 0x02000469 RID: 1129
public class GameModePageButton : GorillaPressableButton
{
	// Token: 0x06001BA8 RID: 7080 RVA: 0x000878F4 File Offset: 0x00085AF4
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.selector.ChangePage(this.left);
	}

	// Token: 0x04001E88 RID: 7816
	[SerializeField]
	private GameModePages selector;

	// Token: 0x04001E89 RID: 7817
	[SerializeField]
	private bool left;
}
