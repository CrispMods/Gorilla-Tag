using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005FE RID: 1534
public class ModIOLinkButton : GorillaPressableButton
{
	// Token: 0x06002618 RID: 9752 RVA: 0x00048E73 File Offset: 0x00047073
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		base.StartCoroutine(this.ButtonPressed_Local());
		if (this.AccountLinkingTerminal != null)
		{
			this.AccountLinkingTerminal.LinkButtonPressed();
		}
	}

	// Token: 0x06002619 RID: 9753 RVA: 0x00048EA1 File Offset: 0x000470A1
	private IEnumerator ButtonPressed_Local()
	{
		this.isOn = true;
		this.UpdateColor();
		yield return new WaitForSeconds(this.pressedTime);
		this.isOn = false;
		this.UpdateColor();
		yield break;
	}

	// Token: 0x04002A1F RID: 10783
	[SerializeField]
	private float pressedTime = 0.2f;

	// Token: 0x04002A20 RID: 10784
	[SerializeField]
	private ModIOAccountLinkingTerminal AccountLinkingTerminal;
}
