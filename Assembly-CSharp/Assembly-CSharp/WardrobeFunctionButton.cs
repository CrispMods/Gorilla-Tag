using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000430 RID: 1072
public class WardrobeFunctionButton : GorillaPressableButton
{
	// Token: 0x06001A7E RID: 6782 RVA: 0x0008313E File Offset: 0x0008133E
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		CosmeticsController.instance.PressWardrobeFunctionButton(this.function);
		base.StartCoroutine(this.ButtonColorUpdate());
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void UpdateColor()
	{
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x00083165 File Offset: 0x00081365
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.buttonFadeTime);
		this.buttonRenderer.material = this.unpressedMaterial;
		yield break;
	}

	// Token: 0x04001D4A RID: 7498
	public string function;

	// Token: 0x04001D4B RID: 7499
	public float buttonFadeTime = 0.25f;
}
