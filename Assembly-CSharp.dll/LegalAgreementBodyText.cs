using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000709 RID: 1801
public class LegalAgreementBodyText : MonoBehaviour
{
	// Token: 0x06002CB8 RID: 11448 RVA: 0x0004D916 File Offset: 0x0004BB16
	private void Awake()
	{
		this.textCollection.Add(this.textBox);
	}

	// Token: 0x06002CB9 RID: 11449 RVA: 0x001224DC File Offset: 0x001206DC
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
				text2 = UnityEngine.Object.Instantiate<Text>(this.textBox, base.transform);
				this.textCollection.Add(text2);
			}
			else
			{
				text2 = this.textCollection[i];
			}
			text2.text = array[i];
		}
	}

	// Token: 0x06002CBA RID: 11450 RVA: 0x0012256C File Offset: 0x0012076C
	public void ClearText()
	{
		foreach (Text text in this.textCollection)
		{
			text.text = string.Empty;
		}
		this.state = LegalAgreementBodyText.State.Ready;
	}

	// Token: 0x06002CBB RID: 11451 RVA: 0x001225C8 File Offset: 0x001207C8
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

	// Token: 0x06002CBC RID: 11452 RVA: 0x0004D929 File Offset: 0x0004BB29
	private void OnPlayFabError(PlayFabError obj)
	{
		Debug.LogError("ERROR: " + obj.ErrorMessage);
		this.state = LegalAgreementBodyText.State.Error;
	}

	// Token: 0x06002CBD RID: 11453 RVA: 0x0004D947 File Offset: 0x0004BB47
	private void OnTitleDataReceived(string text)
	{
		this.cachedText = text;
		this.state = LegalAgreementBodyText.State.Ready;
	}

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x06002CBE RID: 11454 RVA: 0x0012261C File Offset: 0x0012081C
	public float Height
	{
		get
		{
			return this.rectTransform.rect.height;
		}
	}

	// Token: 0x04003204 RID: 12804
	[SerializeField]
	private Text textBox;

	// Token: 0x04003205 RID: 12805
	[SerializeField]
	private TextAsset textAsset;

	// Token: 0x04003206 RID: 12806
	[SerializeField]
	private RectTransform rectTransform;

	// Token: 0x04003207 RID: 12807
	private List<Text> textCollection = new List<Text>();

	// Token: 0x04003208 RID: 12808
	private string cachedText;

	// Token: 0x04003209 RID: 12809
	private LegalAgreementBodyText.State state;

	// Token: 0x0200070A RID: 1802
	private enum State
	{
		// Token: 0x0400320B RID: 12811
		Ready,
		// Token: 0x0400320C RID: 12812
		Loading,
		// Token: 0x0400320D RID: 12813
		Error
	}
}
