using System;
using GorillaNetworking;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000653 RID: 1619
public class PlayFabTitleDataTextDisplay : MonoBehaviour, IBuildValidation
{
	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x0600281A RID: 10266 RVA: 0x0004B4B0 File Offset: 0x000496B0
	public string playFabKeyValue
	{
		get
		{
			return this.playfabKey;
		}
	}

	// Token: 0x0600281B RID: 10267 RVA: 0x0010FBF4 File Offset: 0x0010DDF4
	private void Start()
	{
		if (this.textBox != null)
		{
			this.textBox.color = this.defaultTextColor;
		}
		else
		{
			Debug.LogError("The TextBox is null on this PlayFabTitleDataTextDisplay component");
		}
		PlayFabTitleDataCache.Instance.OnTitleDataUpdate.AddListener(new UnityAction<string>(this.OnNewTitleDataAdded));
		PlayFabTitleDataCache.Instance.GetTitleData(this.playfabKey, new Action<string>(this.OnTitleDataRequestComplete), new Action<PlayFabError>(this.OnPlayFabError));
	}

	// Token: 0x0600281C RID: 10268 RVA: 0x0004B4B8 File Offset: 0x000496B8
	private void OnPlayFabError(PlayFabError error)
	{
		if (this.textBox != null)
		{
			this.textBox.text = this.fallbackText;
		}
	}

	// Token: 0x0600281D RID: 10269 RVA: 0x0010FC70 File Offset: 0x0010DE70
	private void OnTitleDataRequestComplete(string titleDataResult)
	{
		if (this.textBox != null)
		{
			string text = titleDataResult.Replace("\\r", "\r").Replace("\\n", "\n");
			if (text[0] == '"' && text[text.Length - 1] == '"')
			{
				text = text.Substring(1, text.Length - 2);
			}
			this.textBox.text = text;
		}
	}

	// Token: 0x0600281E RID: 10270 RVA: 0x0004B4D9 File Offset: 0x000496D9
	private void OnNewTitleDataAdded(string key)
	{
		if (key == this.playfabKey && this.textBox != null)
		{
			this.textBox.color = this.newUpdateColor;
		}
	}

	// Token: 0x0600281F RID: 10271 RVA: 0x0004B508 File Offset: 0x00049708
	private void OnDestroy()
	{
		PlayFabTitleDataCache.Instance.OnTitleDataUpdate.RemoveListener(new UnityAction<string>(this.OnNewTitleDataAdded));
	}

	// Token: 0x06002820 RID: 10272 RVA: 0x0004B525 File Offset: 0x00049725
	public bool BuildValidationCheck()
	{
		if (this.textBox == null)
		{
			Debug.LogError("text reference is null! sign text will be broken");
			return false;
		}
		return true;
	}

	// Token: 0x06002821 RID: 10273 RVA: 0x0010FCE4 File Offset: 0x0010DEE4
	public void ChangeTitleDataAtRuntime(string newTitleDataKey)
	{
		this.playfabKey = newTitleDataKey;
		if (this.textBox != null)
		{
			this.textBox.color = this.defaultTextColor;
		}
		else
		{
			Debug.LogError("The TextBox is null on this PlayFabTitleDataTextDisplay component");
		}
		PlayFabTitleDataCache.Instance.OnTitleDataUpdate.AddListener(new UnityAction<string>(this.OnNewTitleDataAdded));
		PlayFabTitleDataCache.Instance.GetTitleData(this.playfabKey, new Action<string>(this.OnTitleDataRequestComplete), new Action<PlayFabError>(this.OnPlayFabError));
	}

	// Token: 0x04002D64 RID: 11620
	[SerializeField]
	private TextMeshPro textBox;

	// Token: 0x04002D65 RID: 11621
	[SerializeField]
	private Color newUpdateColor = Color.magenta;

	// Token: 0x04002D66 RID: 11622
	[SerializeField]
	private Color defaultTextColor = Color.white;

	// Token: 0x04002D67 RID: 11623
	[Tooltip("PlayFab Title Data key from where to pull display text")]
	[SerializeField]
	private string playfabKey;

	// Token: 0x04002D68 RID: 11624
	[Tooltip("Text to display when error occurs during fetch")]
	[TextArea(3, 5)]
	[SerializeField]
	private string fallbackText;
}
