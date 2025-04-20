using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020003D4 RID: 980
[Obsolete("Replaced with bundlebutton")]
public class EarlyAccessButton : GorillaPressableButton
{
	// Token: 0x0600179A RID: 6042 RVA: 0x00030607 File Offset: 0x0002E807
	private void Awake()
	{
	}

	// Token: 0x0600179B RID: 6043 RVA: 0x000C8F94 File Offset: 0x000C7194
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

	// Token: 0x0600179C RID: 6044 RVA: 0x0003FF8F File Offset: 0x0003E18F
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		CosmeticsController.instance.PressEarlyAccessButton();
		base.StartCoroutine(this.ButtonColorUpdate());
	}

	// Token: 0x0600179D RID: 6045 RVA: 0x0003FFB0 File Offset: 0x0003E1B0
	public void AlreadyOwn()
	{
		base.enabled = false;
		base.GetComponent<BoxCollider>().enabled = false;
		this.buttonRenderer.material = this.pressedMaterial;
		this.myText.text = "YOU OWN THE BUNDLE ALREADY! THANK YOU!";
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x0003FFE6 File Offset: 0x0003E1E6
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.debounceTime);
		this.buttonRenderer.material = (this.isOn ? this.pressedMaterial : this.unpressedMaterial);
		yield break;
	}
}
