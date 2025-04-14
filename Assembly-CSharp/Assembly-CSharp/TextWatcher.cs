using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200064C RID: 1612
public class TextWatcher : MonoBehaviour
{
	// Token: 0x06002801 RID: 10241 RVA: 0x000C4339 File Offset: 0x000C2539
	private void Start()
	{
		this.myText = base.GetComponent<Text>();
		this.textToCopy.AddCallback(new Action<string>(this.OnTextChanged), true);
	}

	// Token: 0x06002802 RID: 10242 RVA: 0x000C435F File Offset: 0x000C255F
	private void OnDestroy()
	{
		this.textToCopy.RemoveCallback(new Action<string>(this.OnTextChanged));
	}

	// Token: 0x06002803 RID: 10243 RVA: 0x000C4378 File Offset: 0x000C2578
	private void OnTextChanged(string newText)
	{
		this.myText.text = newText;
	}

	// Token: 0x04002BEB RID: 11243
	public WatchableStringSO textToCopy;

	// Token: 0x04002BEC RID: 11244
	private Text myText;
}
