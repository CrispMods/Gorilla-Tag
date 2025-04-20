using System;
using TMPro;
using UnityEngine;

// Token: 0x0200062B RID: 1579
public class TextWatcherTMPro : MonoBehaviour
{
	// Token: 0x06002728 RID: 10024 RVA: 0x0004ABD6 File Offset: 0x00048DD6
	private void Start()
	{
		this.myText = base.GetComponent<TextMeshPro>();
		this.textToCopy.AddCallback(new Action<string>(this.OnTextChanged), true);
	}

	// Token: 0x06002729 RID: 10025 RVA: 0x0004ABFC File Offset: 0x00048DFC
	private void OnDestroy()
	{
		this.textToCopy.RemoveCallback(new Action<string>(this.OnTextChanged));
	}

	// Token: 0x0600272A RID: 10026 RVA: 0x0004AC15 File Offset: 0x00048E15
	private void OnTextChanged(string newText)
	{
		this.myText.text = newText;
	}

	// Token: 0x04002B4D RID: 11085
	public WatchableStringSO textToCopy;

	// Token: 0x04002B4E RID: 11086
	private TextMeshPro myText;
}
