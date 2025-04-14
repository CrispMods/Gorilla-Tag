using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000430 RID: 1072
public class WardrobeFunctionButton : GorillaPressableButton
{
	// Token: 0x06001A7B RID: 6779 RVA: 0x00082DBA File Offset: 0x00080FBA
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		CosmeticsController.instance.PressWardrobeFunctionButton(this.function);
		base.StartCoroutine(this.ButtonColorUpdate());
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void UpdateColor()
	{
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x00082DE1 File Offset: 0x00080FE1
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.buttonFadeTime);
		this.buttonRenderer.material = this.unpressedMaterial;
		yield break;
	}

	// Token: 0x04001D49 RID: 7497
	public string function;

	// Token: 0x04001D4A RID: 7498
	public float buttonFadeTime = 0.25f;
}
