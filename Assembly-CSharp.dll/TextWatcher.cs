using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200064C RID: 1612
public class TextWatcher : MonoBehaviour
{
	// Token: 0x06002801 RID: 10241 RVA: 0x0004A5F4 File Offset: 0x000487F4
	private void Start()
	{
		this.myText = base.GetComponent<Text>();
		this.textToCopy.AddCallback(new Action<string>(this.OnTextChanged), true);
	}

	// Token: 0x06002802 RID: 10242 RVA: 0x0004A61A File Offset: 0x0004881A
	private void OnDestroy()
	{
		this.textToCopy.RemoveCallback(new Action<string>(this.OnTextChanged));
	}

	// Token: 0x06002803 RID: 10243 RVA: 0x0004A633 File Offset: 0x00048833
	private void OnTextChanged(string newText)
	{
		this.myText.text = newText;
	}

	// Token: 0x04002BEB RID: 11243
	public WatchableStringSO textToCopy;

	// Token: 0x04002BEC RID: 11244
	private Text myText;
}
