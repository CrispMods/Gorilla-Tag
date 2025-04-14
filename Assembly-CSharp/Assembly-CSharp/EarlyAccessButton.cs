using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020003C9 RID: 969
[Obsolete("Replaced with bundlebutton")]
public class EarlyAccessButton : GorillaPressableButton
{
	// Token: 0x06001750 RID: 5968 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Awake()
	{
	}

	// Token: 0x06001751 RID: 5969 RVA: 0x00072208 File Offset: 0x00070408
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

	// Token: 0x06001752 RID: 5970 RVA: 0x00072262 File Offset: 0x00070462
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		CosmeticsController.instance.PressEarlyAccessButton();
		base.StartCoroutine(this.ButtonColorUpdate());
	}

	// Token: 0x06001753 RID: 5971 RVA: 0x00072283 File Offset: 0x00070483
	public void AlreadyOwn()
	{
		base.enabled = false;
		base.GetComponent<BoxCollider>().enabled = false;
		this.buttonRenderer.material = this.pressedMaterial;
		this.myText.text = "YOU OWN THE BUNDLE ALREADY! THANK YOU!";
	}

	// Token: 0x06001754 RID: 5972 RVA: 0x000722B9 File Offset: 0x000704B9
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.debounceTime);
		this.buttonRenderer.material = (this.isOn ? this.pressedMaterial : this.unpressedMaterial);
		yield break;
	}
}
