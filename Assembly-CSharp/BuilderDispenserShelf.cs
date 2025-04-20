using System;
using System.Collections.Generic;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004CA RID: 1226
public class BuilderDispenserShelf : MonoBehaviour
{
	// Token: 0x06001DC1 RID: 7617 RVA: 0x00044560 File Offset: 0x00042760
	private void BuildDispenserPool()
	{
		this.dispenserPool = new List<BuilderDispenser>(12);
		this.activeDispensers = new List<BuilderDispenser>(6);
		this.AddToDispenserPool(6);
	}

	// Token: 0x06001DC2 RID: 7618 RVA: 0x000E27DC File Offset: 0x000E09DC
	private void AddToDispenserPool(int count)
	{
		if (this.dispenserPrefab == null)
		{
			return;
		}
		for (int i = 0; i < count; i++)
		{
			BuilderDispenser builderDispenser = UnityEngine.Object.Instantiate<BuilderDispenser>(this.dispenserPrefab, this.shelfCenter);
			builderDispenser.gameObject.SetActive(false);
			builderDispenser.table = this.table;
			builderDispenser.shelfID = this.shelfID;
			this.dispenserPool.Add(builderDispenser);
		}
	}

	// Token: 0x06001DC3 RID: 7619 RVA: 0x000E2848 File Offset: 0x000E0A48
	private void ActivateDispensers()
	{
		this.piecesInSet.Clear();
		foreach (BuilderPieceSet.BuilderPieceSubset builderPieceSubset in this.currentSet.subsets)
		{
			if (this._includedCategories.Contains(builderPieceSubset.pieceCategory))
			{
				this.piecesInSet.AddRange(builderPieceSubset.pieceInfos);
			}
		}
		if (this.piecesInSet.Count <= 0)
		{
			return;
		}
		int count = this.piecesInSet.Count;
		if (this.dispenserPool.Count < count)
		{
			this.AddToDispenserPool(count - this.dispenserPool.Count);
		}
		this.activeDispensers.Clear();
		for (int i = 0; i < this.dispenserPool.Count; i++)
		{
			if (i < count)
			{
				BuilderDispenser builderDispenser = this.dispenserPool[i];
				builderDispenser.gameObject.SetActive(true);
				float x = this.shelfWidth / -2f + this.shelfWidth / (float)(count * 2) + this.shelfWidth / (float)count * (float)i;
				builderDispenser.transform.localPosition = new Vector3(x, 0f, 0f);
				builderDispenser.AssignPieceType(this.piecesInSet[i], this.currentSet.materialId.GetHashCode());
				this.activeDispensers.Add(builderDispenser);
			}
			else
			{
				this.dispenserPool[i].ClearDispenser();
				this.dispenserPool[i].gameObject.SetActive(false);
			}
		}
		this.dispenserToUpdate = 0;
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x000E29F4 File Offset: 0x000E0BF4
	public void Setup()
	{
		this.InitIfNeeded();
		foreach (BuilderDispenser builderDispenser in this.dispenserPool)
		{
			builderDispenser.table = this.table;
			builderDispenser.shelfID = this.shelfID;
		}
	}

	// Token: 0x06001DC5 RID: 7621 RVA: 0x000E2A5C File Offset: 0x000E0C5C
	private void InitIfNeeded()
	{
		if (this.initialized)
		{
			return;
		}
		this.setSelector.Setup(this._includedCategories);
		this.currentSet = this.setSelector.GetSelectedSet();
		this.setSelector.OnSelectedSet.AddListener(new UnityAction<int>(this.OnSelectedSetChange));
		this.BuildDispenserPool();
		this.ActivateDispensers();
		this.initialized = true;
	}

	// Token: 0x06001DC6 RID: 7622 RVA: 0x00044582 File Offset: 0x00042782
	private void OnDestroy()
	{
		if (this.setSelector != null)
		{
			this.setSelector.OnSelectedSet.RemoveListener(new UnityAction<int>(this.OnSelectedSetChange));
		}
	}

	// Token: 0x06001DC7 RID: 7623 RVA: 0x000445AE File Offset: 0x000427AE
	public void OnSelectedSetChange(int setId)
	{
		if (this.table.GetTableState() != BuilderTable.TableState.Ready)
		{
			return;
		}
		this.table.RequestShelfSelection(this.shelfID, setId, false);
	}

	// Token: 0x06001DC8 RID: 7624 RVA: 0x000E2AC4 File Offset: 0x000E0CC4
	public void SetSelection(int setId)
	{
		this.setSelector.SetSelection(setId);
		BuilderPieceSet selectedSet = this.setSelector.GetSelectedSet();
		if ((this.initialized && this.currentSet == null) || selectedSet.setName != this.currentSet.setName)
		{
			this.currentSet = selectedSet;
			if (this.table.GetTableState() == BuilderTable.TableState.Ready)
			{
				if (!this.animatingShelf)
				{
					this.StartShelfSwap();
					return;
				}
			}
			else
			{
				this.animatingShelf = false;
				this.ImmediateShelfSwap();
			}
		}
	}

	// Token: 0x06001DC9 RID: 7625 RVA: 0x000445D2 File Offset: 0x000427D2
	public int GetSelectedSetID()
	{
		return this.setSelector.GetSelectedSet().GetIntIdentifier();
	}

	// Token: 0x06001DCA RID: 7626 RVA: 0x000E2B48 File Offset: 0x000E0D48
	private void ImmediateShelfSwap()
	{
		foreach (BuilderDispenser builderDispenser in this.activeDispensers)
		{
			builderDispenser.ClearDispenser();
		}
		this.ActivateDispensers();
	}

	// Token: 0x06001DCB RID: 7627 RVA: 0x000E2BA0 File Offset: 0x000E0DA0
	private void StartShelfSwap()
	{
		this.dispenserToClear = 0;
		this.timeToClearShelf = (double)(Time.time + 0.15f);
		this.resetAnimation.Rewind();
		foreach (BuilderDispenser builderDispenser in this.activeDispensers)
		{
			builderDispenser.ParentPieceToShelf(this.resetAnimation.transform);
		}
		this.resetAnimation.Play();
		this.animatingShelf = true;
	}

	// Token: 0x06001DCC RID: 7628 RVA: 0x000E2C34 File Offset: 0x000E0E34
	public void UpdateShelf()
	{
		if (this.animatingShelf && (double)Time.time > this.timeToClearShelf)
		{
			if (this.dispenserToClear < this.activeDispensers.Count)
			{
				if (this.dispenserToClear == 0)
				{
					this.resetSoundBank.Play();
				}
				this.activeDispensers[this.dispenserToClear].ClearDispenser();
				this.dispenserToClear++;
				return;
			}
			if (!this.resetAnimation.isPlaying)
			{
				this.playSpawnSetSound = true;
				this.ActivateDispensers();
				this.animatingShelf = false;
			}
		}
	}

	// Token: 0x06001DCD RID: 7629 RVA: 0x000E2CC4 File Offset: 0x000E0EC4
	public void UpdateShelfSliced()
	{
		if (!PhotonNetwork.LocalPlayer.IsMasterClient)
		{
			return;
		}
		if (!this.initialized)
		{
			return;
		}
		if (this.animatingShelf)
		{
			return;
		}
		if (this.shouldVerifySetSelection)
		{
			BuilderPieceSet selectedSet = this.setSelector.GetSelectedSet();
			if (selectedSet == null || !BuilderSetManager.instance.DoesAnyPlayerInRoomOwnPieceSet(selectedSet.GetIntIdentifier()))
			{
				int defaultSetID = this.setSelector.GetDefaultSetID();
				if (defaultSetID != -1)
				{
					this.OnSelectedSetChange(defaultSetID);
				}
			}
			this.shouldVerifySetSelection = false;
		}
		if (this.activeDispensers.Count > 0)
		{
			this.activeDispensers[this.dispenserToUpdate].UpdateDispenser();
			this.dispenserToUpdate = (this.dispenserToUpdate + 1) % this.activeDispensers.Count;
		}
	}

	// Token: 0x06001DCE RID: 7630 RVA: 0x000445E4 File Offset: 0x000427E4
	public void VerifySetSelection()
	{
		this.shouldVerifySetSelection = true;
	}

	// Token: 0x06001DCF RID: 7631 RVA: 0x000E2D80 File Offset: 0x000E0F80
	public void OnShelfPieceCreated(BuilderPiece piece, bool playfx)
	{
		if (this.playSpawnSetSound && playfx)
		{
			this.audioSource.GTPlayOneShot(this.spawnNewSetSound, 1f);
			this.playSpawnSetSound = false;
		}
		foreach (BuilderDispenser builderDispenser in this.activeDispensers)
		{
			builderDispenser.ShelfPieceCreated(piece, playfx);
		}
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x000E2DFC File Offset: 0x000E0FFC
	public void OnShelfPieceRecycled(BuilderPiece piece)
	{
		foreach (BuilderDispenser builderDispenser in this.activeDispensers)
		{
			builderDispenser.ShelfPieceRecycled(piece);
		}
	}

	// Token: 0x06001DD1 RID: 7633 RVA: 0x000E2E50 File Offset: 0x000E1050
	public void OnClearTable()
	{
		if (!this.initialized)
		{
			return;
		}
		foreach (BuilderDispenser builderDispenser in this.activeDispensers)
		{
			builderDispenser.OnClearTable();
		}
		base.StopAllCoroutines();
		if (this.animatingShelf)
		{
			this.resetAnimation.Rewind();
			this.animatingShelf = false;
		}
	}

	// Token: 0x06001DD2 RID: 7634 RVA: 0x000E2ECC File Offset: 0x000E10CC
	public void ClearShelf()
	{
		foreach (BuilderDispenser builderDispenser in this.activeDispensers)
		{
			builderDispenser.ClearDispenser();
		}
	}

	// Token: 0x040020D1 RID: 8401
	[Header("Set Selection")]
	[SerializeField]
	private BuilderSetSelector setSelector;

	// Token: 0x040020D2 RID: 8402
	public List<BuilderPieceSet.BuilderPieceCategory> _includedCategories;

	// Token: 0x040020D3 RID: 8403
	[Header("Dispenser Shelf Properties")]
	public Transform shelfCenter;

	// Token: 0x040020D4 RID: 8404
	public float shelfWidth = 1.4f;

	// Token: 0x040020D5 RID: 8405
	public Animation resetAnimation;

	// Token: 0x040020D6 RID: 8406
	[SerializeField]
	private SoundBankPlayer resetSoundBank;

	// Token: 0x040020D7 RID: 8407
	[SerializeField]
	private AudioClip spawnNewSetSound;

	// Token: 0x040020D8 RID: 8408
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x040020D9 RID: 8409
	private bool playSpawnSetSound;

	// Token: 0x040020DA RID: 8410
	[HideInInspector]
	public BuilderTable table;

	// Token: 0x040020DB RID: 8411
	public int shelfID = -1;

	// Token: 0x040020DC RID: 8412
	private BuilderPieceSet currentSet;

	// Token: 0x040020DD RID: 8413
	private bool initialized;

	// Token: 0x040020DE RID: 8414
	public BuilderDispenser dispenserPrefab;

	// Token: 0x040020DF RID: 8415
	private List<BuilderDispenser> dispenserPool;

	// Token: 0x040020E0 RID: 8416
	private List<BuilderDispenser> activeDispensers;

	// Token: 0x040020E1 RID: 8417
	private List<BuilderPieceSet.PieceInfo> piecesInSet = new List<BuilderPieceSet.PieceInfo>(10);

	// Token: 0x040020E2 RID: 8418
	private bool animatingShelf;

	// Token: 0x040020E3 RID: 8419
	private double timeToClearShelf = double.MaxValue;

	// Token: 0x040020E4 RID: 8420
	private int dispenserToClear;

	// Token: 0x040020E5 RID: 8421
	private int dispenserToUpdate;

	// Token: 0x040020E6 RID: 8422
	private bool shouldVerifySetSelection;
}
