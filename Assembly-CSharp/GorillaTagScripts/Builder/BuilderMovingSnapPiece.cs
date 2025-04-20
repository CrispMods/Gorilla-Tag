using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A26 RID: 2598
	public class BuilderMovingSnapPiece : MonoBehaviour, IBuilderPieceComponent, IBuilderPieceFunctional
	{
		// Token: 0x0600410F RID: 16655 RVA: 0x0016F6CC File Offset: 0x0016D8CC
		private void Awake()
		{
			this.myPiece = base.GetComponent<BuilderPiece>();
			if (this.myPiece == null)
			{
				Debug.LogWarning("Missing BuilderPiece component " + base.gameObject.name);
			}
			foreach (BuilderMovingPart builderMovingPart in this.MovingParts)
			{
				builderMovingPart.myPiece = this.myPiece;
			}
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x0016F758 File Offset: 0x0016D958
		public int GetTimeOffset()
		{
			if (this.myPiece.state != BuilderPiece.State.AttachedAndPlaced)
			{
				return 0;
			}
			foreach (BuilderMovingPart builderMovingPart in this.MovingParts)
			{
				if (!builderMovingPart.IsAnchoredToTable())
				{
					return builderMovingPart.GetTimeOffsetMS();
				}
			}
			return 0;
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPieceCreate(int pieceType, int pieceId)
		{
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x0016F7C8 File Offset: 0x0016D9C8
		public void OnPieceDestroy()
		{
			foreach (BuilderMovingPart builderMovingPart in this.MovingParts)
			{
				builderMovingPart.OnPieceDestroy();
			}
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x0016F818 File Offset: 0x0016DA18
		public void OnPiecePlacementDeserialized()
		{
			foreach (BuilderMovingPart builderMovingPart in this.MovingParts)
			{
				builderMovingPart.InitMovingGrid();
				builderMovingPart.SetMoving(false);
				if (this.myPiece.functionalPieceState == 0 && !builderMovingPart.IsAnchoredToTable())
				{
					this.currentPauseNode = builderMovingPart.GetStartNode();
				}
			}
			this.moving = false;
			if (!this.activated)
			{
				BuilderTable.instance.RegisterFunctionalPiece(this);
				BuilderTable.instance.RegisterFunctionalPieceFixedUpdate(this);
				this.activated = true;
			}
			this.OnStateChanged(this.myPiece.functionalPieceState, NetworkSystem.Instance.MasterClient, this.myPiece.activatedTimeStamp);
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x0016F8E4 File Offset: 0x0016DAE4
		public void OnPieceActivate()
		{
			if (BuilderTable.instance.GetTableState() != BuilderTable.TableState.Ready && BuilderTable.instance.GetTableState() != BuilderTable.TableState.ExecuteQueuedCommands)
			{
				return;
			}
			if (!this.activated)
			{
				BuilderTable.instance.RegisterFunctionalPiece(this);
				BuilderTable.instance.RegisterFunctionalPieceFixedUpdate(this);
				this.activated = true;
			}
			foreach (BuilderMovingPart builderMovingPart in this.MovingParts)
			{
				builderMovingPart.InitMovingGrid();
				if (!builderMovingPart.IsAnchoredToTable())
				{
					int num = 0;
					foreach (BuilderAttachGridPlane builderAttachGridPlane in builderMovingPart.myGridPlanes)
					{
						num += builderAttachGridPlane.GetChildCount();
					}
					if (num <= 5)
					{
						this.currentPauseNode = builderMovingPart.GetStartNode();
						if (this.myPiece.functionalPieceState > 0 && (int)this.myPiece.functionalPieceState < BuilderMovingPart.NUM_PAUSE_NODES * 2 + 1)
						{
							this.currentPauseNode = this.myPiece.functionalPieceState - 1;
						}
						this.myPiece.SetFunctionalPieceState(0, NetworkSystem.Instance.MasterClient, this.myPiece.activatedTimeStamp);
					}
					else
					{
						this.currentPauseNode = builderMovingPart.GetStartNode();
						if (this.myPiece.functionalPieceState > 0 && (int)this.myPiece.functionalPieceState < BuilderMovingPart.NUM_PAUSE_NODES * 2 + 1)
						{
							this.currentPauseNode = this.myPiece.functionalPieceState - 1;
						}
						this.myPiece.SetFunctionalPieceState(this.currentPauseNode + 1, NetworkSystem.Instance.MasterClient, this.myPiece.activatedTimeStamp);
					}
				}
			}
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x0016FA98 File Offset: 0x0016DC98
		public void OnPieceDeactivate()
		{
			BuilderTable.instance.UnregisterFunctionalPiece(this);
			BuilderTable.instance.UnregisterFunctionalPieceFixedUpdate(this);
			this.myPiece.functionalPieceState = 0;
			this.moving = false;
			foreach (BuilderMovingPart builderMovingPart in this.MovingParts)
			{
				builderMovingPart.SetMoving(false);
			}
			this.activated = false;
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x0016FB1C File Offset: 0x0016DD1C
		public void OnStateChanged(byte newState, NetPlayer instigator, int timeStamp)
		{
			if (!this.IsStateValid(newState))
			{
				return;
			}
			if (this.myPiece.state != BuilderPiece.State.AttachedAndPlaced)
			{
				return;
			}
			if (!this.activated)
			{
				return;
			}
			if (newState == 0 && !this.moving)
			{
				this.moving = true;
				if (this.startMovingFX != null)
				{
					ObjectPools.instance.Instantiate(this.startMovingFX, base.transform.position);
				}
				using (List<BuilderMovingPart>.Enumerator enumerator = this.MovingParts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BuilderMovingPart builderMovingPart = enumerator.Current;
						if (!builderMovingPart.IsAnchoredToTable())
						{
							builderMovingPart.ActivateAtNode(this.currentPauseNode, timeStamp);
							this.currentPauseNode = builderMovingPart.GetStartNode();
						}
					}
					return;
				}
			}
			if (this.moving && this.stopMovingFX != null)
			{
				ObjectPools.instance.Instantiate(this.stopMovingFX, base.transform.position);
			}
			this.moving = false;
			this.currentPauseNode = newState - 1;
			foreach (BuilderMovingPart builderMovingPart2 in this.MovingParts)
			{
				if (!builderMovingPart2.IsAnchoredToTable())
				{
					builderMovingPart2.PauseMovement(this.currentPauseNode);
				}
			}
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnStateRequest(byte newState, NetPlayer instigator, int timeStamp)
		{
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x0005A8BB File Offset: 0x00058ABB
		public bool IsStateValid(byte state)
		{
			return (int)state <= BuilderMovingPart.NUM_PAUSE_NODES * 2 + 1;
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x0005A8CC File Offset: 0x00058ACC
		public void FunctionalPieceUpdate()
		{
			this.UpdateMaster();
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x0016FC84 File Offset: 0x0016DE84
		public void FunctionalPieceFixedUpdate()
		{
			if (!this.moving)
			{
				return;
			}
			foreach (BuilderMovingPart builderMovingPart in this.MovingParts)
			{
				if (!builderMovingPart.IsAnchoredToTable())
				{
					builderMovingPart.UpdateMovingGrid();
				}
			}
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x0016FCE8 File Offset: 0x0016DEE8
		private void UpdateMaster()
		{
			if (!NetworkSystem.Instance.IsMasterClient)
			{
				return;
			}
			foreach (BuilderMovingPart builderMovingPart in this.MovingParts)
			{
				if (!builderMovingPart.IsAnchoredToTable())
				{
					int num = 0;
					foreach (BuilderAttachGridPlane builderAttachGridPlane in builderMovingPart.myGridPlanes)
					{
						num += builderAttachGridPlane.GetChildCount();
					}
					bool flag = num <= 5;
					if (flag && !this.moving)
					{
						BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 0, PhotonNetwork.MasterClient, NetworkSystem.Instance.ServerTimestamp);
					}
					if (!flag && this.moving)
					{
						byte state = builderMovingPart.GetNearestNode() + 1;
						BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, state, PhotonNetwork.MasterClient, NetworkSystem.Instance.ServerTimestamp);
					}
				}
			}
		}

		// Token: 0x040041CE RID: 16846
		public List<BuilderMovingPart> MovingParts;

		// Token: 0x040041CF RID: 16847
		public BuilderPiece myPiece;

		// Token: 0x040041D0 RID: 16848
		public const int MAX_MOVING_CHILDREN = 5;

		// Token: 0x040041D1 RID: 16849
		[SerializeField]
		private GameObject startMovingFX;

		// Token: 0x040041D2 RID: 16850
		[SerializeField]
		private GameObject stopMovingFX;

		// Token: 0x040041D3 RID: 16851
		private bool activated;

		// Token: 0x040041D4 RID: 16852
		private bool moving;

		// Token: 0x040041D5 RID: 16853
		private const byte MOVING_STATE = 0;

		// Token: 0x040041D6 RID: 16854
		private byte currentPauseNode;
	}
}
