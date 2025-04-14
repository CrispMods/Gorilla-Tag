using System;
using System.Collections;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004BB RID: 1211
public class BuilderDispenser : MonoBehaviour
{
	// Token: 0x06001D57 RID: 7511 RVA: 0x0008F054 File Offset: 0x0008D254
	private void Awake()
	{
		this.nullPiece = new BuilderPieceSet.PieceInfo
		{
			piecePrefab = null,
			overrideSetMaterial = false
		};
	}

	// Token: 0x06001D58 RID: 7512 RVA: 0x0008F080 File Offset: 0x0008D280
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

	// Token: 0x06001D59 RID: 7513 RVA: 0x0008F14C File Offset: 0x0008D34C
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

	// Token: 0x06001D5A RID: 7514 RVA: 0x0008F2C0 File Offset: 0x0008D4C0
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

	// Token: 0x06001D5B RID: 7515 RVA: 0x0008F2D0 File Offset: 0x0008D4D0
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

	// Token: 0x06001D5C RID: 7516 RVA: 0x0008F32C File Offset: 0x0008D52C
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

	// Token: 0x06001D5D RID: 7517 RVA: 0x0008F388 File Offset: 0x0008D588
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

	// Token: 0x06001D5E RID: 7518 RVA: 0x0008F4C0 File Offset: 0x0008D6C0
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

	// Token: 0x06001D5F RID: 7519 RVA: 0x0008F558 File Offset: 0x0008D758
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

	// Token: 0x06001D60 RID: 7520 RVA: 0x0008F5E4 File Offset: 0x0008D7E4
	public void OnClearTable()
	{
		this.playFX = false;
		this.nextSpawnTime = 0.0;
		this.hasPiece = false;
		this.spawnedPieceInstance = null;
	}

	// Token: 0x04002069 RID: 8297
	public Transform displayTransform;

	// Token: 0x0400206A RID: 8298
	public Transform spawnTransform;

	// Token: 0x0400206B RID: 8299
	public Animation animateParent;

	// Token: 0x0400206C RID: 8300
	public AnimationClip dispenseDefaultAnimation;

	// Token: 0x0400206D RID: 8301
	public GameObject dispenserFX;

	// Token: 0x0400206E RID: 8302
	private AnimationClip currentAnimation;

	// Token: 0x0400206F RID: 8303
	[HideInInspector]
	public BuilderTable table;

	// Token: 0x04002070 RID: 8304
	[HideInInspector]
	public int shelfID;

	// Token: 0x04002071 RID: 8305
	private BuilderPieceSet.PieceInfo pieceToSpawn;

	// Token: 0x04002072 RID: 8306
	private BuilderPiece spawnedPieceInstance;

	// Token: 0x04002073 RID: 8307
	private int materialType = -1;

	// Token: 0x04002074 RID: 8308
	private BuilderPieceSet.PieceInfo nullPiece;

	// Token: 0x04002075 RID: 8309
	private int spawnCount;

	// Token: 0x04002076 RID: 8310
	private double nextSpawnTime;

	// Token: 0x04002077 RID: 8311
	private bool hasPiece;

	// Token: 0x04002078 RID: 8312
	private float OnGrabSpawnDelay = 0.5f;

	// Token: 0x04002079 RID: 8313
	private float spawnRetryDelay = 2f;

	// Token: 0x0400207A RID: 8314
	private bool playFX;
}
