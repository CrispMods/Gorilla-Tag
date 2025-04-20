using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200071D RID: 1821
public class LegalAgreementBodyText : MonoBehaviour
{
	// Token: 0x06002D46 RID: 11590 RVA: 0x0004EC5B File Offset: 0x0004CE5B
	private void Awake()
	{
		this.textCollection.Add(this.textBox);
	}

	// Token: 0x06002D47 RID: 11591 RVA: 0x00127094 File Offset: 0x00125294
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

	// Token: 0x06002D48 RID: 11592 RVA: 0x00127124 File Offset: 0x00125324
	public void ClearText()
	{
		foreach (Text text in this.textCollection)
		{
			text.text = string.Empty;
		}
		this.state = LegalAgreementBodyText.State.Ready;
	}

	// Token: 0x06002D49 RID: 11593 RVA: 0x00127180 File Offset: 0x00125380
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

	// Token: 0x06002D4A RID: 11594 RVA: 0x0004EC6E File Offset: 0x0004CE6E
	private void OnPlayFabError(PlayFabError obj)
	{
		Debug.LogError("ERROR: " + obj.ErrorMessage);
		this.state = LegalAgreementBodyText.State.Error;
	}

	// Token: 0x06002D4B RID: 11595 RVA: 0x0004EC8C File Offset: 0x0004CE8C
	private void OnTitleDataReceived(string text)
	{
		this.cachedText = text;
		this.state = LegalAgreementBodyText.State.Ready;
	}

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x06002D4C RID: 11596 RVA: 0x001271D4 File Offset: 0x001253D4
	public float Height
	{
		get
		{
			return this.rectTransform.rect.height;
		}
	}

	// Token: 0x0400329B RID: 12955
	[SerializeField]
	private Text textBox;

	// Token: 0x0400329C RID: 12956
	[SerializeField]
	private TextAsset textAsset;

	// Token: 0x0400329D RID: 12957
	[SerializeField]
	private RectTransform rectTransform;

	// Token: 0x0400329E RID: 12958
	private List<Text> textCollection = new List<Text>();

	// Token: 0x0400329F RID: 12959
	private string cachedText;

	// Token: 0x040032A0 RID: 12960
	private LegalAgreementBodyText.State state;

	// Token: 0x0200071E RID: 1822
	private enum State
	{
		// Token: 0x040032A2 RID: 12962
		Ready,
		// Token: 0x040032A3 RID: 12963
		Loading,
		// Token: 0x040032A4 RID: 12964
		Error
	}
}
