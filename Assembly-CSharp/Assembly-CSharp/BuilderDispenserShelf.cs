using System;
using System.Collections.Generic;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004BD RID: 1213
public class BuilderDispenserShelf : MonoBehaviour
{
	// Token: 0x06001D6B RID: 7531 RVA: 0x0008FB47 File Offset: 0x0008DD47
	private void BuildDispenserPool()
	{
		this.dispenserPool = new List<BuilderDispenser>(12);
		this.activeDispensers = new List<BuilderDispenser>(6);
		this.AddToDispenserPool(6);
	}

	// Token: 0x06001D6C RID: 7532 RVA: 0x0008FB6C File Offset: 0x0008DD6C
	private void AddToDispenserPool(int count)
	{
		if (this.dispenserPrefab == null)
		{
			return;
		}
		for (int i = 0; i < count; i++)
		{
			BuilderDispenser builderDispenser = Object.Instantiate<BuilderDispenser>(this.dispenserPrefab, this.shelfCenter);
			builderDispenser.gameObject.SetActive(false);
			builderDispenser.table = this.table;
			builderDispenser.shelfID = this.shelfID;
			this.dispenserPool.Add(builderDispenser);
		}
	}

	// Token: 0x06001D6D RID: 7533 RVA: 0x0008FBD8 File Offset: 0x0008DDD8
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

	// Token: 0x06001D6E RID: 7534 RVA: 0x0008FD84 File Offset: 0x0008DF84
	public void Setup()
	{
		this.InitIfNeeded();
		foreach (BuilderDispenser builderDispenser in this.dispenserPool)
		{
			builderDispenser.table = this.table;
			builderDispenser.shelfID = this.shelfID;
		}
	}

	// Token: 0x06001D6F RID: 7535 RVA: 0x0008FDEC File Offset: 0x0008DFEC
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

	// Token: 0x06001D70 RID: 7536 RVA: 0x0008FE53 File Offset: 0x0008E053
	private void OnDestroy()
	{
		if (this.setSelector != null)
		{
			this.setSelector.OnSelectedSet.RemoveListener(new UnityAction<int>(this.OnSelectedSetChange));
		}
	}

	// Token: 0x06001D71 RID: 7537 RVA: 0x0008FE7F File Offset: 0x0008E07F
	public void OnSelectedSetChange(int setId)
	{
		if (this.table.GetTableState() != BuilderTable.TableState.Ready)
		{
			return;
		}
		this.table.RequestShelfSelection(this.shelfID, setId, false);
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x0008FEA4 File Offset: 0x0008E0A4
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

	// Token: 0x06001D73 RID: 7539 RVA: 0x0008FF28 File Offset: 0x0008E128
	public int GetSelectedSetID()
	{
		return this.setSelector.GetSelectedSet().GetIntIdentifier();
	}

	// Token: 0x06001D74 RID: 7540 RVA: 0x0008FF3C File Offset: 0x0008E13C
	private void ImmediateShelfSwap()
	{
		foreach (BuilderDispenser builderDispenser in this.activeDispensers)
		{
			builderDispenser.ClearDispenser();
		}
		this.ActivateDispensers();
	}

	// Token: 0x06001D75 RID: 7541 RVA: 0x0008FF94 File Offset: 0x0008E194
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

	// Token: 0x06001D76 RID: 7542 RVA: 0x00090028 File Offset: 0x0008E228
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

	// Token: 0x06001D77 RID: 7543 RVA: 0x000900B8 File Offset: 0x0008E2B8
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

	// Token: 0x06001D78 RID: 7544 RVA: 0x00090171 File Offset: 0x0008E371
	public void VerifySetSelection()
	{
		this.shouldVerifySetSelection = true;
	}

	// Token: 0x06001D79 RID: 7545 RVA: 0x0009017C File Offset: 0x0008E37C
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

	// Token: 0x06001D7A RID: 7546 RVA: 0x000901F8 File Offset: 0x0008E3F8
	public void OnShelfPieceRecycled(BuilderPiece piece)
	{
		foreach (BuilderDispenser builderDispenser in this.activeDispensers)
		{
			builderDispenser.ShelfPieceRecycled(piece);
		}
	}

	// Token: 0x06001D7B RID: 7547 RVA: 0x0009024C File Offset: 0x0008E44C
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

	// Token: 0x06001D7C RID: 7548 RVA: 0x000902C8 File Offset: 0x0008E4C8
	public void ClearShelf()
	{
		foreach (BuilderDispenser builderDispenser in this.activeDispensers)
		{
			builderDispenser.ClearDispenser();
		}
	}

	// Token: 0x0400207F RID: 8319
	[Header("Set Selection")]
	[SerializeField]
	private BuilderSetSelector setSelector;

	// Token: 0x04002080 RID: 8320
	public List<BuilderPieceSet.BuilderPieceCategory> _includedCategories;

	// Token: 0x04002081 RID: 8321
	[Header("Dispenser Shelf Properties")]
	public Transform shelfCenter;

	// Token: 0x04002082 RID: 8322
	public float shelfWidth = 1.4f;

	// Token: 0x04002083 RID: 8323
	public Animation resetAnimation;

	// Token: 0x04002084 RID: 8324
	[SerializeField]
	private SoundBankPlayer resetSoundBank;

	// Token: 0x04002085 RID: 8325
	[SerializeField]
	private AudioClip spawnNewSetSound;

	// Token: 0x04002086 RID: 8326
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04002087 RID: 8327
	private bool playSpawnSetSound;

	// Token: 0x04002088 RID: 8328
	[HideInInspector]
	public BuilderTable table;

	// Token: 0x04002089 RID: 8329
	public int shelfID = -1;

	// Token: 0x0400208A RID: 8330
	private BuilderPieceSet currentSet;

	// Token: 0x0400208B RID: 8331
	private bool initialized;

	// Token: 0x0400208C RID: 8332
	public BuilderDispenser dispenserPrefab;

	// Token: 0x0400208D RID: 8333
	private List<BuilderDispenser> dispenserPool;

	// Token: 0x0400208E RID: 8334
	private List<BuilderDispenser> activeDispensers;

	// Token: 0x0400208F RID: 8335
	private List<BuilderPieceSet.PieceInfo> piecesInSet = new List<BuilderPieceSet.PieceInfo>(10);

	// Token: 0x04002090 RID: 8336
	private bool animatingShelf;

	// Token: 0x04002091 RID: 8337
	private double timeToClearShelf = double.MaxValue;

	// Token: 0x04002092 RID: 8338
	private int dispenserToClear;

	// Token: 0x04002093 RID: 8339
	private int dispenserToUpdate;

	// Token: 0x04002094 RID: 8340
	private bool shouldVerifySetSelection;
}
