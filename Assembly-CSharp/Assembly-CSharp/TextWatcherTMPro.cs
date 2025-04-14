using System;
using TMPro;
using UnityEngine;

// Token: 0x0200064D RID: 1613
public class TextWatcherTMPro : MonoBehaviour
{
	// Token: 0x06002805 RID: 10245 RVA: 0x000C4386 File Offset: 0x000C2586
	private void Start()
	{
		this.myText = base.GetComponent<TextMeshPro>();
		this.textToCopy.AddCallback(new Action<string>(this.OnTextChanged), true);
	}

	// Token: 0x06002806 RID: 10246 RVA: 0x000C43AC File Offset: 0x000C25AC
	private void OnDestroy()
	{
		this.textToCopy.RemoveCallback(new Action<string>(this.OnTextChanged));
	}

	// Token: 0x06002807 RID: 10247 RVA: 0x000C43C5 File Offset: 0x000C25C5
	private void OnTextChanged(string newText)
	{
		this.myText.text = newText;
	}

	// Token: 0x04002BED RID: 11245
	public WatchableStringSO textToCopy;

	// Token: 0x04002BEE RID: 11246
	private TextMeshPro myText;
}
