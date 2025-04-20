using System;
using TMPro;
using UnityEngine;

// Token: 0x02000450 RID: 1104
public class RandomizeLabel : MonoBehaviour
{
	// Token: 0x06001B38 RID: 6968 RVA: 0x000427E2 File Offset: 0x000409E2
	public void Randomize()
	{
		this.strings.distinct = this.distinct;
		this.label.text = this.strings.NextItem();
	}

	// Token: 0x04001E11 RID: 7697
	public TMP_Text label;

	// Token: 0x04001E12 RID: 7698
	public RandomStrings strings;

	// Token: 0x04001E13 RID: 7699
	public bool distinct;
}
