using System;
using TMPro;
using UnityEngine;

// Token: 0x02000444 RID: 1092
public class RandomizeLabel : MonoBehaviour
{
	// Token: 0x06001AE4 RID: 6884 RVA: 0x000845DC File Offset: 0x000827DC
	public void Randomize()
	{
		this.strings.distinct = this.distinct;
		this.label.text = this.strings.NextItem();
	}

	// Token: 0x04001DC2 RID: 7618
	public TMP_Text label;

	// Token: 0x04001DC3 RID: 7619
	public RandomStrings strings;

	// Token: 0x04001DC4 RID: 7620
	public bool distinct;
}
