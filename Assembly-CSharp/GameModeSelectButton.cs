using System;
using UnityEngine;

// Token: 0x02000477 RID: 1143
public class GameModeSelectButton : GorillaPressableButton
{
	// Token: 0x06001C08 RID: 7176 RVA: 0x00043416 File Offset: 0x00041616
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.selector.SelectEntryOnPage(this.buttonIndex);
	}

	// Token: 0x04001EDF RID: 7903
	[SerializeField]
	internal GameModePages selector;

	// Token: 0x04001EE0 RID: 7904
	[SerializeField]
	internal int buttonIndex;
}
