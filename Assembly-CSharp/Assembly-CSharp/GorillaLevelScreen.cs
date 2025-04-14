using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000579 RID: 1401
public class GorillaLevelScreen : MonoBehaviour
{
	// Token: 0x0600227F RID: 8831 RVA: 0x000AB96E File Offset: 0x000A9B6E
	private void Awake()
	{
		if (this.myText != null)
		{
			this.startingText = this.myText.text;
		}
	}

	// Token: 0x06002280 RID: 8832 RVA: 0x000AB990 File Offset: 0x000A9B90
	public void UpdateText(string newText, bool setToGoodMaterial)
	{
		if (this.myText != null)
		{
			this.myText.text = newText;
		}
		Material[] materials = base.GetComponent<MeshRenderer>().materials;
		materials[0] = (setToGoodMaterial ? this.goodMaterial : this.badMaterial);
		base.GetComponent<MeshRenderer>().materials = materials;
	}

	// Token: 0x04002614 RID: 9748
	public string startingText;

	// Token: 0x04002615 RID: 9749
	public Material goodMaterial;

	// Token: 0x04002616 RID: 9750
	public Material badMaterial;

	// Token: 0x04002617 RID: 9751
	public Text myText;
}
