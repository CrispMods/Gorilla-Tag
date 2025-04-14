using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003BD RID: 957
public class BetaButton : GorillaPressableButton
{
	// Token: 0x0600172C RID: 5932 RVA: 0x00071A38 File Offset: 0x0006FC38
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

	// Token: 0x0600172D RID: 5933 RVA: 0x00071A90 File Offset: 0x0006FC90
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.buttonFadeTime);
		this.buttonRenderer.material = this.unpressedMaterial;
		yield break;
	}

	// Token: 0x040019DD RID: 6621
	public GameObject betaParent;

	// Token: 0x040019DE RID: 6622
	public int count;

	// Token: 0x040019DF RID: 6623
	public float buttonFadeTime = 0.25f;

	// Token: 0x040019E0 RID: 6624
	public Text messageText;
}
