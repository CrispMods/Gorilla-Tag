using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200064B RID: 1611
public class TextWatcher : MonoBehaviour
{
	// Token: 0x060027F9 RID: 10233 RVA: 0x000C3EB9 File Offset: 0x000C20B9
	private void Start()
	{
		this.myText = base.GetComponent<Text>();
		this.textToCopy.AddCallback(new Action<string>(this.OnTextChanged), true);
	}

	// Token: 0x060027FA RID: 10234 RVA: 0x000C3EDF File Offset: 0x000C20DF
	private void OnDestroy()
	{
		this.textToCopy.RemoveCallback(new Action<string>(this.OnTextChanged));
	}

	// Token: 0x060027FB RID: 10235 RVA: 0x000C3EF8 File Offset: 0x000C20F8
	private void OnTextChanged(string newText)
	{
		this.myText.text = newText;
	}

	// Token: 0x04002BE5 RID: 11237
	public WatchableStringSO textToCopy;

	// Token: 0x04002BE6 RID: 11238
	private Text myText;
}
