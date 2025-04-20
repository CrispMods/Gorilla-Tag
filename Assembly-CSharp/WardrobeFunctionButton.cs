using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200043C RID: 1084
public class WardrobeFunctionButton : GorillaPressableButton
{
	// Token: 0x06001ACF RID: 6863 RVA: 0x0004227B File Offset: 0x0004047B
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		CosmeticsController.instance.PressWardrobeFunctionButton(this.function);
		base.StartCoroutine(this.ButtonColorUpdate());
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x00030607 File Offset: 0x0002E807
	public override void UpdateColor()
	{
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x000422A2 File Offset: 0x000404A2
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.buttonFadeTime);
		this.buttonRenderer.material = this.unpressedMaterial;
		yield break;
	}

	// Token: 0x04001D98 RID: 7576
	public string function;

	// Token: 0x04001D99 RID: 7577
	public float buttonFadeTime = 0.25f;
}
