using System;
using System.Collections;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004BE RID: 1214
public class BuilderDropZone : MonoBehaviour
{
	// Token: 0x06001D7B RID: 7547 RVA: 0x0008FFCA File Offset: 0x0008E1CA
	private void Awake()
	{
		this.repelDirectionWorld = base.transform.TransformDirection(this.repelDirectionLocal.normalized);
	}

	// Token: 0x06001D7C RID: 7548 RVA: 0x0008FFE8 File Offset: 0x0008E1E8
	private void OnTriggerEnter(Collider other)
	{
		if (!this.onEnter)
		{
			return;
		}
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		BuilderPieceCollider component = other.GetComponent<BuilderPieceCollider>();
		if (component != null)
		{
			BuilderPiece piece = component.piece;
			if (BuilderTable.instance != null)
			{
				if (piece == null)
				{
					return;
				}
				if (this.dropType == BuilderDropZone.DropType.Recycle)
				{
					bool flag = piece.state != BuilderPiece.State.Displayed && piece.state != BuilderPiece.State.OnShelf && piece.state > BuilderPiece.State.AttachedAndPlaced;
					if (!piece.isBuiltIntoTable && flag)
					{
						BuilderTableNetworking.instance.RequestRecyclePiece(piece.pieceId, piece.transform.position, piece.transform.rotation, true, -1);
						return;
					}
				}
				else
				{
					BuilderTableNetworking.instance.PieceEnteredDropZone(piece, this.dropType, this.dropZoneID);
				}
			}
		}
	}

	// Token: 0x06001D7D RID: 7549 RVA: 0x000900AC File Offset: 0x0008E2AC
	public Vector3 GetRepelDirectionWorld()
	{
		return this.repelDirectionWorld;
	}

	// Token: 0x06001D7E RID: 7550 RVA: 0x000900B4 File Offset: 0x0008E2B4
	public void PlayEffect()
	{
		if (this.vfxRoot != null && !this.playingEffect)
		{
			this.vfxRoot.SetActive(true);
			this.playingEffect = true;
			if (this.sfxPrefab != null)
			{
				ObjectPools.instance.Instantiate(this.sfxPrefab, base.transform.position, base.transform.rotation);
			}
			base.StartCoroutine(this.DelayedStopEffect());
		}
	}

	// Token: 0x06001D7F RID: 7551 RVA: 0x0009012C File Offset: 0x0008E32C
	private IEnumerator DelayedStopEffect()
	{
		yield return new WaitForSeconds(this.effectDuration);
		this.vfxRoot.SetActive(false);
		this.playingEffect = false;
		yield break;
	}

	// Token: 0x06001D80 RID: 7552 RVA: 0x0009013C File Offset: 0x0008E33C
	private void OnTriggerExit(Collider other)
	{
		if (this.onEnter)
		{
			return;
		}
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		BuilderPieceCollider component = other.GetComponent<BuilderPieceCollider>();
		if (component != null)
		{
			BuilderPiece piece = component.piece;
			if (BuilderTableNetworking.instance != null)
			{
				if (piece == null)
				{
					return;
				}
				if (this.dropType == BuilderDropZone.DropType.Recycle)
				{
					bool flag = piece.state != BuilderPiece.State.Displayed && piece.state != BuilderPiece.State.OnShelf && piece.state > BuilderPiece.State.AttachedAndPlaced;
					if (!piece.isBuiltIntoTable && flag)
					{
						BuilderTableNetworking.instance.RequestRecyclePiece(piece.pieceId, piece.transform.position, piece.transform.rotation, true, -1);
						return;
					}
				}
				else
				{
					BuilderTableNetworking.instance.PieceEnteredDropZone(piece, this.dropType, this.dropZoneID);
				}
			}
		}
	}

	// Token: 0x04002094 RID: 8340
	[SerializeField]
	private BuilderDropZone.DropType dropType;

	// Token: 0x04002095 RID: 8341
	[SerializeField]
	private bool onEnter = true;

	// Token: 0x04002096 RID: 8342
	[SerializeField]
	private GameObject vfxRoot;

	// Token: 0x04002097 RID: 8343
	[SerializeField]
	private GameObject sfxPrefab;

	// Token: 0x04002098 RID: 8344
	public float effectDuration = 1f;

	// Token: 0x04002099 RID: 8345
	private bool playingEffect;

	// Token: 0x0400209A RID: 8346
	public bool overrideDirection;

	// Token: 0x0400209B RID: 8347
	[SerializeField]
	private Vector3 repelDirectionLocal = Vector3.up;

	// Token: 0x0400209C RID: 8348
	private Vector3 repelDirectionWorld = Vector3.up;

	// Token: 0x0400209D RID: 8349
	[HideInInspector]
	public int dropZoneID = -1;

	// Token: 0x020004BF RID: 1215
	public enum DropType
	{
		// Token: 0x0400209F RID: 8351
		Invalid = -1,
		// Token: 0x040020A0 RID: 8352
		Repel,
		// Token: 0x040020A1 RID: 8353
		ReturnToShelf,
		// Token: 0x040020A2 RID: 8354
		BreakApart,
		// Token: 0x040020A3 RID: 8355
		Recycle
	}
}
