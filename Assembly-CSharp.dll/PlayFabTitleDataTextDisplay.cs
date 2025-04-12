using System;
using GorillaNetworking;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200068A RID: 1674
public class PlayFabTitleDataTextDisplay : MonoBehaviour, IBuildValidation
{
	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x060029A3 RID: 10659 RVA: 0x0004B5AB File Offset: 0x000497AB
	public string playFabKeyValue
	{
		get
		{
			return this.playfabKey;
		}
	}

	// Token: 0x060029A4 RID: 10660 RVA: 0x001167EC File Offset: 0x001149EC
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

	// Token: 0x060029A5 RID: 10661 RVA: 0x0004B5B3 File Offset: 0x000497B3
	private void OnPlayFabError(PlayFabError error)
	{
		if (this.textBox != null)
		{
			this.textBox.text = this.fallbackText;
		}
	}

	// Token: 0x060029A6 RID: 10662 RVA: 0x00116868 File Offset: 0x00114A68
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

	// Token: 0x060029A7 RID: 10663 RVA: 0x0004B5D4 File Offset: 0x000497D4
	private void OnNewTitleDataAdded(string key)
	{
		if (key == this.playfabKey && this.textBox != null)
		{
			this.textBox.color = this.newUpdateColor;
		}
	}

	// Token: 0x060029A8 RID: 10664 RVA: 0x0004B603 File Offset: 0x00049803
	private void OnDestroy()
	{
		PlayFabTitleDataCache.Instance.OnTitleDataUpdate.RemoveListener(new UnityAction<string>(this.OnNewTitleDataAdded));
	}

	// Token: 0x060029A9 RID: 10665 RVA: 0x0004B620 File Offset: 0x00049820
	public bool BuildValidationCheck()
	{
		if (this.textBox == null)
		{
			Debug.LogError("text reference is null! sign text will be broken");
			return false;
		}
		return true;
	}

	// Token: 0x060029AA RID: 10666 RVA: 0x001168DC File Offset: 0x00114ADC
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

	// Token: 0x04002EFA RID: 12026
	[SerializeField]
	private TextMeshPro textBox;

	// Token: 0x04002EFB RID: 12027
	[SerializeField]
	private Color newUpdateColor = Color.magenta;

	// Token: 0x04002EFC RID: 12028
	[SerializeField]
	private Color defaultTextColor = Color.white;

	// Token: 0x04002EFD RID: 12029
	[Tooltip("PlayFab Title Data key from where to pull display text")]
	[SerializeField]
	private string playfabKey;

	// Token: 0x04002EFE RID: 12030
	[Tooltip("Text to display when error occurs during fetch")]
	[TextArea(3, 5)]
	[SerializeField]
	private string fallbackText;
}
