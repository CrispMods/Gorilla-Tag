using System;
using GorillaTagScripts.Builder;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000986 RID: 2438
	public class BuilderAttachGridPlane : MonoBehaviour
	{
		// Token: 0x06003B9C RID: 15260 RVA: 0x0011223A File Offset: 0x0011043A
		private void Awake()
		{
			if (this.center == null)
			{
				this.center = base.transform;
			}
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x00112258 File Offset: 0x00110458
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

		// Token: 0x06003B9E RID: 15262 RVA: 0x00112344 File Offset: 0x00110544
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

		// Token: 0x06003B9F RID: 15263 RVA: 0x001123C4 File Offset: 0x001105C4
		public Vector3 GetGridPosition(int x, int z, float gridSize)
		{
			float num = (this.width % 2 == 0) ? (gridSize / 2f) : 0f;
			float num2 = (this.length % 2 == 0) ? (gridSize / 2f) : 0f;
			return this.center.position + this.center.rotation * new Vector3((float)x * gridSize - num, (this.male ? 0.002f : -0.002f) * gridSize, (float)z * gridSize - num2);
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x0011244A File Offset: 0x0011064A
		public int GetChildCount()
		{
			return this.childPieceCount;
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x00112454 File Offset: 0x00110654
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

		// Token: 0x06003BA2 RID: 15266 RVA: 0x001124DA File Offset: 0x001106DA
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

		// Token: 0x06003BA3 RID: 15267 RVA: 0x00112510 File Offset: 0x00110710
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

		// Token: 0x06003BA4 RID: 15268 RVA: 0x00112628 File Offset: 0x00110828
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

		// Token: 0x06003BA5 RID: 15269 RVA: 0x00112728 File Offset: 0x00110928
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

		// Token: 0x06003BA6 RID: 15270 RVA: 0x001127F8 File Offset: 0x001109F8
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

		// Token: 0x06003BA7 RID: 15271 RVA: 0x001128CC File Offset: 0x00110ACC
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

		// Token: 0x06003BA8 RID: 15272 RVA: 0x000023F4 File Offset: 0x000005F4
		protected virtual void OnDrawGizmosSelected()
		{
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x00112A90 File Offset: 0x00110C90
		public bool IsAttachedToMovingGrid()
		{
			return this.piece.state == BuilderPiece.State.AttachedAndPlaced && !this.piece.isBuiltIntoTable && (this.isMoving || (!(this.piece.parentPiece == null) && this.piece.parentAttachIndex >= 0 && this.piece.parentAttachIndex < this.piece.parentPiece.gridPlanes.Count && this.piece.parentPiece.gridPlanes[this.piece.parentAttachIndex].IsAttachedToMovingGrid()));
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x00112B34 File Offset: 0x00110D34
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

		// Token: 0x04003CC3 RID: 15555
		public bool male;

		// Token: 0x04003CC4 RID: 15556
		public Transform center;

		// Token: 0x04003CC5 RID: 15557
		public int width;

		// Token: 0x04003CC6 RID: 15558
		public int length;

		// Token: 0x04003CC7 RID: 15559
		[NonSerialized]
		public int gridPlaneDataIndex;

		// Token: 0x04003CC8 RID: 15560
		[NonSerialized]
		public BuilderItem item;

		// Token: 0x04003CC9 RID: 15561
		[NonSerialized]
		public BuilderPiece piece;

		// Token: 0x04003CCA RID: 15562
		[NonSerialized]
		public int attachIndex;

		// Token: 0x04003CCB RID: 15563
		[NonSerialized]
		public float boundingRadius;

		// Token: 0x04003CCC RID: 15564
		[NonSerialized]
		public Vector3 pieceToGridPosition;

		// Token: 0x04003CCD RID: 15565
		[NonSerialized]
		public Quaternion pieceToGridRotation;

		// Token: 0x04003CCE RID: 15566
		[NonSerialized]
		public bool[] connected;

		// Token: 0x04003CCF RID: 15567
		[NonSerialized]
		public SnapOverlap firstOverlap;

		// Token: 0x04003CD0 RID: 15568
		[NonSerialized]
		public float widthOffset;

		// Token: 0x04003CD1 RID: 15569
		[NonSerialized]
		public float lengthOffset;

		// Token: 0x04003CD2 RID: 15570
		private int childPieceCount;

		// Token: 0x04003CD3 RID: 15571
		[HideInInspector]
		public bool isMoving;

		// Token: 0x04003CD4 RID: 15572
		[HideInInspector]
		public bool movesOnPlace;

		// Token: 0x04003CD5 RID: 15573
		[HideInInspector]
		public BuilderMovingPart movingPart;
	}
}
