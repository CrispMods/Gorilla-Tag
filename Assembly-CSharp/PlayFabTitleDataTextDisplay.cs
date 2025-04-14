using System;
using GorillaNetworking;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000689 RID: 1673
public class PlayFabTitleDataTextDisplay : MonoBehaviour, IBuildValidation
{
	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x0600299B RID: 10651 RVA: 0x000CE876 File Offset: 0x000CCA76
	public string playFabKeyValue
	{
		get
		{
			return this.playfabKey;
		}
	}

	// Token: 0x0600299C RID: 10652 RVA: 0x000CE880 File Offset: 0x000CCA80
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

	// Token: 0x0600299D RID: 10653 RVA: 0x000CE8FB File Offset: 0x000CCAFB
	private void OnPlayFabError(PlayFabError error)
	{
		if (this.textBox != null)
		{
			this.textBox.text = this.fallbackText;
		}
	}

	// Token: 0x0600299E RID: 10654 RVA: 0x000CE91C File Offset: 0x000CCB1C
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

	// Token: 0x0600299F RID: 10655 RVA: 0x000CE990 File Offset: 0x000CCB90
	private void OnNewTitleDataAdded(string key)
	{
		if (key == this.playfabKey && this.textBox != null)
		{
			this.textBox.color = this.newUpdateColor;
		}
	}

	// Token: 0x060029A0 RID: 10656 RVA: 0x000CE9BF File Offset: 0x000CCBBF
	private void OnDestroy()
	{
		PlayFabTitleDataCache.Instance.OnTitleDataUpdate.RemoveListener(new UnityAction<string>(this.OnNewTitleDataAdded));
	}

	// Token: 0x060029A1 RID: 10657 RVA: 0x000CE9DC File Offset: 0x000CCBDC
	public bool BuildValidationCheck()
	{
		if (this.textBox == null)
		{
			Debug.LogError("text reference is null! sign text will be broken");
			return false;
		}
		return true;
	}

	// Token: 0x060029A2 RID: 10658 RVA: 0x000CE9FC File Offset: 0x000CCBFC
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

	// Token: 0x04002EF4 RID: 12020
	[SerializeField]
	private TextMeshPro textBox;

	// Token: 0x04002EF5 RID: 12021
	[SerializeField]
	private Color newUpdateColor = Color.magenta;

	// Token: 0x04002EF6 RID: 12022
	[SerializeField]
	private Color defaultTextColor = Color.white;

	// Token: 0x04002EF7 RID: 12023
	[Tooltip("PlayFab Title Data key from where to pull display text")]
	[SerializeField]
	private string playfabKey;

	// Token: 0x04002EF8 RID: 12024
	[Tooltip("Text to display when error occurs during fetch")]
	[TextArea(3, 5)]
	[SerializeField]
	private string fallbackText;
}
