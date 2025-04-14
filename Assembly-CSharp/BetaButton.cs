using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003BD RID: 957
public class BetaButton : GorillaPressableButton
{
	// Token: 0x06001729 RID: 5929 RVA: 0x000716B4 File Offset: 0x0006F8B4
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

	// Token: 0x0600172A RID: 5930 RVA: 0x0007170C File Offset: 0x0006F90C
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.buttonFadeTime);
		this.buttonRenderer.material = this.unpressedMaterial;
		yield break;
	}

	// Token: 0x040019DC RID: 6620
	public GameObject betaParent;

	// Token: 0x040019DD RID: 6621
	public int count;

	// Token: 0x040019DE RID: 6622
	public float buttonFadeTime = 0.25f;

	// Token: 0x040019DF RID: 6623
	public Text messageText;
}
