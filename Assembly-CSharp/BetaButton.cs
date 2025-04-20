using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003C8 RID: 968
public class BetaButton : GorillaPressableButton
{
	// Token: 0x06001776 RID: 6006 RVA: 0x000C8918 File Offset: 0x000C6B18
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.count++;
		base.StartCoroutine(this.ButtonColorUpdate());
		if (this.count >= 10)
		{
			this.betaParent.SetActive(false);
			PlayerPrefs.SetString("CheckedBox2", "true");
			PlayerPrefs.Save();
		}
	}

	// Token: 0x06001777 RID: 6007 RVA: 0x0003FE41 File Offset: 0x0003E041
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.buttonFadeTime);
		this.buttonRenderer.material = this.unpressedMaterial;
		yield break;
	}

	// Token: 0x04001A25 RID: 6693
	public GameObject betaParent;

	// Token: 0x04001A26 RID: 6694
	public int count;

	// Token: 0x04001A27 RID: 6695
	public float buttonFadeTime = 0.25f;

	// Token: 0x04001A28 RID: 6696
	public Text messageText;
}
