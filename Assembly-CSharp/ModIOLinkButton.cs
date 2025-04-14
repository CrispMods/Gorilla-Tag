using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005FD RID: 1533
public class ModIOLinkButton : GorillaPressableButton
{
	// Token: 0x06002610 RID: 9744 RVA: 0x000BBED6 File Offset: 0x000BA0D6
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		base.StartCoroutine(this.ButtonPressed_Local());
		if (this.AccountLinkingTerminal != null)
		{
			this.AccountLinkingTerminal.LinkButtonPressed();
		}
	}

	// Token: 0x06002611 RID: 9745 RVA: 0x000BBF04 File Offset: 0x000BA104
	private IEnumerator ButtonPressed_Local()
	{
		this.isOn = true;
		this.UpdateColor();
		yield return new WaitForSeconds(this.pressedTime);
		this.isOn = false;
		this.UpdateColor();
		yield break;
	}

	// Token: 0x04002A19 RID: 10777
	[SerializeField]
	private float pressedTime = 0.2f;

	// Token: 0x04002A1A RID: 10778
	[SerializeField]
	private ModIOAccountLinkingTerminal AccountLinkingTerminal;
}
