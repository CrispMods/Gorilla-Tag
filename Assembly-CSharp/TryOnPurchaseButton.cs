using System;
using System.Collections;
using GorillaNetworking.Store;
using UnityEngine;

// Token: 0x02000425 RID: 1061
public class TryOnPurchaseButton : GorillaPressableButton
{
	// Token: 0x06001A42 RID: 6722 RVA: 0x000814FC File Offset: 0x0007F6FC
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

	// Token: 0x06001A43 RID: 6723 RVA: 0x0008155E File Offset: 0x0007F75E
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

	// Token: 0x06001A44 RID: 6724 RVA: 0x00081588 File Offset: 0x0007F788
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

	// Token: 0x06001A45 RID: 6725 RVA: 0x000815C8 File Offset: 0x0007F7C8
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

	// Token: 0x06001A46 RID: 6726 RVA: 0x00081608 File Offset: 0x0007F808
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.debounceTime);
		this.buttonRenderer.material = (this.isOn ? this.pressedMaterial : this.unpressedMaterial);
		yield break;
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x00081617 File Offset: 0x0007F817
	public void ErrorHappened()
	{
		this.bError = true;
		this.myText.text = this.ErrorText;
		this.buttonRenderer.material = this.unpressedMaterial;
		base.enabled = false;
		this.isOn = false;
	}

	// Token: 0x04001D0D RID: 7437
	public bool bError;

	// Token: 0x04001D0E RID: 7438
	public string ErrorText = "ERROR COMPLETING PURCHASE! PLEASE RESTART THE GAME";

	// Token: 0x04001D0F RID: 7439
	public string AlreadyOwnText;
}
