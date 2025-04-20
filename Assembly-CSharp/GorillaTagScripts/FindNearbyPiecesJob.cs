using System;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace GorillaTagScripts
{
	// Token: 0x020009C9 RID: 2505
	[BurstCompile]
	internal struct FindNearbyPiecesJob : IJobParallelForTransform
	{
		// Token: 0x06003E06 RID: 15878 RVA: 0x00161310 File Offset: 0x0015F510
		public void Execute(int index, TransformAccess transform)
		{
			if (!transform.isValid)
			{
				return;
			}
			this.CheckGridPlane(index, this.leftPieceInHandIndex, transform, this.leftHandPos, true, this.leftHandGridPlanes);
			this.CheckGridPlane(index, this.rightPieceInHandIndex, transform, this.rightHandPos, false, this.rightHandGridPlanes);
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x00161360 File Offset: 0x0015F560
		private void CheckGridPlane(int gridPlaneIndex, int handPieceIndex, TransformAccess transform, Vector3 handPos, bool isLeft, NativeList<BuilderGridPlaneData>.ParallelWriter checkGridPlanes)
		{
			if (handPieceIndex < 0)
			{
				return;
			}
			if ((transform.position - handPos).sqrMagnitude > this.distanceThreshSq)
			{
				return;
			}
			BuilderGridPlaneData builderGridPlaneData = this.gridPlaneData[gridPlaneIndex];
			int pieceIndex = builderGridPlaneData.pieceIndex;
			int rootPieceIndex = this.GetRootPieceIndex(pieceIndex);
			if (rootPieceIndex == handPieceIndex)
			{
				return;
			}
			if (!this.CanPiecesPotentiallySnap(this.localPlayerActorNumber, handPieceIndex, pieceIndex, rootPieceIndex, this.pieceData[pieceIndex].requestedParentPieceIndex, isLeft))
			{
				return;
			}
			transform.GetPositionAndRotation(out builderGridPlaneData.position, out builderGridPlaneData.rotation);
			checkGridPlanes.AddNoResize(builderGridPlaneData);
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x00058653 File Offset: 0x00056853
		public bool CanPiecesPotentiallySnap(int localActorNumber, int pieceInHandIndex, int attachToPieceIndex, int attachToPieceRootIndex, int requestedParentPieceIndex, bool isLeft)
		{
			return this.CanPlayerAttachToRootPiece(localActorNumber, attachToPieceRootIndex, isLeft) && (requestedParentPieceIndex == -1 || pieceInHandIndex != this.GetRootPieceIndex(requestedParentPieceIndex));
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x001613F4 File Offset: 0x0015F5F4
		public bool CanPlayerAttachToRootPiece(int playerActorNumber, int attachToPieceRootIndex, bool isLeft)
		{
			BuilderPieceData builderPieceData = this.pieceData[attachToPieceRootIndex];
			if (builderPieceData.state != BuilderPiece.State.AttachedAndPlaced && builderPieceData.privatePlotIndex < 0 && builderPieceData.state != BuilderPiece.State.AttachedToArm)
			{
				return true;
			}
			int attachedBuiltInPiece = this.GetAttachedBuiltInPiece(attachToPieceRootIndex);
			if (attachedBuiltInPiece == -1)
			{
				return true;
			}
			BuilderPieceData builderPieceData2 = this.pieceData[attachedBuiltInPiece];
			if (builderPieceData2.privatePlotIndex < 0 && !builderPieceData2.isArmPiece)
			{
				return true;
			}
			if (builderPieceData2.isArmPiece)
			{
				if (builderPieceData2.heldByActorNumber == playerActorNumber)
				{
					int playerIndex = this.GetPlayerIndex(playerActorNumber);
					return playerIndex >= 0 && this.playerData[playerIndex].scale >= 1f;
				}
				return false;
			}
			else
			{
				if (builderPieceData2.privatePlotIndex < 0)
				{
					return true;
				}
				if (!this.CanPlayerAttachToPlot(builderPieceData2.privatePlotIndex, playerActorNumber))
				{
					return false;
				}
				if (!isLeft)
				{
					return this.privatePlotData[builderPieceData2.privatePlotIndex].isUnderCapacityRight;
				}
				return this.privatePlotData[builderPieceData2.privatePlotIndex].isUnderCapacityLeft;
			}
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x001614E4 File Offset: 0x0015F6E4
		public bool CanPlayerAttachToPlot(int privatePlotIndex, int actorNumber)
		{
			BuilderPrivatePlotData builderPrivatePlotData = this.privatePlotData[privatePlotIndex];
			return (builderPrivatePlotData.plotState == BuilderPiecePrivatePlot.PlotState.Occupied && builderPrivatePlotData.ownerActorNumber == actorNumber) || (builderPrivatePlotData.plotState == BuilderPiecePrivatePlot.PlotState.Vacant && this.localPlayerPlotIndex < 0);
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x00161528 File Offset: 0x0015F728
		private int GetPlayerIndex(int playerActorNumber)
		{
			for (int i = 0; i < this.playerData.Length; i++)
			{
				if (this.playerData[i].playerActorNumber == playerActorNumber)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x00161564 File Offset: 0x0015F764
		public int GetAttachedBuiltInPiece(int pieceIndex)
		{
			BuilderPieceData builderPieceData = this.pieceData[pieceIndex];
			if (builderPieceData.isBuiltIntoTable)
			{
				return pieceIndex;
			}
			if (builderPieceData.state != BuilderPiece.State.AttachedAndPlaced)
			{
				return -1;
			}
			int num = this.GetRootPieceIndex(pieceIndex);
			int parentPieceIndex = this.pieceData[num].parentPieceIndex;
			if (parentPieceIndex != -1)
			{
				num = parentPieceIndex;
			}
			if (this.pieceData[num].isBuiltIntoTable)
			{
				return num;
			}
			return -1;
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x001615C8 File Offset: 0x0015F7C8
		private int GetRootPieceIndex(int pieceIndex)
		{
			int num = pieceIndex;
			while (num != -1 && this.pieceData[num].parentPieceIndex != -1 && !this.pieceData[this.pieceData[num].parentPieceIndex].isBuiltIntoTable)
			{
				num = this.pieceData[num].parentPieceIndex;
			}
			return num;
		}

		// Token: 0x04003F28 RID: 16168
		[ReadOnly]
		public float distanceThreshSq;

		// Token: 0x04003F29 RID: 16169
		[ReadOnly]
		public Vector3 leftHandPos;

		// Token: 0x04003F2A RID: 16170
		[ReadOnly]
		public int leftPieceInHandIndex;

		// Token: 0x04003F2B RID: 16171
		[ReadOnly]
		public Vector3 rightHandPos;

		// Token: 0x04003F2C RID: 16172
		[ReadOnly]
		public int rightPieceInHandIndex;

		// Token: 0x04003F2D RID: 16173
		[ReadOnly]
		public int localPlayerPlotIndex;

		// Token: 0x04003F2E RID: 16174
		[ReadOnly]
		public int localPlayerActorNumber;

		// Token: 0x04003F2F RID: 16175
		[ReadOnly]
		public NativeArray<BuilderPieceData> pieceData;

		// Token: 0x04003F30 RID: 16176
		[ReadOnly]
		public NativeArray<BuilderGridPlaneData> gridPlaneData;

		// Token: 0x04003F31 RID: 16177
		[ReadOnly]
		public NativeArray<BuilderPrivatePlotData> privatePlotData;

		// Token: 0x04003F32 RID: 16178
		[ReadOnly]
		public NativeArray<BuilderPlayerData> playerData;

		// Token: 0x04003F33 RID: 16179
		public NativeList<BuilderGridPlaneData>.ParallelWriter leftHandGridPlanes;

		// Token: 0x04003F34 RID: 16180
		public NativeList<BuilderGridPlaneData>.ParallelWriter rightHandGridPlanes;
	}
}
