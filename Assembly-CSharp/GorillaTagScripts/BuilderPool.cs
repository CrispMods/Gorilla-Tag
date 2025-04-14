using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x0200098D RID: 2445
	public class BuilderPool : MonoBehaviour
	{
		// Token: 0x06003BE2 RID: 15330 RVA: 0x00113D5C File Offset: 0x00111F5C
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

		// Token: 0x06003BE3 RID: 15331 RVA: 0x00113DC8 File Offset: 0x00111FC8
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

		// Token: 0x06003BE4 RID: 15332 RVA: 0x00113E30 File Offset: 0x00112030
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

		// Token: 0x06003BE5 RID: 15333 RVA: 0x00113F74 File Offset: 0x00112174
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
				BuilderPiece builderPiece = Object.Instantiate<BuilderPiece>(piecePrefab);
				builderPiece.OnCreatedByPool();
				builderPiece.gameObject.SetActive(false);
				list.Add(builderPiece);
			}
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x00114038 File Offset: 0x00112238
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

		// Token: 0x06003BE7 RID: 15335 RVA: 0x001140DC File Offset: 0x001122DC
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
				Object.Destroy(piece.gameObject);
				return;
			}
			piece.gameObject.SetActive(false);
			piece.transform.SetParent(null);
			piece.transform.SetPositionAndRotation(Vector3.up * 10000f, Quaternion.identity);
			piece.OnReturnToPool();
			list.Add(piece);
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x001141A4 File Offset: 0x001123A4
		private void AddToGlowBumpPool(int count)
		{
			if (this.bumpGlowPrefab == null)
			{
				Debug.LogError("Builderpool missing bump glow prefab");
				return;
			}
			for (int i = 0; i < count; i++)
			{
				BuilderBumpGlow builderBumpGlow = Object.Instantiate<BuilderBumpGlow>(this.bumpGlowPrefab);
				builderBumpGlow.gameObject.SetActive(false);
				this.bumpGlowPool.Add(builderBumpGlow);
			}
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x001141FC File Offset: 0x001123FC
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

		// Token: 0x06003BEA RID: 15338 RVA: 0x00114258 File Offset: 0x00112458
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

		// Token: 0x06003BEB RID: 15339 RVA: 0x001142B0 File Offset: 0x001124B0
		private void AddToSnapOverlapPool(int count)
		{
			this.snapOverlapPool.Capacity = this.snapOverlapPool.Capacity + count;
			for (int i = 0; i < count; i++)
			{
				this.snapOverlapPool.Add(new SnapOverlap());
			}
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x001142F4 File Offset: 0x001124F4
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

		// Token: 0x06003BED RID: 15341 RVA: 0x00114368 File Offset: 0x00112568
		public void DestroySnapOverlap(SnapOverlap snapOverlap)
		{
			snapOverlap.otherPlane = null;
			snapOverlap.nextOverlap = null;
			this.snapOverlapPool.Add(snapOverlap);
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x00114384 File Offset: 0x00112584
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
							Object.Destroy(builderPiece);
						}
					}
					this.piecePools[i].Clear();
				}
			}
			this.piecePoolLookup.Clear();
			foreach (BuilderBumpGlow obj in this.bumpGlowPool)
			{
				Object.Destroy(obj);
			}
			this.bumpGlowPool.Clear();
		}

		// Token: 0x04003D0C RID: 15628
		public List<List<BuilderPiece>> piecePools;

		// Token: 0x04003D0D RID: 15629
		public Dictionary<int, int> piecePoolLookup;

		// Token: 0x04003D0E RID: 15630
		[HideInInspector]
		public List<BuilderBumpGlow> bumpGlowPool;

		// Token: 0x04003D0F RID: 15631
		public BuilderBumpGlow bumpGlowPrefab;

		// Token: 0x04003D10 RID: 15632
		[HideInInspector]
		public List<SnapOverlap> snapOverlapPool;

		// Token: 0x04003D11 RID: 15633
		private const int INITIAL_POOL_SIZE = 32;

		// Token: 0x04003D12 RID: 15634
		private const int HIDDEN_PIECE_POOL_SIZE = 8;

		// Token: 0x04003D13 RID: 15635
		private BuilderFactory factory;
	}
}
