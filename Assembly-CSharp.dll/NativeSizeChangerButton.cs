using System;
using UnityEngine;

// Token: 0x02000252 RID: 594
public class NativeSizeChangerButton : GorillaPressableButton
{
	// Token: 0x06000DC8 RID: 3528 RVA: 0x00038E75 File Offset: 0x00037075
	public override void ButtonActivation()
	{
		this.nativeSizeChanger.Activate(this.settings);
	}

	// Token: 0x040010D1 RID: 4305
	[SerializeField]
	private NativeSizeChanger nativeSizeChanger;

	// Token: 0x040010D2 RID: 4306
	[SerializeField]
	private NativeSizeChangerSettings settings;
}
