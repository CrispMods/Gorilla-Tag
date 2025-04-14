using System;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace GorillaTagScripts
{
	// Token: 0x020009A3 RID: 2467
	[BurstCompile]
	internal struct FindNearbyPiecesJob : IJobParallelForTransform
	{
		// Token: 0x06003CEE RID: 15598 RVA: 0x0011EED8 File Offset: 0x0011D0D8
		public void Execute(int index, TransformAccess transform)
		{
			if (!transform.isValid)
			{
				return;
			}
			this.CheckGridPlane(index, this.leftPieceInHandIndex, transform, this.leftHandPos, true, this.leftHandGridPlanes);
			this.CheckGridPlane(index, this.rightPieceInHandIndex, transform, this.rightHandPos, false, this.rightHandGridPlanes);
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x0011EF28 File Offset: 0x0011D128
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

		// Token: 0x06003CF0 RID: 15600 RVA: 0x0011EFBC File Offset: 0x0011D1BC
		public bool CanPiecesPotentiallySnap(int localActorNumber, int pieceInHandIndex, int attachToPieceIndex, int attachToPieceRootIndex, int requestedParentPieceIndex, bool isLeft)
		{
			return this.CanPlayerAttachToRootPiece(localActorNumber, attachToPieceRootIndex, isLeft) && (requestedParentPieceIndex == -1 || pieceInHandIndex != this.GetRootPieceIndex(requestedParentPieceIndex));
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x0011EFE8 File Offset: 0x0011D1E8
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

		// Token: 0x06003CF2 RID: 15602 RVA: 0x0011F0D8 File Offset: 0x0011D2D8
		public bool CanPlayerAttachToPlot(int privatePlotIndex, int actorNumber)
		{
			BuilderPrivatePlotData builderPrivatePlotData = this.privatePlotData[privatePlotIndex];
			return (builderPrivatePlotData.plotState == BuilderPiecePrivatePlot.PlotState.Occupied && builderPrivatePlotData.ownerActorNumber == actorNumber) || (builderPrivatePlotData.plotState == BuilderPiecePrivatePlot.PlotState.Vacant && this.localPlayerPlotIndex < 0);
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x0011F11C File Offset: 0x0011D31C
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

		// Token: 0x06003CF4 RID: 15604 RVA: 0x0011F158 File Offset: 0x0011D358
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

		// Token: 0x06003CF5 RID: 15605 RVA: 0x0011F1BC File Offset: 0x0011D3BC
		private int GetRootPieceIndex(int pieceIndex)
		{
			int num = pieceIndex;
			while (num != -1 && this.pieceData[num].parentPieceIndex != -1 && !this.pieceData[this.pieceData[num].parentPieceIndex].isBuiltIntoTable)
			{
				num = this.pieceData[num].parentPieceIndex;
			}
			return num;
		}

		// Token: 0x04003E4E RID: 15950
		[ReadOnly]
		public float distanceThreshSq;

		// Token: 0x04003E4F RID: 15951
		[ReadOnly]
		public Vector3 leftHandPos;

		// Token: 0x04003E50 RID: 15952
		[ReadOnly]
		public int leftPieceInHandIndex;

		// Token: 0x04003E51 RID: 15953
		[ReadOnly]
		public Vector3 rightHandPos;

		// Token: 0x04003E52 RID: 15954
		[ReadOnly]
		public int rightPieceInHandIndex;

		// Token: 0x04003E53 RID: 15955
		[ReadOnly]
		public int localPlayerPlotIndex;

		// Token: 0x04003E54 RID: 15956
		[ReadOnly]
		public int localPlayerActorNumber;

		// Token: 0x04003E55 RID: 15957
		[ReadOnly]
		public NativeArray<BuilderPieceData> pieceData;

		// Token: 0x04003E56 RID: 15958
		[ReadOnly]
		public NativeArray<BuilderGridPlaneData> gridPlaneData;

		// Token: 0x04003E57 RID: 15959
		[ReadOnly]
		public NativeArray<BuilderPrivatePlotData> privatePlotData;

		// Token: 0x04003E58 RID: 15960
		[ReadOnly]
		public NativeArray<BuilderPlayerData> playerData;

		// Token: 0x04003E59 RID: 15961
		public NativeList<BuilderGridPlaneData>.ParallelWriter leftHandGridPlanes;

		// Token: 0x04003E5A RID: 15962
		public NativeList<BuilderGridPlaneData>.ParallelWriter rightHandGridPlanes;
	}
}
