using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x020009FC RID: 2556
	public class BuilderMovingSnapPiece : MonoBehaviour, IBuilderPieceComponent, IBuilderPieceFunctional
	{
		// Token: 0x06003FD6 RID: 16342 RVA: 0x0012EBC8 File Offset: 0x0012CDC8
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

		// Token: 0x06003FD7 RID: 16343 RVA: 0x0012EC54 File Offset: 0x0012CE54
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

		// Token: 0x06003FD8 RID: 16344 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceCreate(int pieceType, int pieceId)
		{
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x0012ECC4 File Offset: 0x0012CEC4
		public void OnPieceDestroy()
		{
			foreach (BuilderMovingPart builderMovingPart in this.MovingParts)
			{
				builderMovingPart.OnPieceDestroy();
			}
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x0012ED14 File Offset: 0x0012CF14
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

		// Token: 0x06003FDB RID: 16347 RVA: 0x0012EDE0 File Offset: 0x0012CFE0
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

		// Token: 0x06003FDC RID: 16348 RVA: 0x0012EF94 File Offset: 0x0012D194
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

		// Token: 0x06003FDD RID: 16349 RVA: 0x0012F018 File Offset: 0x0012D218
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

		// Token: 0x06003FDE RID: 16350 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnStateRequest(byte newState, NetPlayer instigator, int timeStamp)
		{
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x0012F180 File Offset: 0x0012D380
		public bool IsStateValid(byte state)
		{
			return (int)state <= BuilderMovingPart.NUM_PAUSE_NODES * 2 + 1;
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x0012F191 File Offset: 0x0012D391
		public void FunctionalPieceUpdate()
		{
			this.UpdateMaster();
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x0012F19C File Offset: 0x0012D39C
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

		// Token: 0x06003FE2 RID: 16354 RVA: 0x0012F200 File Offset: 0x0012D400
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

		// Token: 0x040040E6 RID: 16614
		public List<BuilderMovingPart> MovingParts;

		// Token: 0x040040E7 RID: 16615
		public BuilderPiece myPiece;

		// Token: 0x040040E8 RID: 16616
		public const int MAX_MOVING_CHILDREN = 5;

		// Token: 0x040040E9 RID: 16617
		[SerializeField]
		private GameObject startMovingFX;

		// Token: 0x040040EA RID: 16618
		[SerializeField]
		private GameObject stopMovingFX;

		// Token: 0x040040EB RID: 16619
		private bool activated;

		// Token: 0x040040EC RID: 16620
		private bool moving;

		// Token: 0x040040ED RID: 16621
		private const byte MOVING_STATE = 0;

		// Token: 0x040040EE RID: 16622
		private byte currentPauseNode;
	}
}
