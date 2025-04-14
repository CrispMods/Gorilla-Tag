using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000616 RID: 1558
public class ModIONetworkTestButton : GorillaPressableButton
{
	// Token: 0x060026DE RID: 9950 RVA: 0x000BF478 File Offset: 0x000BD678
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.networkTest.IncrementIndex();
	}

	// Token: 0x04002AB3 RID: 10931
	[SerializeField]
	private ModIONetworkTest networkTest;
}
