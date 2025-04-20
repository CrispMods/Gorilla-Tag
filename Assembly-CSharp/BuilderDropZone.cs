using System;
using System.Collections;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004CB RID: 1227
public class BuilderDropZone : MonoBehaviour
{
	// Token: 0x06001DD4 RID: 7636 RVA: 0x00044623 File Offset: 0x00042823
	private void Awake()
	{
		this.repelDirectionWorld = base.transform.TransformDirection(this.repelDirectionLocal.normalized);
	}

	// Token: 0x06001DD5 RID: 7637 RVA: 0x000E2F1C File Offset: 0x000E111C
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

	// Token: 0x06001DD6 RID: 7638 RVA: 0x00044641 File Offset: 0x00042841
	public Vector3 GetRepelDirectionWorld()
	{
		return this.repelDirectionWorld;
	}

	// Token: 0x06001DD7 RID: 7639 RVA: 0x000E2FE0 File Offset: 0x000E11E0
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

	// Token: 0x06001DD8 RID: 7640 RVA: 0x00044649 File Offset: 0x00042849
	private IEnumerator DelayedStopEffect()
	{
		yield return new WaitForSeconds(this.effectDuration);
		this.vfxRoot.SetActive(false);
		this.playingEffect = false;
		yield break;
	}

	// Token: 0x06001DD9 RID: 7641 RVA: 0x000E3058 File Offset: 0x000E1258
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

	// Token: 0x040020E7 RID: 8423
	[SerializeField]
	private BuilderDropZone.DropType dropType;

	// Token: 0x040020E8 RID: 8424
	[SerializeField]
	private bool onEnter = true;

	// Token: 0x040020E9 RID: 8425
	[SerializeField]
	private GameObject vfxRoot;

	// Token: 0x040020EA RID: 8426
	[SerializeField]
	private GameObject sfxPrefab;

	// Token: 0x040020EB RID: 8427
	public float effectDuration = 1f;

	// Token: 0x040020EC RID: 8428
	private bool playingEffect;

	// Token: 0x040020ED RID: 8429
	public bool overrideDirection;

	// Token: 0x040020EE RID: 8430
	[SerializeField]
	private Vector3 repelDirectionLocal = Vector3.up;

	// Token: 0x040020EF RID: 8431
	private Vector3 repelDirectionWorld = Vector3.up;

	// Token: 0x040020F0 RID: 8432
	[HideInInspector]
	public int dropZoneID = -1;

	// Token: 0x020004CC RID: 1228
	public enum DropType
	{
		// Token: 0x040020F2 RID: 8434
		Invalid = -1,
		// Token: 0x040020F3 RID: 8435
		Repel,
		// Token: 0x040020F4 RID: 8436
		ReturnToShelf,
		// Token: 0x040020F5 RID: 8437
		BreakApart,
		// Token: 0x040020F6 RID: 8438
		Recycle
	}
}
