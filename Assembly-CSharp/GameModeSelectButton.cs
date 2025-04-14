using System;
using UnityEngine;

// Token: 0x0200046B RID: 1131
public class GameModeSelectButton : GorillaPressableButton
{
	// Token: 0x06001BB4 RID: 7092 RVA: 0x00087847 File Offset: 0x00085A47
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.selector.SelectEntryOnPage(this.buttonIndex);
	}

	// Token: 0x04001E90 RID: 7824
	[SerializeField]
	internal GameModePages selector;

	// Token: 0x04001E91 RID: 7825
	[SerializeField]
	internal int buttonIndex;
}
