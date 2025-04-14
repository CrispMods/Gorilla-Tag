using System;
using UnityEngine;

// Token: 0x02000252 RID: 594
public class NativeSizeChangerButton : GorillaPressableButton
{
	// Token: 0x06000DC6 RID: 3526 RVA: 0x00046144 File Offset: 0x00044344
	public override void ButtonActivation()
	{
		this.nativeSizeChanger.Activate(this.settings);
	}

	// Token: 0x040010D0 RID: 4304
	[SerializeField]
	private NativeSizeChanger nativeSizeChanger;

	// Token: 0x040010D1 RID: 4305
	[SerializeField]
	private NativeSizeChangerSettings settings;
}
