using System;
using System.Collections;
using GorillaNetworking.Store;
using UnityEngine;

// Token: 0x02000425 RID: 1061
public class TryOnPurchaseButton : GorillaPressableButton
{
	// Token: 0x06001A45 RID: 6725 RVA: 0x00081880 File Offset: 0x0007FA80
	public void Update()
	{
		if (NetworkSystem.Instance != null && NetworkSystem.Instance.WrongVersion && !this.bError)
		{
			base.enabled = false;
			base.GetComponent<BoxCollider>().enabled = false;
			this.buttonRenderer.material = this.pressedMaterial;
			this.myText.text = "UNAVAILABLE";
		}
	}

	// Token: 0x06001A46 RID: 6726 RVA: 0x000818E2 File Offset: 0x0007FAE2
	public override void ButtonActivation()
	{
		if (this.bError)
		{
			return;
		}
		base.ButtonActivation();
		BundleManager.instance.PressPurchaseTryOnBundleButton();
		base.StartCoroutine(this.ButtonColorUpdate());
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x0008190C File Offset: 0x0007FB0C
	public void AlreadyOwn()
	{
		if (this.bError)
		{
			return;
		}
		base.enabled = false;
		base.GetComponent<BoxCollider>().enabled = false;
		this.buttonRenderer.material = this.pressedMaterial;
		this.myText.text = this.AlreadyOwnText;
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x0008194C File Offset: 0x0007FB4C
	public void ResetButton()
	{
		if (this.bError)
		{
			return;
		}
		base.enabled = true;
		base.GetComponent<BoxCollider>().enabled = true;
		this.buttonRenderer.material = this.unpressedMaterial;
		this.myText.text = this.offText;
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x0008198C File Offset: 0x0007FB8C
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.debounceTime);
		this.buttonRenderer.material = (this.isOn ? this.pressedMaterial : this.unpressedMaterial);
		yield break;
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x0008199B File Offset: 0x0007FB9B
	public void ErrorHappened()
	{
		this.bError = true;
		this.myText.text = this.ErrorText;
		this.buttonRenderer.material = this.unpressedMaterial;
		base.enabled = false;
		this.isOn = false;
	}

	// Token: 0x04001D0E RID: 7438
	public bool bError;

	// Token: 0x04001D0F RID: 7439
	public string ErrorText = "ERROR COMPLETING PURCHASE! PLEASE RESTART THE GAME";

	// Token: 0x04001D10 RID: 7440
	public string AlreadyOwnText;
}
