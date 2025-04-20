using System;
using System.Collections;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004C8 RID: 1224
public class BuilderDispenser : MonoBehaviour
{
	// Token: 0x06001DB0 RID: 7600 RVA: 0x000E20E0 File Offset: 0x000E02E0
	private void Awake()
	{
		this.nullPiece = new BuilderPieceSet.PieceInfo
		{
			piecePrefab = null,
			overrideSetMaterial = false
		};
	}

	// Token: 0x06001DB1 RID: 7601 RVA: 0x000E210C File Offset: 0x000E030C
	public void UpdateDispenser()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (!this.hasPiece && Time.timeAsDouble > this.nextSpawnTime && this.pieceToSpawn.piecePrefab != null)
		{
			this.TrySpawnPiece();
			this.nextSpawnTime = Time.timeAsDouble + (double)this.spawnRetryDelay;
			return;
		}
		if (this.hasPiece && (this.spawnedPieceInstance == null || (this.spawnedPieceInstance.state != BuilderPiece.State.OnShelf && this.spawnedPieceInstance.state != BuilderPiece.State.Displayed)))
		{
			base.StopAllCoroutines();
			if (this.spawnedPieceInstance != null)
			{
				this.spawnedPieceInstance.shelfOwner = -1;
			}
			this.nextSpawnTime = Time.timeAsDouble + (double)this.OnGrabSpawnDelay;
			this.spawnedPieceInstance = null;
			this.hasPiece = false;
		}
	}

	// Token: 0x06001DB2 RID: 7602 RVA: 0x000E21D8 File Offset: 0x000E03D8
	public void ShelfPieceCreated(BuilderPiece piece, bool playAnimation)
	{
		if (piece != null && this.pieceToSpawn.piecePrefab != null && piece.pieceType == this.pieceToSpawn.piecePrefab.name.GetStaticHash())
		{
			if (this.hasPiece && this.spawnedPieceInstance != null)
			{
				this.spawnedPieceInstance.shelfOwner = -1;
			}
			this.spawnedPieceInstance = piece;
			this.hasPiece = true;
			this.spawnCount++;
			this.spawnCount = Mathf.Max(0, this.spawnCount);
			if (BuilderTable.instance.GetTableState() == BuilderTable.TableState.Ready && playAnimation)
			{
				base.StartCoroutine(this.PlayAnimation());
				if (this.playFX)
				{
					ObjectPools.instance.Instantiate(this.dispenserFX, this.spawnTransform.position, this.spawnTransform.rotation);
					return;
				}
				this.playFX = true;
				return;
			}
			else
			{
				Vector3 desiredShelfOffset = this.pieceToSpawn.piecePrefab.desiredShelfOffset;
				Vector3 position = this.displayTransform.position + this.displayTransform.rotation * desiredShelfOffset;
				Quaternion rotation = this.displayTransform.rotation * Quaternion.Euler(this.pieceToSpawn.piecePrefab.desiredShelfRotationOffset);
				this.spawnedPieceInstance.transform.SetPositionAndRotation(position, rotation);
				this.spawnedPieceInstance.SetState(BuilderPiece.State.OnShelf, false);
				this.playFX = true;
			}
		}
	}

	// Token: 0x06001DB3 RID: 7603 RVA: 0x000444EF File Offset: 0x000426EF
	private IEnumerator PlayAnimation()
	{
		this.spawnedPieceInstance.SetState(BuilderPiece.State.Displayed, false);
		this.animateParent.Rewind();
		this.spawnedPieceInstance.transform.SetParent(this.animateParent.transform);
		this.spawnedPieceInstance.transform.SetLocalPositionAndRotation(this.pieceToSpawn.piecePrefab.desiredShelfOffset, Quaternion.Euler(this.pieceToSpawn.piecePrefab.desiredShelfRotationOffset));
		this.animateParent.Play();
		yield return new WaitForSeconds(this.animateParent.clip.length);
		if (this.spawnedPieceInstance != null && this.spawnedPieceInstance.state == BuilderPiece.State.Displayed)
		{
			this.spawnedPieceInstance.transform.SetParent(null);
			Vector3 desiredShelfOffset = this.pieceToSpawn.piecePrefab.desiredShelfOffset;
			Vector3 position = this.displayTransform.position + this.displayTransform.rotation * desiredShelfOffset;
			Quaternion rotation = this.displayTransform.rotation * Quaternion.Euler(this.pieceToSpawn.piecePrefab.desiredShelfRotationOffset);
			this.spawnedPieceInstance.transform.SetPositionAndRotation(position, rotation);
			this.spawnedPieceInstance.SetState(BuilderPiece.State.OnShelf, false);
		}
		yield break;
	}

	// Token: 0x06001DB4 RID: 7604 RVA: 0x000E234C File Offset: 0x000E054C
	public void ShelfPieceRecycled(BuilderPiece piece)
	{
		if (piece != null && this.spawnedPieceInstance != null && piece.Equals(this.spawnedPieceInstance))
		{
			piece.shelfOwner = -1;
			this.spawnedPieceInstance = null;
			this.hasPiece = false;
			this.nextSpawnTime = Time.timeAsDouble + (double)this.OnGrabSpawnDelay;
		}
	}

	// Token: 0x06001DB5 RID: 7605 RVA: 0x000E23A8 File Offset: 0x000E05A8
	public void AssignPieceType(BuilderPieceSet.PieceInfo piece, int inMaterialType)
	{
		this.playFX = false;
		this.pieceToSpawn = piece;
		this.materialType = inMaterialType;
		this.nextSpawnTime = Time.timeAsDouble + (double)this.OnGrabSpawnDelay;
		this.currentAnimation = this.dispenseDefaultAnimation;
		this.animateParent.clip = this.currentAnimation;
		this.spawnCount = 0;
	}

	// Token: 0x06001DB6 RID: 7606 RVA: 0x000E2404 File Offset: 0x000E0604
	private void TrySpawnPiece()
	{
		if (this.spawnedPieceInstance != null && this.spawnedPieceInstance.state == BuilderPiece.State.OnShelf)
		{
			return;
		}
		if (this.pieceToSpawn.piecePrefab == null)
		{
			return;
		}
		if (this.table.HasEnoughResources(this.pieceToSpawn.piecePrefab))
		{
			Vector3 desiredShelfOffset = this.pieceToSpawn.piecePrefab.desiredShelfOffset;
			Vector3 position = this.spawnTransform.position + this.spawnTransform.rotation * desiredShelfOffset;
			Quaternion rotation = this.spawnTransform.rotation * Quaternion.Euler(this.pieceToSpawn.piecePrefab.desiredShelfRotationOffset);
			int num = this.materialType;
			if (this.pieceToSpawn.overrideSetMaterial && this.pieceToSpawn.pieceMaterialTypes.Length != 0)
			{
				int num2 = this.spawnCount % this.pieceToSpawn.pieceMaterialTypes.Length;
				string text = this.pieceToSpawn.pieceMaterialTypes[num2];
				if (string.IsNullOrEmpty(text))
				{
					num = -1;
				}
				else
				{
					num = text.GetHashCode();
				}
			}
			this.table.RequestCreateDispenserShelfPiece(this.pieceToSpawn.piecePrefab.name.GetStaticHash(), position, rotation, num, this.shelfID);
		}
	}

	// Token: 0x06001DB7 RID: 7607 RVA: 0x000E253C File Offset: 0x000E073C
	public void ParentPieceToShelf(Transform shelfTransform)
	{
		if (this.spawnedPieceInstance != null)
		{
			if (this.spawnedPieceInstance.state != BuilderPiece.State.OnShelf && this.spawnedPieceInstance.state != BuilderPiece.State.Displayed)
			{
				base.StopAllCoroutines();
				if (this.spawnedPieceInstance != null)
				{
					this.spawnedPieceInstance.shelfOwner = -1;
				}
				this.nextSpawnTime = Time.timeAsDouble + (double)this.OnGrabSpawnDelay;
				this.spawnedPieceInstance = null;
				this.hasPiece = false;
				return;
			}
			this.spawnedPieceInstance.SetState(BuilderPiece.State.Displayed, false);
			this.spawnedPieceInstance.transform.SetParent(shelfTransform);
		}
	}

	// Token: 0x06001DB8 RID: 7608 RVA: 0x000E25D4 File Offset: 0x000E07D4
	public void ClearDispenser()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		this.pieceToSpawn = this.nullPiece;
		this.hasPiece = false;
		if (this.spawnedPieceInstance != null)
		{
			if (this.spawnedPieceInstance.state != BuilderPiece.State.OnShelf && this.spawnedPieceInstance.state != BuilderPiece.State.Displayed)
			{
				this.spawnedPieceInstance.shelfOwner = -1;
				this.nextSpawnTime = Time.timeAsDouble + (double)this.OnGrabSpawnDelay;
				this.spawnedPieceInstance = null;
				return;
			}
			this.table.RequestRecyclePiece(this.spawnedPieceInstance, false, -1);
		}
	}

	// Token: 0x06001DB9 RID: 7609 RVA: 0x000444FE File Offset: 0x000426FE
	public void OnClearTable()
	{
		this.playFX = false;
		this.nextSpawnTime = 0.0;
		this.hasPiece = false;
		this.spawnedPieceInstance = null;
	}

	// Token: 0x040020BC RID: 8380
	public Transform displayTransform;

	// Token: 0x040020BD RID: 8381
	public Transform spawnTransform;

	// Token: 0x040020BE RID: 8382
	public Animation animateParent;

	// Token: 0x040020BF RID: 8383
	public AnimationClip dispenseDefaultAnimation;

	// Token: 0x040020C0 RID: 8384
	public GameObject dispenserFX;

	// Token: 0x040020C1 RID: 8385
	private AnimationClip currentAnimation;

	// Token: 0x040020C2 RID: 8386
	[HideInInspector]
	public BuilderTable table;

	// Token: 0x040020C3 RID: 8387
	[HideInInspector]
	public int shelfID;

	// Token: 0x040020C4 RID: 8388
	private BuilderPieceSet.PieceInfo pieceToSpawn;

	// Token: 0x040020C5 RID: 8389
	private BuilderPiece spawnedPieceInstance;

	// Token: 0x040020C6 RID: 8390
	private int materialType = -1;

	// Token: 0x040020C7 RID: 8391
	private BuilderPieceSet.PieceInfo nullPiece;

	// Token: 0x040020C8 RID: 8392
	private int spawnCount;

	// Token: 0x040020C9 RID: 8393
	private double nextSpawnTime;

	// Token: 0x040020CA RID: 8394
	private bool hasPiece;

	// Token: 0x040020CB RID: 8395
	private float OnGrabSpawnDelay = 0.5f;

	// Token: 0x040020CC RID: 8396
	private float spawnRetryDelay = 2f;

	// Token: 0x040020CD RID: 8397
	private bool playFX;
}
