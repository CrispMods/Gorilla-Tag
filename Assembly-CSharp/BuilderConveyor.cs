using System;
using System.Collections.Generic;
using GorillaTagScripts;
using Photon.Pun;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

// Token: 0x020004C7 RID: 1223
public class BuilderConveyor : MonoBehaviour
{
	// Token: 0x06001D98 RID: 7576 RVA: 0x000443E8 File Offset: 0x000425E8
	private void Start()
	{
		this.InitIfNeeded();
	}

	// Token: 0x06001D99 RID: 7577 RVA: 0x000443E8 File Offset: 0x000425E8
	public void Setup()
	{
		this.InitIfNeeded();
	}

	// Token: 0x06001D9A RID: 7578 RVA: 0x000E1878 File Offset: 0x000DFA78
	private void InitIfNeeded()
	{
		if (this.initialized)
		{
			return;
		}
		this.nextPieceToSpawn = 0;
		this.grabbedPieceTypes = new Queue<int>(10);
		this.grabbedPieceMaterials = new Queue<int>(10);
		this.setSelector.Setup(this._includeCategories);
		this.currentSet = this.setSelector.GetSelectedSet();
		this.piecesInSet.Clear();
		foreach (BuilderPieceSet.BuilderPieceSubset builderPieceSubset in this.currentSet.subsets)
		{
			if (this._includeCategories.Contains(builderPieceSubset.pieceCategory))
			{
				this.piecesInSet.AddRange(builderPieceSubset.pieceInfos);
			}
		}
		double timeAsDouble = Time.timeAsDouble;
		this.nextSpawnTime = timeAsDouble + (double)this.spawnDelay;
		this.setSelector.OnSelectedSet.AddListener(new UnityAction<int>(this.OnSelectedSetChange));
		this.initialized = true;
		this.splineLength = this.spline.Splines[0].GetLength();
		this.maxItemsOnSpline = Mathf.RoundToInt(this.splineLength / (this.conveyorMoveSpeed * this.spawnDelay)) + 5;
		this.nativeSpline = new NativeSpline(this.spline.Splines[0], this.spline.transform.localToWorldMatrix, Allocator.Persistent);
	}

	// Token: 0x06001D9B RID: 7579 RVA: 0x000443F0 File Offset: 0x000425F0
	public int GetMaxItemsOnConveyor()
	{
		return Mathf.RoundToInt(this.splineLength / (this.conveyorMoveSpeed * this.spawnDelay)) + 5;
	}

	// Token: 0x06001D9C RID: 7580 RVA: 0x0004440D File Offset: 0x0004260D
	public float GetFrameMovement()
	{
		return this.conveyorMoveSpeed / this.splineLength;
	}

	// Token: 0x06001D9D RID: 7581 RVA: 0x0004441C File Offset: 0x0004261C
	private void OnDestroy()
	{
		if (this.setSelector != null)
		{
			this.setSelector.OnSelectedSet.RemoveListener(new UnityAction<int>(this.OnSelectedSetChange));
		}
		this.nativeSpline.Dispose();
	}

	// Token: 0x06001D9E RID: 7582 RVA: 0x00044453 File Offset: 0x00042653
	public void OnSelectedSetChange(int setId)
	{
		if (this.table.GetTableState() != BuilderTable.TableState.Ready)
		{
			return;
		}
		this.table.RequestShelfSelection(this.shelfID, setId, true);
	}

	// Token: 0x06001D9F RID: 7583 RVA: 0x000E19EC File Offset: 0x000DFBEC
	public void SetSelection(int setId)
	{
		this.setSelector.SetSelection(setId);
		this.currentSet = this.setSelector.GetSelectedSet();
		this.piecesInSet.Clear();
		foreach (BuilderPieceSet.BuilderPieceSubset builderPieceSubset in this.currentSet.subsets)
		{
			if (this._includeCategories.Contains(builderPieceSubset.pieceCategory))
			{
				this.piecesInSet.AddRange(builderPieceSubset.pieceInfos);
			}
		}
		this.nextPieceToSpawn = 0;
		this.loopCount = 0;
	}

	// Token: 0x06001DA0 RID: 7584 RVA: 0x00044477 File Offset: 0x00042677
	public int GetSelectedSetID()
	{
		return this.setSelector.GetSelectedSet().GetIntIdentifier();
	}

	// Token: 0x06001DA1 RID: 7585 RVA: 0x000E1A98 File Offset: 0x000DFC98
	public void UpdateConveyor()
	{
		if (!this.initialized)
		{
			this.Setup();
		}
		for (int i = this.piecesOnConveyor.Count - 1; i >= 0; i--)
		{
			BuilderPiece builderPiece = this.piecesOnConveyor[i];
			if (builderPiece.state != BuilderPiece.State.OnConveyor)
			{
				if (PhotonNetwork.LocalPlayer.IsMasterClient && builderPiece.state != BuilderPiece.State.None)
				{
					this.grabbedPieceTypes.Enqueue(builderPiece.pieceType);
					this.grabbedPieceMaterials.Enqueue(builderPiece.materialType);
				}
				builderPiece.shelfOwner = -1;
				this.piecesOnConveyor.RemoveAt(i);
				this.table.conveyorManager.RemovePieceFromJob(builderPiece);
			}
		}
	}

	// Token: 0x06001DA2 RID: 7586 RVA: 0x000E1B3C File Offset: 0x000DFD3C
	public void RemovePieceFromConveyor(Transform pieceTransform)
	{
		foreach (BuilderPiece builderPiece in this.piecesOnConveyor)
		{
			if (builderPiece.transform == pieceTransform)
			{
				this.piecesOnConveyor.Remove(builderPiece);
				builderPiece.shelfOwner = -1;
				this.table.RequestRecyclePiece(builderPiece, false, -1);
				break;
			}
		}
	}

	// Token: 0x06001DA3 RID: 7587 RVA: 0x000E1BBC File Offset: 0x000DFDBC
	private Vector3 EvaluateSpline(float t)
	{
		float t2;
		this._evaluateCurve = this.nativeSpline.GetCurve(this.nativeSpline.SplineToCurveT(t, out t2));
		return CurveUtility.EvaluatePosition(this._evaluateCurve, t2);
	}

	// Token: 0x06001DA4 RID: 7588 RVA: 0x000E1BFC File Offset: 0x000DFDFC
	public void UpdateShelfSliced()
	{
		if (!PhotonNetwork.LocalPlayer.IsMasterClient)
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
		if (this.waitForResourceChange)
		{
			return;
		}
		double timeAsDouble = Time.timeAsDouble;
		if (timeAsDouble >= this.nextSpawnTime)
		{
			this.SpawnNextPiece();
			this.nextSpawnTime = timeAsDouble + (double)this.spawnDelay;
		}
	}

	// Token: 0x06001DA5 RID: 7589 RVA: 0x00044489 File Offset: 0x00042689
	public void VerifySetSelection()
	{
		this.shouldVerifySetSelection = true;
	}

	// Token: 0x06001DA6 RID: 7590 RVA: 0x00044492 File Offset: 0x00042692
	public void OnAvailableResourcesChange()
	{
		this.waitForResourceChange = false;
	}

	// Token: 0x06001DA7 RID: 7591 RVA: 0x0004449B File Offset: 0x0004269B
	public Transform GetSpawnTransform()
	{
		return this.spawnTransform;
	}

	// Token: 0x06001DA8 RID: 7592 RVA: 0x000E1C94 File Offset: 0x000DFE94
	public void OnShelfPieceCreated(BuilderPiece piece, float timeOffset)
	{
		float num = timeOffset * this.conveyorMoveSpeed / this.splineLength;
		if (num > 1f)
		{
			Debug.LogWarningFormat("Piece {0} add to shelf time {1}", new object[]
			{
				piece.pieceId,
				num
			});
		}
		int count = this.piecesOnConveyor.Count;
		this.piecesOnConveyor.Add(piece);
		float num2 = Mathf.Clamp(num, 0f, 1f);
		Vector3 a = this.EvaluateSpline(num2);
		Quaternion rotation = this.spawnTransform.rotation * Quaternion.Euler(piece.desiredShelfRotationOffset);
		Vector3 position = a + this.spawnTransform.rotation * piece.desiredShelfOffset;
		piece.transform.SetPositionAndRotation(position, rotation);
		this.table.conveyorManager.AddPieceToJob(piece, num2, this.shelfID);
	}

	// Token: 0x06001DA9 RID: 7593 RVA: 0x000444A3 File Offset: 0x000426A3
	public void OnShelfPieceRecycled(BuilderPiece piece)
	{
		this.piecesOnConveyor.Remove(piece);
		if (piece != null)
		{
			this.table.conveyorManager.RemovePieceFromJob(piece);
		}
	}

	// Token: 0x06001DAA RID: 7594 RVA: 0x000444CC File Offset: 0x000426CC
	public void OnClearTable()
	{
		this.piecesOnConveyor.Clear();
		this.grabbedPieceTypes.Clear();
		this.grabbedPieceMaterials.Clear();
	}

	// Token: 0x06001DAB RID: 7595 RVA: 0x000E1D70 File Offset: 0x000DFF70
	public void ResetConveyorState()
	{
		for (int i = this.piecesOnConveyor.Count - 1; i >= 0; i--)
		{
			BuilderPiece builderPiece = this.piecesOnConveyor[i];
			if (!(builderPiece == null))
			{
				BuilderTable.BuilderCommand cmd = new BuilderTable.BuilderCommand
				{
					type = BuilderTable.BuilderCommandType.Recycle,
					pieceId = builderPiece.pieceId,
					localPosition = builderPiece.transform.position,
					localRotation = builderPiece.transform.rotation,
					player = NetworkSystem.Instance.LocalPlayer,
					isLeft = false,
					parentPieceId = -1
				};
				this.table.ExecutePieceRecycled(cmd);
			}
		}
		this.OnClearTable();
	}

	// Token: 0x06001DAC RID: 7596 RVA: 0x000E1E28 File Offset: 0x000E0028
	private void SpawnNextPiece()
	{
		int num;
		int materialType;
		this.FindNextAffordablePieceType(out num, out materialType);
		if (num == -1)
		{
			return;
		}
		this.table.RequestCreateConveyorPiece(num, materialType, this.shelfID);
	}

	// Token: 0x06001DAD RID: 7597 RVA: 0x000E1E58 File Offset: 0x000E0058
	private void FindNextAffordablePieceType(out int pieceType, out int materialType)
	{
		if (this.grabbedPieceTypes.Count > 0)
		{
			pieceType = this.grabbedPieceTypes.Dequeue();
			materialType = this.grabbedPieceMaterials.Dequeue();
			return;
		}
		pieceType = -1;
		materialType = -1;
		if (this.piecesInSet.Count <= 0)
		{
			return;
		}
		for (int i = this.nextPieceToSpawn; i < this.piecesInSet.Count; i++)
		{
			BuilderPiece piecePrefab = this.piecesInSet[i].piecePrefab;
			if (this.table.HasEnoughResources(piecePrefab))
			{
				if (i + 1 >= this.piecesInSet.Count)
				{
					this.loopCount++;
					this.loopCount = Mathf.Max(0, this.loopCount);
				}
				this.nextPieceToSpawn = (i + 1) % this.piecesInSet.Count;
				pieceType = piecePrefab.name.GetStaticHash();
				materialType = this.GetMaterialType(this.piecesInSet[i]);
				return;
			}
		}
		this.loopCount++;
		this.loopCount = Mathf.Max(0, this.loopCount);
		for (int j = 0; j < this.nextPieceToSpawn; j++)
		{
			BuilderPiece piecePrefab2 = this.piecesInSet[j].piecePrefab;
			if (this.table.HasEnoughResources(piecePrefab2))
			{
				this.nextPieceToSpawn = (j + 1) % this.piecesInSet.Count;
				pieceType = piecePrefab2.name.GetStaticHash();
				materialType = this.GetMaterialType(this.piecesInSet[j]);
				return;
			}
		}
		this.waitForResourceChange = true;
	}

	// Token: 0x06001DAE RID: 7598 RVA: 0x000E1FDC File Offset: 0x000E01DC
	private int GetMaterialType(BuilderPieceSet.PieceInfo info)
	{
		if (info.piecePrefab.materialOptions != null && info.overrideSetMaterial && info.pieceMaterialTypes.Length != 0)
		{
			int num = this.loopCount % info.pieceMaterialTypes.Length;
			string text = info.pieceMaterialTypes[num];
			if (string.IsNullOrEmpty(text))
			{
				Debug.LogErrorFormat("Empty Material Override for piece {0} in set {1}", new object[]
				{
					info.piecePrefab.name,
					this.currentSet.name
				});
				return -1;
			}
			return text.GetHashCode();
		}
		else
		{
			if (string.IsNullOrEmpty(this.currentSet.materialId))
			{
				return -1;
			}
			return this.currentSet.materialId.GetHashCode();
		}
	}

	// Token: 0x040020A4 RID: 8356
	[Header("Set Selection")]
	[SerializeField]
	private BuilderSetSelector setSelector;

	// Token: 0x040020A5 RID: 8357
	public List<BuilderPieceSet.BuilderPieceCategory> _includeCategories;

	// Token: 0x040020A6 RID: 8358
	[HideInInspector]
	public BuilderTable table;

	// Token: 0x040020A7 RID: 8359
	public int shelfID = -1;

	// Token: 0x040020A8 RID: 8360
	[Header("Conveyor Properties")]
	[SerializeField]
	private Transform spawnTransform;

	// Token: 0x040020A9 RID: 8361
	[SerializeField]
	private SplineContainer spline;

	// Token: 0x040020AA RID: 8362
	private float conveyorMoveSpeed = 0.2f;

	// Token: 0x040020AB RID: 8363
	private float spawnDelay = 1.5f;

	// Token: 0x040020AC RID: 8364
	private double nextSpawnTime;

	// Token: 0x040020AD RID: 8365
	private int nextPieceToSpawn;

	// Token: 0x040020AE RID: 8366
	private BuilderPieceSet currentSet;

	// Token: 0x040020AF RID: 8367
	private int loopCount;

	// Token: 0x040020B0 RID: 8368
	private List<BuilderPieceSet.PieceInfo> piecesInSet = new List<BuilderPieceSet.PieceInfo>(10);

	// Token: 0x040020B1 RID: 8369
	private Queue<int> grabbedPieceTypes;

	// Token: 0x040020B2 RID: 8370
	private Queue<int> grabbedPieceMaterials;

	// Token: 0x040020B3 RID: 8371
	private List<BuilderPiece> piecesOnConveyor = new List<BuilderPiece>(10);

	// Token: 0x040020B4 RID: 8372
	private Vector3 moveDirection;

	// Token: 0x040020B5 RID: 8373
	private bool waitForResourceChange;

	// Token: 0x040020B6 RID: 8374
	private bool initialized;

	// Token: 0x040020B7 RID: 8375
	private float splineLength = 1f;

	// Token: 0x040020B8 RID: 8376
	private int maxItemsOnSpline;

	// Token: 0x040020B9 RID: 8377
	private UnityEngine.Splines.BezierCurve _evaluateCurve;

	// Token: 0x040020BA RID: 8378
	public NativeSpline nativeSpline;

	// Token: 0x040020BB RID: 8379
	private bool shouldVerifySetSelection;
}
