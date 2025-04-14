using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x020009F9 RID: 2553
	public class BuilderMovingSnapPiece : MonoBehaviour, IBuilderPieceComponent, IBuilderPieceFunctional
	{
		// Token: 0x06003FCA RID: 16330 RVA: 0x0012E600 File Offset: 0x0012C800
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

		// Token: 0x06003FCB RID: 16331 RVA: 0x0012E68C File Offset: 0x0012C88C
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

		// Token: 0x06003FCC RID: 16332 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceCreate(int pieceType, int pieceId)
		{
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x0012E6FC File Offset: 0x0012C8FC
		public void OnPieceDestroy()
		{
			foreach (BuilderMovingPart builderMovingPart in this.MovingParts)
			{
				builderMovingPart.OnPieceDestroy();
			}
		}

		// Token: 0x06003FCE RID: 16334 RVA: 0x0012E74C File Offset: 0x0012C94C
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

		// Token: 0x06003FCF RID: 16335 RVA: 0x0012E818 File Offset: 0x0012CA18
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

		// Token: 0x06003FD0 RID: 16336 RVA: 0x0012E9CC File Offset: 0x0012CBCC
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

		// Token: 0x06003FD1 RID: 16337 RVA: 0x0012EA50 File Offset: 0x0012CC50
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

		// Token: 0x06003FD2 RID: 16338 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnStateRequest(byte newState, NetPlayer instigator, int timeStamp)
		{
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x0012EBB8 File Offset: 0x0012CDB8
		public bool IsStateValid(byte state)
		{
			return (int)state <= BuilderMovingPart.NUM_PAUSE_NODES * 2 + 1;
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x0012EBC9 File Offset: 0x0012CDC9
		public void FunctionalPieceUpdate()
		{
			this.UpdateMaster();
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x0012EBD4 File Offset: 0x0012CDD4
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

		// Token: 0x06003FD6 RID: 16342 RVA: 0x0012EC38 File Offset: 0x0012CE38
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

		// Token: 0x040040D4 RID: 16596
		public List<BuilderMovingPart> MovingParts;

		// Token: 0x040040D5 RID: 16597
		public BuilderPiece myPiece;

		// Token: 0x040040D6 RID: 16598
		public const int MAX_MOVING_CHILDREN = 5;

		// Token: 0x040040D7 RID: 16599
		[SerializeField]
		private GameObject startMovingFX;

		// Token: 0x040040D8 RID: 16600
		[SerializeField]
		private GameObject stopMovingFX;

		// Token: 0x040040D9 RID: 16601
		private bool activated;

		// Token: 0x040040DA RID: 16602
		private bool moving;

		// Token: 0x040040DB RID: 16603
		private const byte MOVING_STATE = 0;

		// Token: 0x040040DC RID: 16604
		private byte currentPauseNode;
	}
}
