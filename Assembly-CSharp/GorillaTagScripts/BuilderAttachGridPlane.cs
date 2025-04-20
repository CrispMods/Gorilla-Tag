using System;
using GorillaTagScripts.Builder;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009AC RID: 2476
	public class BuilderAttachGridPlane : MonoBehaviour
	{
		// Token: 0x06003CB4 RID: 15540 RVA: 0x00057AB8 File Offset: 0x00055CB8
		private void Awake()
		{
			if (this.center == null)
			{
				this.center = base.transform;
			}
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x00155218 File Offset: 0x00153418
		public void Setup(BuilderPiece piece, int attachIndex, float gridSize)
		{
			this.piece = piece;
			this.attachIndex = attachIndex;
			this.pieceToGridPosition = piece.transform.InverseTransformPoint(base.transform.position);
			this.pieceToGridRotation = Quaternion.Inverse(piece.transform.rotation) * base.transform.rotation;
			float num = (float)(this.width + 2) * gridSize;
			float num2 = (float)(this.length + 2) * gridSize;
			this.boundingRadius = Mathf.Sqrt(num * num + num2 * num2);
			this.connected = new bool[this.width * this.length];
			this.widthOffset = ((this.width % 2 == 0) ? (gridSize / 2f) : 0f);
			this.lengthOffset = ((this.length % 2 == 0) ? (gridSize / 2f) : 0f);
			this.gridPlaneDataIndex = -1;
			this.childPieceCount = 0;
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x00155304 File Offset: 0x00153504
		public void OnReturnToPool(BuilderPool pool)
		{
			SnapOverlap nextOverlap = this.firstOverlap;
			while (nextOverlap != null)
			{
				SnapOverlap snapOverlap = nextOverlap;
				nextOverlap = nextOverlap.nextOverlap;
				if (snapOverlap.otherPlane != null)
				{
					snapOverlap.otherPlane.RemoveSnapsWithPiece(this.piece, pool);
				}
				this.SetConnected(snapOverlap.bounds, false);
				pool.DestroySnapOverlap(snapOverlap);
			}
			int num = this.width * this.length;
			for (int i = 0; i < num; i++)
			{
				this.connected[i] = false;
			}
			this.childPieceCount = 0;
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x00155384 File Offset: 0x00153584
		public Vector3 GetGridPosition(int x, int z, float gridSize)
		{
			float num = (this.width % 2 == 0) ? (gridSize / 2f) : 0f;
			float num2 = (this.length % 2 == 0) ? (gridSize / 2f) : 0f;
			return this.center.position + this.center.rotation * new Vector3((float)x * gridSize - num, (this.male ? 0.002f : -0.002f) * gridSize, (float)z * gridSize - num2);
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x00057AD4 File Offset: 0x00055CD4
		public int GetChildCount()
		{
			return this.childPieceCount;
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x0015540C File Offset: 0x0015360C
		public void ChangeChildPieceCount(int delta)
		{
			this.childPieceCount += delta;
			if (this.piece.parentPiece == null)
			{
				return;
			}
			if (this.piece.parentAttachIndex < 0 || this.piece.parentAttachIndex >= this.piece.parentPiece.gridPlanes.Count)
			{
				return;
			}
			this.piece.parentPiece.gridPlanes[this.piece.parentAttachIndex].ChangeChildPieceCount(delta);
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x00057ADC File Offset: 0x00055CDC
		public void AddSnapOverlap(SnapOverlap newOverlap)
		{
			if (this.firstOverlap == null)
			{
				this.firstOverlap = newOverlap;
			}
			else
			{
				newOverlap.nextOverlap = this.firstOverlap;
				this.firstOverlap = newOverlap;
			}
			this.SetConnected(newOverlap.bounds, true);
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x00155494 File Offset: 0x00153694
		public void RemoveSnapsWithDifferentRoot(BuilderPiece root, BuilderPool pool)
		{
			if (this.firstOverlap == null)
			{
				return;
			}
			if (pool == null)
			{
				return;
			}
			SnapOverlap snapOverlap = null;
			SnapOverlap nextOverlap = this.firstOverlap;
			while (nextOverlap != null)
			{
				if (nextOverlap.otherPlane == null || nextOverlap.otherPlane.piece == null)
				{
					SnapOverlap snapOverlap2 = nextOverlap;
					if (snapOverlap == null)
					{
						this.firstOverlap = nextOverlap.nextOverlap;
						nextOverlap = this.firstOverlap;
					}
					else
					{
						snapOverlap.nextOverlap = nextOverlap.nextOverlap;
						nextOverlap = snapOverlap.nextOverlap;
					}
					this.SetConnected(snapOverlap2.bounds, false);
					pool.DestroySnapOverlap(snapOverlap2);
				}
				else if (root == null || nextOverlap.otherPlane.piece.GetRootPiece() != root)
				{
					SnapOverlap snapOverlap3 = nextOverlap;
					if (snapOverlap == null)
					{
						this.firstOverlap = nextOverlap.nextOverlap;
						nextOverlap = this.firstOverlap;
					}
					else
					{
						snapOverlap.nextOverlap = nextOverlap.nextOverlap;
						nextOverlap = snapOverlap.nextOverlap;
					}
					this.SetConnected(snapOverlap3.bounds, false);
					snapOverlap3.otherPlane.RemoveSnapsWithPiece(this.piece, pool);
					pool.DestroySnapOverlap(snapOverlap3);
				}
				else
				{
					snapOverlap = nextOverlap;
					nextOverlap = nextOverlap.nextOverlap;
				}
			}
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x001555AC File Offset: 0x001537AC
		public void RemoveSnapsWithPiece(BuilderPiece piece, BuilderPool pool)
		{
			if (this.firstOverlap == null)
			{
				return;
			}
			if (piece == null || pool == null)
			{
				return;
			}
			SnapOverlap snapOverlap = null;
			SnapOverlap nextOverlap = this.firstOverlap;
			while (nextOverlap != null)
			{
				if (nextOverlap.otherPlane == null || nextOverlap.otherPlane.piece == null)
				{
					SnapOverlap snapOverlap2 = nextOverlap;
					if (snapOverlap == null)
					{
						this.firstOverlap = nextOverlap.nextOverlap;
						nextOverlap = this.firstOverlap;
					}
					else
					{
						snapOverlap.nextOverlap = nextOverlap.nextOverlap;
						nextOverlap = snapOverlap.nextOverlap;
					}
					this.SetConnected(snapOverlap2.bounds, false);
					pool.DestroySnapOverlap(snapOverlap2);
				}
				else if (nextOverlap.otherPlane.piece == piece)
				{
					SnapOverlap snapOverlap3 = nextOverlap;
					if (snapOverlap == null)
					{
						this.firstOverlap = nextOverlap.nextOverlap;
						nextOverlap = this.firstOverlap;
					}
					else
					{
						snapOverlap.nextOverlap = nextOverlap.nextOverlap;
						nextOverlap = snapOverlap.nextOverlap;
					}
					this.SetConnected(snapOverlap3.bounds, false);
					pool.DestroySnapOverlap(snapOverlap3);
				}
				else
				{
					snapOverlap = nextOverlap;
					nextOverlap = nextOverlap.nextOverlap;
				}
			}
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x001556AC File Offset: 0x001538AC
		private void SetConnected(SnapBounds bounds, bool connect)
		{
			int num = this.width / 2 - ((this.width % 2 == 0) ? 1 : 0);
			int num2 = this.length / 2 - ((this.length % 2 == 0) ? 1 : 0);
			int num3 = this.connected.Length;
			for (int i = bounds.min.x; i <= bounds.max.x; i++)
			{
				for (int j = bounds.min.y; j <= bounds.max.y; j++)
				{
					int num4 = (num + i) * this.length + (j + num2);
					if (num4 >= num3 || num4 < 0)
					{
						if (this.piece != null)
						{
							int pieceId = this.piece.pieceId;
						}
						return;
					}
					this.connected[num4] = connect;
				}
			}
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x0015577C File Offset: 0x0015397C
		public bool IsConnected(SnapBounds bounds)
		{
			int num = this.width / 2 - ((this.width % 2 == 0) ? 1 : 0);
			int num2 = this.length / 2 - ((this.length % 2 == 0) ? 1 : 0);
			int num3 = this.connected.Length;
			for (int i = bounds.min.x; i <= bounds.max.x; i++)
			{
				for (int j = bounds.min.y; j <= bounds.max.y; j++)
				{
					int num4 = (num + i) * this.length + (j + num2);
					if (num4 < 0 || num4 >= num3)
					{
						if (this.piece != null)
						{
							int pieceId = this.piece.pieceId;
						}
						return false;
					}
					if (this.connected[num4])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x00155850 File Offset: 0x00153A50
		public void CalcGridOverlap(BuilderAttachGridPlane otherGridPlane, Vector3 otherPieceLocalPos, Quaternion otherPieceLocalRot, float gridSize, out Vector2Int min, out Vector2Int max)
		{
			int num = otherGridPlane.width;
			int num2 = otherGridPlane.length;
			Quaternion rotation = otherPieceLocalRot * otherGridPlane.pieceToGridRotation;
			Vector3 lossyScale = base.transform.lossyScale;
			otherPieceLocalPos.Scale(base.transform.lossyScale);
			Vector3 vector = otherPieceLocalPos + otherPieceLocalRot * otherGridPlane.pieceToGridPosition;
			if (Mathf.Abs(Vector3.Dot(rotation * Vector3.forward, Vector3.forward)) < 0.707f)
			{
				num = otherGridPlane.length;
				num2 = otherGridPlane.width;
			}
			float num3 = (num % 2 == 0) ? (gridSize / 2f) : 0f;
			float num4 = (num2 % 2 == 0) ? (gridSize / 2f) : 0f;
			float num5 = (this.width % 2 == 0) ? (gridSize / 2f) : 0f;
			float num6 = (this.length % 2 == 0) ? (gridSize / 2f) : 0f;
			float num7 = num3 - num5;
			float num8 = num4 - num6;
			int num9 = Mathf.RoundToInt((vector.x - num7) / gridSize);
			int num10 = Mathf.RoundToInt((vector.z - num8) / gridSize);
			int num11 = num9 + Mathf.FloorToInt((float)num / 2f);
			int num12 = num10 + Mathf.FloorToInt((float)num2 / 2f);
			int a = num11 - (num - 1);
			int a2 = num12 - (num2 - 1);
			int num13 = Mathf.FloorToInt((float)this.width / 2f);
			int num14 = Mathf.FloorToInt((float)this.length / 2f);
			int b = num13 - (this.width - 1);
			int b2 = num14 - (this.length - 1);
			min = new Vector2Int(Mathf.Max(a, b), Mathf.Max(a2, b2));
			max = new Vector2Int(Mathf.Min(num11, num13), Mathf.Min(num12, num14));
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x00030607 File Offset: 0x0002E807
		protected virtual void OnDrawGizmosSelected()
		{
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x00155A14 File Offset: 0x00153C14
		public bool IsAttachedToMovingGrid()
		{
			return this.piece.state == BuilderPiece.State.AttachedAndPlaced && !this.piece.isBuiltIntoTable && (this.isMoving || (!(this.piece.parentPiece == null) && this.piece.parentAttachIndex >= 0 && this.piece.parentAttachIndex < this.piece.parentPiece.gridPlanes.Count && this.piece.parentPiece.gridPlanes[this.piece.parentAttachIndex].IsAttachedToMovingGrid()));
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x00155AB8 File Offset: 0x00153CB8
		public BuilderAttachGridPlane GetMovingParentGrid()
		{
			if (this.piece.isBuiltIntoTable)
			{
				return null;
			}
			if (this.movesOnPlace && this.movingPart != null && !this.movingPart.IsAnchoredToTable())
			{
				return this;
			}
			if (this.piece.parentPiece == null)
			{
				return null;
			}
			if (this.piece.parentAttachIndex < 0 || this.piece.parentAttachIndex >= this.piece.parentPiece.gridPlanes.Count)
			{
				return null;
			}
			return this.piece.parentPiece.gridPlanes[this.piece.parentAttachIndex].GetMovingParentGrid();
		}

		// Token: 0x04003D9D RID: 15773
		public bool male;

		// Token: 0x04003D9E RID: 15774
		public Transform center;

		// Token: 0x04003D9F RID: 15775
		public int width;

		// Token: 0x04003DA0 RID: 15776
		public int length;

		// Token: 0x04003DA1 RID: 15777
		[NonSerialized]
		public int gridPlaneDataIndex;

		// Token: 0x04003DA2 RID: 15778
		[NonSerialized]
		public BuilderItem item;

		// Token: 0x04003DA3 RID: 15779
		[NonSerialized]
		public BuilderPiece piece;

		// Token: 0x04003DA4 RID: 15780
		[NonSerialized]
		public int attachIndex;

		// Token: 0x04003DA5 RID: 15781
		[NonSerialized]
		public float boundingRadius;

		// Token: 0x04003DA6 RID: 15782
		[NonSerialized]
		public Vector3 pieceToGridPosition;

		// Token: 0x04003DA7 RID: 15783
		[NonSerialized]
		public Quaternion pieceToGridRotation;

		// Token: 0x04003DA8 RID: 15784
		[NonSerialized]
		public bool[] connected;

		// Token: 0x04003DA9 RID: 15785
		[NonSerialized]
		public SnapOverlap firstOverlap;

		// Token: 0x04003DAA RID: 15786
		[NonSerialized]
		public float widthOffset;

		// Token: 0x04003DAB RID: 15787
		[NonSerialized]
		public float lengthOffset;

		// Token: 0x04003DAC RID: 15788
		private int childPieceCount;

		// Token: 0x04003DAD RID: 15789
		[HideInInspector]
		public bool isMoving;

		// Token: 0x04003DAE RID: 15790
		[HideInInspector]
		public bool movesOnPlace;

		// Token: 0x04003DAF RID: 15791
		[HideInInspector]
		public BuilderMovingPart movingPart;
	}
}
