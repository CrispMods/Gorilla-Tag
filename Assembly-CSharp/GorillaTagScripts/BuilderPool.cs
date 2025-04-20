using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009B3 RID: 2483
	public class BuilderPool : MonoBehaviour
	{
		// Token: 0x06003CFA RID: 15610 RVA: 0x00156AD8 File Offset: 0x00154CD8
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

		// Token: 0x06003CFB RID: 15611 RVA: 0x00156B44 File Offset: 0x00154D44
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

		// Token: 0x06003CFC RID: 15612 RVA: 0x00156BAC File Offset: 0x00154DAC
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

		// Token: 0x06003CFD RID: 15613 RVA: 0x00156CF0 File Offset: 0x00154EF0
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

		// Token: 0x06003CFE RID: 15614 RVA: 0x00156DB4 File Offset: 0x00154FB4
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

		// Token: 0x06003CFF RID: 15615 RVA: 0x00156E58 File Offset: 0x00155058
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

		// Token: 0x06003D00 RID: 15616 RVA: 0x00156F20 File Offset: 0x00155120
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

		// Token: 0x06003D01 RID: 15617 RVA: 0x00156F78 File Offset: 0x00155178
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

		// Token: 0x06003D02 RID: 15618 RVA: 0x00156FD4 File Offset: 0x001551D4
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

		// Token: 0x06003D03 RID: 15619 RVA: 0x0015702C File Offset: 0x0015522C
		private void AddToSnapOverlapPool(int count)
		{
			this.snapOverlapPool.Capacity = this.snapOverlapPool.Capacity + count;
			for (int i = 0; i < count; i++)
			{
				this.snapOverlapPool.Add(new SnapOverlap());
			}
		}

		// Token: 0x06003D04 RID: 15620 RVA: 0x00157070 File Offset: 0x00155270
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

		// Token: 0x06003D05 RID: 15621 RVA: 0x00057D19 File Offset: 0x00055F19
		public void DestroySnapOverlap(SnapOverlap snapOverlap)
		{
			snapOverlap.otherPlane = null;
			snapOverlap.nextOverlap = null;
			this.snapOverlapPool.Add(snapOverlap);
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x001570E4 File Offset: 0x001552E4
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

		// Token: 0x04003DE6 RID: 15846
		public List<List<BuilderPiece>> piecePools;

		// Token: 0x04003DE7 RID: 15847
		public Dictionary<int, int> piecePoolLookup;

		// Token: 0x04003DE8 RID: 15848
		[HideInInspector]
		public List<BuilderBumpGlow> bumpGlowPool;

		// Token: 0x04003DE9 RID: 15849
		public BuilderBumpGlow bumpGlowPrefab;

		// Token: 0x04003DEA RID: 15850
		[HideInInspector]
		public List<SnapOverlap> snapOverlapPool;

		// Token: 0x04003DEB RID: 15851
		private const int INITIAL_POOL_SIZE = 32;

		// Token: 0x04003DEC RID: 15852
		private const int HIDDEN_PIECE_POOL_SIZE = 8;

		// Token: 0x04003DED RID: 15853
		private BuilderFactory factory;
	}
}
