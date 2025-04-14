using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000708 RID: 1800
public class LegalAgreementBodyText : MonoBehaviour
{
	// Token: 0x06002CB0 RID: 11440 RVA: 0x000DC946 File Offset: 0x000DAB46
	private void Awake()
	{
		this.textCollection.Add(this.textBox);
	}

	// Token: 0x06002CB1 RID: 11441 RVA: 0x000DC95C File Offset: 0x000DAB5C
	public void SetText(string text)
	{
		text = Regex.Unescape(text);
		string[] array = text.Split(new string[]
		{
			Environment.NewLine,
			"\\r\\n",
			"\n"
		}, StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			Text text2;
			if (i >= this.textCollection.Count)
			{
				text2 = Object.Instantiate<Text>(this.textBox, base.transform);
				this.textCollection.Add(text2);
			}
			else
			{
				text2 = this.textCollection[i];
			}
			text2.text = array[i];
		}
	}

	// Token: 0x06002CB2 RID: 11442 RVA: 0x000DC9EC File Offset: 0x000DABEC
	public void ClearText()
	{
		foreach (Text text in this.textCollection)
		{
			text.text = string.Empty;
		}
		this.state = LegalAgreementBodyText.State.Ready;
	}

	// Token: 0x06002CB3 RID: 11443 RVA: 0x000DCA48 File Offset: 0x000DAC48
	public Task<bool> UpdateTextFromPlayFabTitleData(string key, string version)
	{
		LegalAgreementBodyText.<UpdateTextFromPlayFabTitleData>d__10 <UpdateTextFromPlayFabTitleData>d__;
		<UpdateTextFromPlayFabTitleData>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<UpdateTextFromPlayFabTitleData>d__.<>4__this = this;
		<UpdateTextFromPlayFabTitleData>d__.key = key;
		<UpdateTextFromPlayFabTitleData>d__.version = version;
		<UpdateTextFromPlayFabTitleData>d__.<>1__state = -1;
		<UpdateTextFromPlayFabTitleData>d__.<>t__builder.Start<LegalAgreementBodyText.<UpdateTextFromPlayFabTitleData>d__10>(ref <UpdateTextFromPlayFabTitleData>d__);
		return <UpdateTextFromPlayFabTitleData>d__.<>t__builder.Task;
	}

	// Token: 0x06002CB4 RID: 11444 RVA: 0x000DCA9B File Offset: 0x000DAC9B
	private void OnPlayFabError(PlayFabError obj)
	{
		Debug.LogError("ERROR: " + obj.ErrorMessage);
		this.state = LegalAgreementBodyText.State.Error;
	}

	// Token: 0x06002CB5 RID: 11445 RVA: 0x000DCAB9 File Offset: 0x000DACB9
	private void OnTitleDataReceived(string text)
	{
		this.cachedText = text;
		this.state = LegalAgreementBodyText.State.Ready;
	}

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06002CB6 RID: 11446 RVA: 0x000DCACC File Offset: 0x000DACCC
	public float Height
	{
		get
		{
			return this.rectTransform.rect.height;
		}
	}

	// Token: 0x040031FE RID: 12798
	[SerializeField]
	private Text textBox;

	// Token: 0x040031FF RID: 12799
	[SerializeField]
	private TextAsset textAsset;

	// Token: 0x04003200 RID: 12800
	[SerializeField]
	private RectTransform rectTransform;

	// Token: 0x04003201 RID: 12801
	private List<Text> textCollection = new List<Text>();

	// Token: 0x04003202 RID: 12802
	private string cachedText;

	// Token: 0x04003203 RID: 12803
	private LegalAgreementBodyText.State state;

	// Token: 0x02000709 RID: 1801
	private enum State
	{
		// Token: 0x04003205 RID: 12805
		Ready,
		// Token: 0x04003206 RID: 12806
		Loading,
		// Token: 0x04003207 RID: 12807
		Error
	}
}
