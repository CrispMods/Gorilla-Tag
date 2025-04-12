using System;
using System.Collections.Generic;
using GorillaTagScripts;
using Photon.Pun;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

// Token: 0x020004BA RID: 1210
public class BuilderConveyor : MonoBehaviour
{
	// Token: 0x06001D42 RID: 7490 RVA: 0x00043049 File Offset: 0x00041249
	private void Start()
	{
		this.InitIfNeeded();
	}

	// Token: 0x06001D43 RID: 7491 RVA: 0x00043049 File Offset: 0x00041249
	public void Setup()
	{
		this.InitIfNeeded();
	}

	// Token: 0x06001D44 RID: 7492 RVA: 0x000DEB3C File Offset: 0x000DCD3C
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

	// Token: 0x06001D45 RID: 7493 RVA: 0x00043051 File Offset: 0x00041251
	public int GetMaxItemsOnConveyor()
	{
		return Mathf.RoundToInt(this.splineLength / (this.conveyorMoveSpeed * this.spawnDelay)) + 5;
	}

	// Token: 0x06001D46 RID: 7494 RVA: 0x0004306E File Offset: 0x0004126E
	public float GetFrameMovement()
	{
		return this.conveyorMoveSpeed / this.splineLength;
	}

	// Token: 0x06001D47 RID: 7495 RVA: 0x0004307D File Offset: 0x0004127D
	private void OnDestroy()
	{
		if (this.setSelector != null)
		{
			this.setSelector.OnSelectedSet.RemoveListener(new UnityAction<int>(this.OnSelectedSetChange));
		}
		this.nativeSpline.Dispose();
	}

	// Token: 0x06001D48 RID: 7496 RVA: 0x000430B4 File Offset: 0x000412B4
	public void OnSelectedSetChange(int setId)
	{
		if (this.table.GetTableState() != BuilderTable.TableState.Ready)
		{
			return;
		}
		this.table.RequestShelfSelection(this.shelfID, setId, true);
	}

	// Token: 0x06001D49 RID: 7497 RVA: 0x000DECB0 File Offset: 0x000DCEB0
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

	// Token: 0x06001D4A RID: 7498 RVA: 0x000430D8 File Offset: 0x000412D8
	public int GetSelectedSetID()
	{
		return this.setSelector.GetSelectedSet().GetIntIdentifier();
	}

	// Token: 0x06001D4B RID: 7499 RVA: 0x000DED5C File Offset: 0x000DCF5C
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

	// Token: 0x06001D4C RID: 7500 RVA: 0x000DEE00 File Offset: 0x000DD000
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

	// Token: 0x06001D4D RID: 7501 RVA: 0x000DEE80 File Offset: 0x000DD080
	private Vector3 EvaluateSpline(float t)
	{
		float t2;
		this._evaluateCurve = this.nativeSpline.GetCurve(this.nativeSpline.SplineToCurveT(t, out t2));
		return CurveUtility.EvaluatePosition(this._evaluateCurve, t2);
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x000DEEC0 File Offset: 0x000DD0C0
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

	// Token: 0x06001D4F RID: 7503 RVA: 0x000430EA File Offset: 0x000412EA
	public void VerifySetSelection()
	{
		this.shouldVerifySetSelection = true;
	}

	// Token: 0x06001D50 RID: 7504 RVA: 0x000430F3 File Offset: 0x000412F3
	public void OnAvailableResourcesChange()
	{
		this.waitForResourceChange = false;
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x000430FC File Offset: 0x000412FC
	public Transform GetSpawnTransform()
	{
		return this.spawnTransform;
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x000DEF58 File Offset: 0x000DD158
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

	// Token: 0x06001D53 RID: 7507 RVA: 0x00043104 File Offset: 0x00041304
	public void OnShelfPieceRecycled(BuilderPiece piece)
	{
		this.piecesOnConveyor.Remove(piece);
		if (piece != null)
		{
			this.table.conveyorManager.RemovePieceFromJob(piece);
		}
	}

	// Token: 0x06001D54 RID: 7508 RVA: 0x0004312D File Offset: 0x0004132D
	public void OnClearTable()
	{
		this.piecesOnConveyor.Clear();
		this.grabbedPieceTypes.Clear();
		this.grabbedPieceMaterials.Clear();
	}

	// Token: 0x06001D55 RID: 7509 RVA: 0x000DF034 File Offset: 0x000DD234
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

	// Token: 0x06001D56 RID: 7510 RVA: 0x000DF0EC File Offset: 0x000DD2EC
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

	// Token: 0x06001D57 RID: 7511 RVA: 0x000DF11C File Offset: 0x000DD31C
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

	// Token: 0x06001D58 RID: 7512 RVA: 0x000DF2A0 File Offset: 0x000DD4A0
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

	// Token: 0x04002052 RID: 8274
	[Header("Set Selection")]
	[SerializeField]
	private BuilderSetSelector setSelector;

	// Token: 0x04002053 RID: 8275
	public List<BuilderPieceSet.BuilderPieceCategory> _includeCategories;

	// Token: 0x04002054 RID: 8276
	[HideInInspector]
	public BuilderTable table;

	// Token: 0x04002055 RID: 8277
	public int shelfID = -1;

	// Token: 0x04002056 RID: 8278
	[Header("Conveyor Properties")]
	[SerializeField]
	private Transform spawnTransform;

	// Token: 0x04002057 RID: 8279
	[SerializeField]
	private SplineContainer spline;

	// Token: 0x04002058 RID: 8280
	private float conveyorMoveSpeed = 0.2f;

	// Token: 0x04002059 RID: 8281
	private float spawnDelay = 1.5f;

	// Token: 0x0400205A RID: 8282
	private double nextSpawnTime;

	// Token: 0x0400205B RID: 8283
	private int nextPieceToSpawn;

	// Token: 0x0400205C RID: 8284
	private BuilderPieceSet currentSet;

	// Token: 0x0400205D RID: 8285
	private int loopCount;

	// Token: 0x0400205E RID: 8286
	private List<BuilderPieceSet.PieceInfo> piecesInSet = new List<BuilderPieceSet.PieceInfo>(10);

	// Token: 0x0400205F RID: 8287
	private Queue<int> grabbedPieceTypes;

	// Token: 0x04002060 RID: 8288
	private Queue<int> grabbedPieceMaterials;

	// Token: 0x04002061 RID: 8289
	private List<BuilderPiece> piecesOnConveyor = new List<BuilderPiece>(10);

	// Token: 0x04002062 RID: 8290
	private Vector3 moveDirection;

	// Token: 0x04002063 RID: 8291
	private bool waitForResourceChange;

	// Token: 0x04002064 RID: 8292
	private bool initialized;

	// Token: 0x04002065 RID: 8293
	private float splineLength = 1f;

	// Token: 0x04002066 RID: 8294
	private int maxItemsOnSpline;

	// Token: 0x04002067 RID: 8295
	private UnityEngine.Splines.BezierCurve _evaluateCurve;

	// Token: 0x04002068 RID: 8296
	public NativeSpline nativeSpline;

	// Token: 0x04002069 RID: 8297
	private bool shouldVerifySetSelection;
}
