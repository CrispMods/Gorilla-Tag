using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000990 RID: 2448
	public class BuilderPool : MonoBehaviour
	{
		// Token: 0x06003BEE RID: 15342 RVA: 0x00150AF0 File Offset: 0x0014ECF0
		public void Setup(BuilderFactory factory)
		{
			this.factory = factory;
			this.piecePools = new List<List<BuilderPiece>>(512);
			this.piecePoolLookup = new Dictionary<int, int>(512);
			this.bumpGlowPool = new List<BuilderBumpGlow>(256);
			this.AddToGlowBumpPool(256);
			this.snapOverlapPool = new List<SnapOverlap>(4096);
			this.AddToSnapOverlapPool(4096);
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x00150B5C File Offset: 0x0014ED5C
		public void BuildFromShelves(List<BuilderShelf> shelves)
		{
			for (int i = 0; i < shelves.Count; i++)
			{
				BuilderShelf builderShelf = shelves[i];
				for (int j = 0; j < builderShelf.buildPieceSpawns.Count; j++)
				{
					BuilderShelf.BuildPieceSpawn buildPieceSpawn = builderShelf.buildPieceSpawns[j];
					this.AddToPool(buildPieceSpawn.buildPiecePrefab.name.GetStaticHash(), buildPieceSpawn.count);
				}
			}
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x00150BC4 File Offset: 0x0014EDC4
		public void BuildFromPieceSets()
		{
			foreach (BuilderPieceSet builderPieceSet in BuilderSetManager.instance.GetAllPieceSets())
			{
				foreach (BuilderPieceSet.BuilderPieceSubset builderPieceSubset in builderPieceSet.subsets)
				{
					int num = builderPieceSet.setName.Equals("HIDDEN") ? 8 : 32;
					foreach (BuilderPieceSet.PieceInfo pieceInfo in builderPieceSubset.pieceInfos)
					{
						int staticHash = pieceInfo.piecePrefab.name.GetStaticHash();
						int count;
						if (!this.piecePoolLookup.TryGetValue(staticHash, out count))
						{
							count = this.piecePools.Count;
							this.piecePools.Add(new List<BuilderPiece>(num));
							this.piecePoolLookup.Add(staticHash, count);
							this.AddToPool(staticHash, num);
						}
					}
				}
			}
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x00150D08 File Offset: 0x0014EF08
		private void AddToPool(int pieceType, int count)
		{
			int count2;
			if (!this.piecePoolLookup.TryGetValue(pieceType, out count2))
			{
				count2 = this.piecePools.Count;
				this.piecePools.Add(new List<BuilderPiece>(count * 8));
				this.piecePoolLookup.Add(pieceType, count2);
				Debug.LogWarningFormat("Creating Pool for piece {0} of size {1}. Is this piece not in a piece set?", new object[]
				{
					pieceType,
					count * 8
				});
			}
			BuilderPiece piecePrefab = this.factory.GetPiecePrefab(pieceType);
			if (piecePrefab == null)
			{
				return;
			}
			List<BuilderPiece> list = this.piecePools[count2];
			for (int i = 0; i < count; i++)
			{
				BuilderPiece builderPiece = UnityEngine.Object.Instantiate<BuilderPiece>(piecePrefab);
				builderPiece.OnCreatedByPool();
				builderPiece.gameObject.SetActive(false);
				list.Add(builderPiece);
			}
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x00150DCC File Offset: 0x0014EFCC
		public BuilderPiece CreatePiece(int pieceType, bool assertNotEmpty)
		{
			int count;
			if (!this.piecePoolLookup.TryGetValue(pieceType, out count))
			{
				if (assertNotEmpty)
				{
					Debug.LogErrorFormat("No Pool Found for {0} Adding 4", new object[]
					{
						pieceType
					});
				}
				count = this.piecePools.Count;
				this.AddToPool(pieceType, 4);
			}
			List<BuilderPiece> list = this.piecePools[count];
			if (list.Count == 0)
			{
				if (assertNotEmpty)
				{
					Debug.LogErrorFormat("Pool for {0} is Empty Adding 4", new object[]
					{
						pieceType
					});
				}
				this.AddToPool(pieceType, 4);
			}
			BuilderPiece result = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return result;
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x00150E70 File Offset: 0x0014F070
		public void DestroyPiece(BuilderPiece piece)
		{
			if (piece == null)
			{
				Debug.LogError("Why is a null piece being destroyed");
				return;
			}
			int index;
			if (!this.piecePoolLookup.TryGetValue(piece.pieceType, out index))
			{
				Debug.LogErrorFormat("No Pool Found for {0} Cannot return to pool", new object[]
				{
					piece.pieceType
				});
				return;
			}
			List<BuilderPiece> list = this.piecePools[index];
			if (list.Count == list.Capacity)
			{
				piece.OnReturnToPool();
				UnityEngine.Object.Destroy(piece.gameObject);
				return;
			}
			piece.gameObject.SetActive(false);
			piece.transform.SetParent(null);
			piece.transform.SetPositionAndRotation(Vector3.up * 10000f, Quaternion.identity);
			piece.OnReturnToPool();
			list.Add(piece);
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x00150F38 File Offset: 0x0014F138
		private void AddToGlowBumpPool(int count)
		{
			if (this.bumpGlowPrefab == null)
			{
				Debug.LogError("Builderpool missing bump glow prefab");
				return;
			}
			for (int i = 0; i < count; i++)
			{
				BuilderBumpGlow builderBumpGlow = UnityEngine.Object.Instantiate<BuilderBumpGlow>(this.bumpGlowPrefab);
				builderBumpGlow.gameObject.SetActive(false);
				this.bumpGlowPool.Add(builderBumpGlow);
			}
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x00150F90 File Offset: 0x0014F190
		public BuilderBumpGlow CreateGlowBump()
		{
			if (this.bumpGlowPool.Count == 0)
			{
				Debug.LogError(" Glow bump Pool is Empty Adding 4");
				this.AddToGlowBumpPool(4);
			}
			BuilderBumpGlow result = this.bumpGlowPool[this.bumpGlowPool.Count - 1];
			this.bumpGlowPool.RemoveAt(this.bumpGlowPool.Count - 1);
			return result;
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x00150FEC File Offset: 0x0014F1EC
		public void DestroyBumpGlow(BuilderBumpGlow bump)
		{
			if (bump == null)
			{
				Debug.LogError("Returning null glow bump to pool");
				return;
			}
			bump.gameObject.SetActive(false);
			bump.transform.SetPositionAndRotation(Vector3.up * 10000f, Quaternion.identity);
			this.bumpGlowPool.Add(bump);
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x00151044 File Offset: 0x0014F244
		private void AddToSnapOverlapPool(int count)
		{
			this.snapOverlapPool.Capacity = this.snapOverlapPool.Capacity + count;
			for (int i = 0; i < count; i++)
			{
				this.snapOverlapPool.Add(new SnapOverlap());
			}
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x00151088 File Offset: 0x0014F288
		public SnapOverlap CreateSnapOverlap(BuilderAttachGridPlane otherPlane, SnapBounds bounds)
		{
			if (this.snapOverlapPool.Count == 0)
			{
				Debug.LogError("Snap Overlap Pool is Empty Adding 1024");
				this.AddToSnapOverlapPool(1024);
			}
			SnapOverlap snapOverlap = this.snapOverlapPool[this.snapOverlapPool.Count - 1];
			this.snapOverlapPool.RemoveAt(this.snapOverlapPool.Count - 1);
			snapOverlap.otherPlane = otherPlane;
			snapOverlap.bounds = bounds;
			snapOverlap.nextOverlap = null;
			return snapOverlap;
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x00056482 File Offset: 0x00054682
		public void DestroySnapOverlap(SnapOverlap snapOverlap)
		{
			snapOverlap.otherPlane = null;
			snapOverlap.nextOverlap = null;
			this.snapOverlapPool.Add(snapOverlap);
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x001510FC File Offset: 0x0014F2FC
		private void OnDestroy()
		{
			for (int i = 0; i < this.piecePools.Count; i++)
			{
				if (this.piecePools[i] != null)
				{
					foreach (BuilderPiece builderPiece in this.piecePools[i])
					{
						if (builderPiece != null)
						{
							UnityEngine.Object.Destroy(builderPiece);
						}
					}
					this.piecePools[i].Clear();
				}
			}
			this.piecePoolLookup.Clear();
			foreach (BuilderBumpGlow obj in this.bumpGlowPool)
			{
				UnityEngine.Object.Destroy(obj);
			}
			this.bumpGlowPool.Clear();
		}

		// Token: 0x04003D1E RID: 15646
		public List<List<BuilderPiece>> piecePools;

		// Token: 0x04003D1F RID: 15647
		public Dictionary<int, int> piecePoolLookup;

		// Token: 0x04003D20 RID: 15648
		[HideInInspector]
		public List<BuilderBumpGlow> bumpGlowPool;

		// Token: 0x04003D21 RID: 15649
		public BuilderBumpGlow bumpGlowPrefab;

		// Token: 0x04003D22 RID: 15650
		[HideInInspector]
		public List<SnapOverlap> snapOverlapPool;

		// Token: 0x04003D23 RID: 15651
		private const int INITIAL_POOL_SIZE = 32;

		// Token: 0x04003D24 RID: 15652
		private const int HIDDEN_PIECE_POOL_SIZE = 8;

		// Token: 0x04003D25 RID: 15653
		private BuilderFactory factory;
	}
}
