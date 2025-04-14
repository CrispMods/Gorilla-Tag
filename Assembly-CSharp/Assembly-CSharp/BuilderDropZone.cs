using System;
using System.Collections;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004BE RID: 1214
public class BuilderDropZone : MonoBehaviour
{
	// Token: 0x06001D7E RID: 7550 RVA: 0x0009034E File Offset: 0x0008E54E
	private void Awake()
	{
		this.repelDirectionWorld = base.transform.TransformDirection(this.repelDirectionLocal.normalized);
	}

	// Token: 0x06001D7F RID: 7551 RVA: 0x0009036C File Offset: 0x0008E56C
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

	// Token: 0x06001D80 RID: 7552 RVA: 0x00090430 File Offset: 0x0008E630
	public Vector3 GetRepelDirectionWorld()
	{
		return this.repelDirectionWorld;
	}

	// Token: 0x06001D81 RID: 7553 RVA: 0x00090438 File Offset: 0x0008E638
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

	// Token: 0x06001D82 RID: 7554 RVA: 0x000904B0 File Offset: 0x0008E6B0
	private IEnumerator DelayedStopEffect()
	{
		yield return new WaitForSeconds(this.effectDuration);
		this.vfxRoot.SetActive(false);
		this.playingEffect = false;
		yield break;
	}

	// Token: 0x06001D83 RID: 7555 RVA: 0x000904C0 File Offset: 0x0008E6C0
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

	// Token: 0x04002095 RID: 8341
	[SerializeField]
	private BuilderDropZone.DropType dropType;

	// Token: 0x04002096 RID: 8342
	[SerializeField]
	private bool onEnter = true;

	// Token: 0x04002097 RID: 8343
	[SerializeField]
	private GameObject vfxRoot;

	// Token: 0x04002098 RID: 8344
	[SerializeField]
	private GameObject sfxPrefab;

	// Token: 0x04002099 RID: 8345
	public float effectDuration = 1f;

	// Token: 0x0400209A RID: 8346
	private bool playingEffect;

	// Token: 0x0400209B RID: 8347
	public bool overrideDirection;

	// Token: 0x0400209C RID: 8348
	[SerializeField]
	private Vector3 repelDirectionLocal = Vector3.up;

	// Token: 0x0400209D RID: 8349
	private Vector3 repelDirectionWorld = Vector3.up;

	// Token: 0x0400209E RID: 8350
	[HideInInspector]
	public int dropZoneID = -1;

	// Token: 0x020004BF RID: 1215
	public enum DropType
	{
		// Token: 0x040020A0 RID: 8352
		Invalid = -1,
		// Token: 0x040020A1 RID: 8353
		Repel,
		// Token: 0x040020A2 RID: 8354
		ReturnToShelf,
		// Token: 0x040020A3 RID: 8355
		BreakApart,
		// Token: 0x040020A4 RID: 8356
		Recycle
	}
}
