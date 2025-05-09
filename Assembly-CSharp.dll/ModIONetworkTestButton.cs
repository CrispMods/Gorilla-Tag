﻿using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000617 RID: 1559
public class ModIONetworkTestButton : GorillaPressableButton
{
	// Token: 0x060026E6 RID: 9958 RVA: 0x000497FD File Offset: 0x000479FD
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.networkTest.IncrementIndex();
	}

	// Token: 0x04002AB9 RID: 10937
	[SerializeField]
	private ModIONetworkTest networkTest;
}
