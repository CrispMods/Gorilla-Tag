using System;
using TMPro;
using UnityEngine;

// Token: 0x02000444 RID: 1092
public class RandomizeLabel : MonoBehaviour
{
	// Token: 0x06001AE7 RID: 6887 RVA: 0x000414A9 File Offset: 0x0003F6A9
	public void Randomize()
	{
		this.strings.distinct = this.distinct;
		this.label.text = this.strings.NextItem();
	}

	// Token: 0x04001DC3 RID: 7619
	public TMP_Text label;

	// Token: 0x04001DC4 RID: 7620
	public RandomStrings strings;

	// Token: 0x04001DC5 RID: 7621
	public bool distinct;
}
