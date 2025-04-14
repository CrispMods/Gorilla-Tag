using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000578 RID: 1400
public class GorillaLevelScreen : MonoBehaviour
{
	// Token: 0x06002277 RID: 8823 RVA: 0x000AB4EE File Offset: 0x000A96EE
	private void Awake()
	{
		if (this.myText != null)
		{
			this.startingText = this.myText.text;
		}
	}

	// Token: 0x06002278 RID: 8824 RVA: 0x000AB510 File Offset: 0x000A9710
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

	// Token: 0x0400260E RID: 9742
	public string startingText;

	// Token: 0x0400260F RID: 9743
	public Material goodMaterial;

	// Token: 0x04002610 RID: 9744
	public Material badMaterial;

	// Token: 0x04002611 RID: 9745
	public Text myText;
}
