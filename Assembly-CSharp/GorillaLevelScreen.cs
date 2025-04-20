using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000586 RID: 1414
public class GorillaLevelScreen : MonoBehaviour
{
	// Token: 0x060022D5 RID: 8917 RVA: 0x00047959 File Offset: 0x00045B59
	private void Awake()
	{
		if (this.myText != null)
		{
			this.startingText = this.myText.text;
		}
	}

	// Token: 0x060022D6 RID: 8918 RVA: 0x000FB204 File Offset: 0x000F9404
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

	// Token: 0x04002666 RID: 9830
	public string startingText;

	// Token: 0x04002667 RID: 9831
	public Material goodMaterial;

	// Token: 0x04002668 RID: 9832
	public Material badMaterial;

	// Token: 0x04002669 RID: 9833
	public Text myText;
}
