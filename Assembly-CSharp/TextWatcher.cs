using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200062A RID: 1578
public class TextWatcher : MonoBehaviour
{
	// Token: 0x06002724 RID: 10020 RVA: 0x0004AB89 File Offset: 0x00048D89
	private void Start()
	{
		this.myText = base.GetComponent<Text>();
		this.textToCopy.AddCallback(new Action<string>(this.OnTextChanged), true);
	}

	// Token: 0x06002725 RID: 10021 RVA: 0x0004ABAF File Offset: 0x00048DAF
	private void OnDestroy()
	{
		this.textToCopy.RemoveCallback(new Action<string>(this.OnTextChanged));
	}

	// Token: 0x06002726 RID: 10022 RVA: 0x0004ABC8 File Offset: 0x00048DC8
	private void OnTextChanged(string newText)
	{
		this.myText.text = newText;
	}

	// Token: 0x04002B4B RID: 11083
	public WatchableStringSO textToCopy;

	// Token: 0x04002B4C RID: 11084
	private Text myText;
}
