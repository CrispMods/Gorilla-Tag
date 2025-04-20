using System;
using UnityEngine;

// Token: 0x0200025D RID: 605
public class NativeSizeChangerButton : GorillaPressableButton
{
	// Token: 0x06000E11 RID: 3601 RVA: 0x0003A135 File Offset: 0x00038335
	public override void ButtonActivation()
	{
		this.nativeSizeChanger.Activate(this.settings);
	}

	// Token: 0x04001116 RID: 4374
	[SerializeField]
	private NativeSizeChanger nativeSizeChanger;

	// Token: 0x04001117 RID: 4375
	[SerializeField]
	private NativeSizeChangerSettings settings;
}
