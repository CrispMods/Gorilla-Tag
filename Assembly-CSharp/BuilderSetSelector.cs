using System;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020004F5 RID: 1269
public class BuilderSetSelector : MonoBehaviour
{
	// Token: 0x06001EC5 RID: 7877 RVA: 0x0009B390 File Offset: 0x00099590
	private void Start()
	{
		this.zoneRenderers.Clear();
		foreach (GorillaPressableButton gorillaPressableButton in this.setButtons)
		{
			this.zoneRenderers.Add(gorillaPressableButton.buttonRenderer);
			TMP_Text myTmpText = gorillaPressableButton.myTmpText;
			Renderer renderer = (myTmpText != null) ? myTmpText.GetComponent<Renderer>() : null;
			if (renderer != null)
			{
				this.zoneRenderers.Add(renderer);
			}
		}
		this.zoneRenderers.Add(this.previousPageButton.buttonRenderer);
		this.zoneRenderers.Add(this.nextPageButton.buttonRenderer);
		TMP_Text myTmpText2 = this.previousPageButton.myTmpText;
		Renderer renderer2 = (myTmpText2 != null) ? myTmpText2.GetComponent<Renderer>() : null;
		if (renderer2 != null)
		{
			this.zoneRenderers.Add(renderer2);
		}
		TMP_Text myTmpText3 = this.nextPageButton.myTmpText;
		renderer2 = ((myTmpText3 != null) ? myTmpText3.GetComponent<Renderer>() : null);
		if (renderer2 != null)
		{
			this.zoneRenderers.Add(renderer2);
		}
		this.inBuilderZone = true;
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
		this.OnZoneChanged();
	}

	// Token: 0x06001EC6 RID: 7878 RVA: 0x0009B4B8 File Offset: 0x000996B8
	public void Setup(List<BuilderPieceSet.BuilderPieceCategory> categories)
	{
		List<BuilderPieceSet> livePieceSets = BuilderSetManager.instance.GetLivePieceSets();
		this.numLivePieceSets = livePieceSets.Count;
		this.pieceSets = new List<BuilderPieceSet>(livePieceSets.Count);
		this._includedCategories = categories;
		foreach (BuilderPieceSet builderPieceSet in livePieceSets)
		{
			if (!builderPieceSet.setName.Equals("HIDDEN") && this.DoesSetHaveIncludedCategories(builderPieceSet))
			{
				this.pieceSets.Add(builderPieceSet);
			}
		}
		BuilderSetManager.instance.OnOwnedSetsUpdated.AddListener(new UnityAction(this.RefreshUnlockedSets));
		BuilderSetManager.instance.OnLiveSetsUpdated.AddListener(new UnityAction(this.RefreshUnlockedSets));
		this.setsPerPage = this.setButtons.Length;
		this.totalPages = this.pieceSets.Count / this.setsPerPage;
		if (this.pieceSets.Count % this.setsPerPage > 0)
		{
			this.totalPages++;
		}
		this.previousPageButton.gameObject.SetActive(this.totalPages > 1);
		this.nextPageButton.gameObject.SetActive(this.totalPages > 1);
		this.previousPageButton.myTmpText.enabled = (this.totalPages > 1);
		this.nextPageButton.myTmpText.enabled = (this.totalPages > 1);
		this.pageIndex = 0;
		this.currentSet = this.pieceSets[this.setIndex];
		this.previousPageButton.onPressButton.AddListener(new UnityAction(this.OnPreviousPageClicked));
		this.nextPageButton.onPressButton.AddListener(new UnityAction(this.OnNextPageClicked));
		GorillaPressableButton[] array = this.setButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].onPressed += this.OnSetButtonPressed;
		}
		this.UpdateLabels();
	}

	// Token: 0x06001EC7 RID: 7879 RVA: 0x0009B6C8 File Offset: 0x000998C8
	private void OnDestroy()
	{
		if (this.previousPageButton != null)
		{
			this.previousPageButton.onPressButton.RemoveListener(new UnityAction(this.OnPreviousPageClicked));
		}
		if (this.nextPageButton != null)
		{
			this.nextPageButton.onPressButton.RemoveListener(new UnityAction(this.OnNextPageClicked));
		}
		if (BuilderSetManager.instance != null)
		{
			BuilderSetManager.instance.OnOwnedSetsUpdated.RemoveListener(new UnityAction(this.RefreshUnlockedSets));
			BuilderSetManager.instance.OnLiveSetsUpdated.RemoveListener(new UnityAction(this.RefreshUnlockedSets));
		}
		foreach (GorillaPressableButton gorillaPressableButton in this.setButtons)
		{
			if (!(gorillaPressableButton == null))
			{
				gorillaPressableButton.onPressed -= this.OnSetButtonPressed;
			}
		}
		if (ZoneManagement.instance != null)
		{
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
		}
	}

	// Token: 0x06001EC8 RID: 7880 RVA: 0x0009B7DC File Offset: 0x000999DC
	private void OnZoneChanged()
	{
		bool flag = ZoneManagement.instance.IsZoneActive(GTZone.monkeBlocks);
		if (flag && !this.inBuilderZone)
		{
			using (List<Renderer>.Enumerator enumerator = this.zoneRenderers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Renderer renderer = enumerator.Current;
					renderer.enabled = true;
				}
				goto IL_8B;
			}
		}
		if (!flag && this.inBuilderZone)
		{
			foreach (Renderer renderer2 in this.zoneRenderers)
			{
				renderer2.enabled = false;
			}
		}
		IL_8B:
		this.inBuilderZone = flag;
	}

	// Token: 0x06001EC9 RID: 7881 RVA: 0x0009B898 File Offset: 0x00099A98
	private void OnSetButtonPressed(GorillaPressableButton button, bool isLeft)
	{
		int num = 0;
		for (int i = 0; i < this.setButtons.Length; i++)
		{
			if (button.Equals(this.setButtons[i]))
			{
				num = i;
				break;
			}
		}
		int num2 = this.pageIndex * this.setsPerPage + num;
		if (num2 < this.pieceSets.Count)
		{
			BuilderPieceSet builderPieceSet = this.pieceSets[num2];
			if (this.currentSet == null || builderPieceSet.setName != this.currentSet.setName)
			{
				UnityEvent<int> onSelectedSet = this.OnSelectedSet;
				if (onSelectedSet == null)
				{
					return;
				}
				onSelectedSet.Invoke(builderPieceSet.GetIntIdentifier());
			}
		}
	}

	// Token: 0x06001ECA RID: 7882 RVA: 0x0009B938 File Offset: 0x00099B38
	private void RefreshUnlockedSets()
	{
		List<BuilderPieceSet> livePieceSets = BuilderSetManager.instance.GetLivePieceSets();
		if (livePieceSets.Count != this.numLivePieceSets)
		{
			string value = (this.currentSet != null) ? this.currentSet.setName : "";
			this.numLivePieceSets = livePieceSets.Count;
			this.pieceSets.EnsureCapacity(this.numLivePieceSets);
			this.pieceSets.Clear();
			int num = 0;
			foreach (BuilderPieceSet builderPieceSet in livePieceSets)
			{
				if (!builderPieceSet.setName.Equals("HIDDEN") && this.DoesSetHaveIncludedCategories(builderPieceSet))
				{
					if (builderPieceSet.setName.Equals(value))
					{
						num = this.pieceSets.Count;
					}
					this.pieceSets.Add(builderPieceSet);
				}
			}
			if (this.pieceSets.Count < 1)
			{
				this.currentSet = null;
			}
			else
			{
				this.setIndex = num;
				this.currentSet = this.pieceSets[this.setIndex];
			}
			this.totalPages = this.pieceSets.Count / this.setsPerPage;
			if (this.pieceSets.Count % this.setsPerPage > 0)
			{
				this.totalPages++;
			}
			this.previousPageButton.gameObject.SetActive(this.totalPages > 1);
			this.nextPageButton.gameObject.SetActive(this.totalPages > 1);
			this.previousPageButton.myTmpText.enabled = (this.totalPages > 1);
			this.nextPageButton.myTmpText.enabled = (this.totalPages > 1);
		}
		this.UpdateLabels();
	}

	// Token: 0x06001ECB RID: 7883 RVA: 0x0009BB08 File Offset: 0x00099D08
	private void OnPreviousPageClicked()
	{
		this.RefreshUnlockedSets();
		int num = Mathf.Clamp(this.pageIndex - 1, 0, this.totalPages - 1);
		if (num != this.pageIndex)
		{
			this.pageIndex = num;
			this.UpdateLabels();
		}
	}

	// Token: 0x06001ECC RID: 7884 RVA: 0x0009BB48 File Offset: 0x00099D48
	private void OnNextPageClicked()
	{
		this.RefreshUnlockedSets();
		int num = Mathf.Clamp(this.pageIndex + 1, 0, this.totalPages - 1);
		if (num != this.pageIndex)
		{
			this.pageIndex = num;
			this.UpdateLabels();
		}
	}

	// Token: 0x06001ECD RID: 7885 RVA: 0x0009BB88 File Offset: 0x00099D88
	public void SetSelection(int setID)
	{
		if (BuilderSetManager.instance == null)
		{
			return;
		}
		BuilderPieceSet pieceSetFromID = BuilderSetManager.instance.GetPieceSetFromID(setID);
		if (pieceSetFromID == null)
		{
			return;
		}
		this.currentSet = pieceSetFromID;
		this.UpdateLabels();
	}

	// Token: 0x06001ECE RID: 7886 RVA: 0x0009BBCC File Offset: 0x00099DCC
	private void UpdateLabels()
	{
		for (int i = 0; i < this.setLabels.Length; i++)
		{
			int num = this.pageIndex * this.setsPerPage + i;
			if (num < this.pieceSets.Count && this.pieceSets[num] != null)
			{
				if (!this.setButtons[i].gameObject.activeSelf)
				{
					this.setButtons[i].gameObject.SetActive(true);
					this.setButtons[i].myTmpText.gameObject.SetActive(true);
				}
				if (this.setButtons[i].myTmpText.text != this.pieceSets[num].setName.ToUpper())
				{
					this.setButtons[i].myTmpText.text = this.pieceSets[num].setName.ToUpper();
				}
				if (BuilderSetManager.instance.IsPieceSetOwnedLocally(this.pieceSets[num].GetIntIdentifier()))
				{
					bool flag = this.currentSet != null && this.pieceSets[num].setName == this.currentSet.setName;
					if (flag != this.setButtons[i].isOn || !this.setButtons[i].enabled)
					{
						this.setButtons[i].isOn = flag;
						this.setButtons[i].buttonRenderer.material = (flag ? this.setButtons[i].pressedMaterial : this.setButtons[i].unpressedMaterial);
					}
					this.setButtons[i].enabled = true;
				}
				else
				{
					if (this.setButtons[i].enabled)
					{
						this.setButtons[i].buttonRenderer.material = this.disabledMaterial;
					}
					this.setButtons[i].enabled = false;
				}
			}
			else
			{
				if (this.setButtons[i].gameObject.activeSelf)
				{
					this.setButtons[i].gameObject.SetActive(false);
					this.setButtons[i].myTmpText.gameObject.SetActive(false);
				}
				if (this.setButtons[i].isOn || this.setButtons[i].enabled)
				{
					this.setButtons[i].isOn = false;
					this.setButtons[i].enabled = false;
				}
			}
		}
		bool flag2 = this.pageIndex > 0 && this.totalPages > 1;
		bool flag3 = this.pageIndex < this.totalPages - 1 && this.totalPages > 1;
		if (this.previousPageButton.myTmpText.enabled != flag2)
		{
			this.previousPageButton.myTmpText.enabled = flag2;
		}
		if (this.nextPageButton.myTmpText.enabled != flag3)
		{
			this.nextPageButton.myTmpText.enabled = flag3;
		}
	}

	// Token: 0x06001ECF RID: 7887 RVA: 0x0009BEB8 File Offset: 0x0009A0B8
	public bool DoesSetHaveIncludedCategories(BuilderPieceSet set)
	{
		foreach (BuilderPieceSet.BuilderPieceSubset builderPieceSubset in set.subsets)
		{
			if (this._includedCategories.Contains(builderPieceSubset.pieceCategory))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001ED0 RID: 7888 RVA: 0x0009BF20 File Offset: 0x0009A120
	public BuilderPieceSet GetSelectedSet()
	{
		return this.currentSet;
	}

	// Token: 0x06001ED1 RID: 7889 RVA: 0x0009BF28 File Offset: 0x0009A128
	public int GetDefaultSetID()
	{
		if (this.pieceSets == null || this.pieceSets.Count < 1)
		{
			return -1;
		}
		BuilderPieceSet builderPieceSet = this.pieceSets[0];
		if (!BuilderSetManager.instance.IsPieceSetOwnedLocally(builderPieceSet.GetIntIdentifier()))
		{
			foreach (BuilderPieceSet builderPieceSet2 in this.pieceSets)
			{
				if (BuilderSetManager.instance.IsPieceSetOwnedLocally(builderPieceSet2.GetIntIdentifier()))
				{
					return builderPieceSet2.GetIntIdentifier();
				}
			}
			Debug.LogWarning("No default set available for shelf");
			return -1;
		}
		return builderPieceSet.GetIntIdentifier();
	}

	// Token: 0x0400226D RID: 8813
	private List<BuilderPieceSet> pieceSets;

	// Token: 0x0400226E RID: 8814
	private int numLivePieceSets;

	// Token: 0x0400226F RID: 8815
	[SerializeField]
	private Material disabledMaterial;

	// Token: 0x04002270 RID: 8816
	[Header("UI")]
	[SerializeField]
	private Text[] setLabels;

	// Token: 0x04002271 RID: 8817
	[Header("Buttons")]
	[SerializeField]
	private GorillaPressableButton[] setButtons;

	// Token: 0x04002272 RID: 8818
	[SerializeField]
	private GorillaPressableButton previousPageButton;

	// Token: 0x04002273 RID: 8819
	[SerializeField]
	private GorillaPressableButton nextPageButton;

	// Token: 0x04002274 RID: 8820
	private List<BuilderPieceSet.BuilderPieceCategory> _includedCategories;

	// Token: 0x04002275 RID: 8821
	private int setIndex;

	// Token: 0x04002276 RID: 8822
	private BuilderPieceSet currentSet;

	// Token: 0x04002277 RID: 8823
	private int pageIndex;

	// Token: 0x04002278 RID: 8824
	private int setsPerPage = 3;

	// Token: 0x04002279 RID: 8825
	private int totalPages = 1;

	// Token: 0x0400227A RID: 8826
	private List<Renderer> zoneRenderers = new List<Renderer>(10);

	// Token: 0x0400227B RID: 8827
	private bool inBuilderZone;

	// Token: 0x0400227C RID: 8828
	[HideInInspector]
	public UnityEvent<int> OnSelectedSet;
}
