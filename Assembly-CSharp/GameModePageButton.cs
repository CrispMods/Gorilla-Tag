using System;
using UnityEngine;

// Token: 0x02000475 RID: 1141
public class GameModePageButton : GorillaPressableButton
{
	// Token: 0x06001BF9 RID: 7161 RVA: 0x0004334B File Offset: 0x0004154B
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.selector.ChangePage(this.left);
	}

	// Token: 0x04001ED6 RID: 7894
	[SerializeField]
	private GameModePages selector;

	// Token: 0x04001ED7 RID: 7895
	[SerializeField]
	private bool left;
}
