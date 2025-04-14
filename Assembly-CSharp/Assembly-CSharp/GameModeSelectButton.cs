using System;
using UnityEngine;

// Token: 0x0200046B RID: 1131
public class GameModeSelectButton : GorillaPressableButton
{
	// Token: 0x06001BB7 RID: 7095 RVA: 0x00087BCB File Offset: 0x00085DCB
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.selector.SelectEntryOnPage(this.buttonIndex);
	}

	// Token: 0x04001E91 RID: 7825
	[SerializeField]
	internal GameModePages selector;

	// Token: 0x04001E92 RID: 7826
	[SerializeField]
	internal int buttonIndex;
}
