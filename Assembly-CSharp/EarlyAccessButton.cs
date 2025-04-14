using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020003C9 RID: 969
[Obsolete("Replaced with bundlebutton")]
public class EarlyAccessButton : GorillaPressableButton
{
	// Token: 0x0600174D RID: 5965 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Awake()
	{
	}

	// Token: 0x0600174E RID: 5966 RVA: 0x00071E84 File Offset: 0x00070084
	public void Update()
	{
		if (NetworkSystem.Instance != null && NetworkSystem.Instance.WrongVersion)
		{
			base.enabled = false;
			base.GetComponent<BoxCollider>().enabled = false;
			this.buttonRenderer.material = this.pressedMaterial;
			this.myText.text = "UNAVAILABLE";
		}
	}

	// Token: 0x0600174F RID: 5967 RVA: 0x00071EDE File Offset: 0x000700DE
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		CosmeticsController.instance.PressEarlyAccessButton();
		base.StartCoroutine(this.ButtonColorUpdate());
	}

	// Token: 0x06001750 RID: 5968 RVA: 0x00071EFF File Offset: 0x000700FF
	public void AlreadyOwn()
	{
		base.enabled = false;
		base.GetComponent<BoxCollider>().enabled = false;
		this.buttonRenderer.material = this.pressedMaterial;
		this.myText.text = "YOU OWN THE BUNDLE ALREADY! THANK YOU!";
	}

	// Token: 0x06001751 RID: 5969 RVA: 0x00071F35 File Offset: 0x00070135
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.debounceTime);
		this.buttonRenderer.material = (this.isOn ? this.pressedMaterial : this.unpressedMaterial);
		yield break;
	}
}
