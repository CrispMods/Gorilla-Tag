using System;
using TMPro;
using UnityEngine;

// Token: 0x0200064D RID: 1613
public class TextWatcherTMPro : MonoBehaviour
{
	// Token: 0x06002805 RID: 10245 RVA: 0x0004A641 File Offset: 0x00048841
	private void Start()
	{
		this.myText = base.GetComponent<TextMeshPro>();
		this.textToCopy.AddCallback(new Action<string>(this.OnTextChanged), true);
	}

	// Token: 0x06002806 RID: 10246 RVA: 0x0004A667 File Offset: 0x00048867
	private void OnDestroy()
	{
		this.textToCopy.RemoveCallback(new Action<string>(this.OnTextChanged));
	}

	// Token: 0x06002807 RID: 10247 RVA: 0x0004A680 File Offset: 0x00048880
	private void OnTextChanged(string newText)
	{
		this.myText.text = newText;
	}

	// Token: 0x04002BED RID: 11245
	public WatchableStringSO textToCopy;

	// Token: 0x04002BEE RID: 11246
	private TextMeshPro myText;
}
