using System;
using System.Collections;
using GorillaNetworking.Store;
using UnityEngine;

// Token: 0x02000431 RID: 1073
public class TryOnPurchaseButton : GorillaPressableButton
{
	// Token: 0x06001A96 RID: 6806 RVA: 0x000D6A50 File Offset: 0x000D4C50
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

	// Token: 0x06001A97 RID: 6807 RVA: 0x00041F8B File Offset: 0x0004018B
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

	// Token: 0x06001A98 RID: 6808 RVA: 0x00041FB5 File Offset: 0x000401B5
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

	// Token: 0x06001A99 RID: 6809 RVA: 0x00041FF5 File Offset: 0x000401F5
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

	// Token: 0x06001A9A RID: 6810 RVA: 0x00042035 File Offset: 0x00040235
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.debounceTime);
		this.buttonRenderer.material = (this.isOn ? this.pressedMaterial : this.unpressedMaterial);
		yield break;
	}

	// Token: 0x06001A9B RID: 6811 RVA: 0x00042044 File Offset: 0x00040244
	public void ErrorHappened()
	{
		this.bError = true;
		this.myText.text = this.ErrorText;
		this.buttonRenderer.material = this.unpressedMaterial;
		base.enabled = false;
		this.isOn = false;
	}

	// Token: 0x04001D5C RID: 7516
	public bool bError;

	// Token: 0x04001D5D RID: 7517
	public string ErrorText = "ERROR COMPLETING PURCHASE! PLEASE RESTART THE GAME";

	// Token: 0x04001D5E RID: 7518
	public string AlreadyOwnText;
}
