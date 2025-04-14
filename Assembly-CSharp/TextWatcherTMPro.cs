using System;
using TMPro;
using UnityEngine;

// Token: 0x0200064C RID: 1612
public class TextWatcherTMPro : MonoBehaviour
{
	// Token: 0x060027FD RID: 10237 RVA: 0x000C3F06 File Offset: 0x000C2106
	private void Start()
	{
		this.myText = base.GetComponent<TextMeshPro>();
		this.textToCopy.AddCallback(new Action<string>(this.OnTextChanged), true);
	}

	// Token: 0x060027FE RID: 10238 RVA: 0x000C3F2C File Offset: 0x000C212C
	private void OnDestroy()
	{
		this.textToCopy.RemoveCallback(new Action<string>(this.OnTextChanged));
	}

	// Token: 0x060027FF RID: 10239 RVA: 0x000C3F45 File Offset: 0x000C2145
	private void OnTextChanged(string newText)
	{
		this.myText.text = newText;
	}

	// Token: 0x04002BE7 RID: 11239
	public WatchableStringSO textToCopy;

	// Token: 0x04002BE8 RID: 11240
	private TextMeshPro myText;
}
