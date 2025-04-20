using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000678 RID: 1656
public class ModIOLinkButton : GorillaPressableButton
{
	// Token: 0x0600291E RID: 10526 RVA: 0x0004BD8A File Offset: 0x00049F8A
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		base.StartCoroutine(this.ButtonPressed_Local());
		if (this.AccountLinkingTerminal != null)
		{
			this.AccountLinkingTerminal.LinkButtonPressed();
		}
	}

	// Token: 0x0600291F RID: 10527 RVA: 0x0004BDB8 File Offset: 0x00049FB8
	private IEnumerator ButtonPressed_Local()
	{
		this.isOn = true;
		this.UpdateColor();
		yield return new WaitForSeconds(this.pressedTime);
		this.isOn = false;
		this.UpdateColor();
		yield break;
	}

	// Token: 0x04002E79 RID: 11897
	[SerializeField]
	private float pressedTime = 0.2f;

	// Token: 0x04002E7A RID: 11898
	[SerializeField]
	private ModIOAccountLinkingTerminal AccountLinkingTerminal;
}
