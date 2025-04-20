using System;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A20 RID: 2592
	public class BuilderAnimatePieceOnTriggerEnter : MonoBehaviour, IBuilderPieceComponent, IBuilderPieceFunctional
	{
		// Token: 0x060040DC RID: 16604 RVA: 0x0005A66B File Offset: 0x0005886B
		private void OnTriggerEnter(Collider other)
		{
			if (this.lastAnimateTime + this.animateCooldown < Time.time)
			{
				BuilderTableNetworking.instance.RequestFunctionalPieceStateChange(this.myPiece.pieceId, 1);
			}
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x0005A697 File Offset: 0x00058897
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.currentState = BuilderAnimatePieceOnTriggerEnter.FunctionalState.Idle;
			this.trigger.enabled = false;
		}

		// Token: 0x060040DE RID: 16606 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPieceDestroy()
		{
		}

		// Token: 0x060040DF RID: 16607 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x060040E0 RID: 16608 RVA: 0x0005A6AC File Offset: 0x000588AC
		public void OnPieceActivate()
		{
			this.trigger.enabled = true;
		}

		// Token: 0x060040E1 RID: 16609 RVA: 0x0016EA10 File Offset: 0x0016CC10
		public void OnPieceDeactivate()
		{
			this.trigger.enabled = false;
			if (this.currentState == BuilderAnimatePieceOnTriggerEnter.FunctionalState.Animating)
			{
				this.myPiece.SetFunctionalPieceState(0, NetworkSystem.Instance.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				BuilderTable.instance.UnregisterFunctionalPiece(this);
			}
		}

		// Token: 0x060040E2 RID: 16610 RVA: 0x0016EA60 File Offset: 0x0016CC60
		public void OnStateChanged(byte newState, NetPlayer instigator, int timeStamp)
		{
			if (!this.IsStateValid(newState))
			{
				return;
			}
			if (newState == 1 && this.currentState != BuilderAnimatePieceOnTriggerEnter.FunctionalState.Animating)
			{
				this.lastAnimateTime = Time.time;
				this.anim.Rewind();
				this.anim.Play();
				BuilderTable.instance.RegisterFunctionalPiece(this);
			}
			this.currentState = (BuilderAnimatePieceOnTriggerEnter.FunctionalState)newState;
		}

		// Token: 0x060040E3 RID: 16611 RVA: 0x0016EAB8 File Offset: 0x0016CCB8
		public void OnStateRequest(byte newState, NetPlayer instigator, int timeStamp)
		{
			if (!NetworkSystem.Instance.IsMasterClient)
			{
				return;
			}
			if (!this.IsStateValid(newState) || instigator == null)
			{
				return;
			}
			if (this.lastAnimateTime + this.animateCooldown < Time.time)
			{
				BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, newState, instigator.GetPlayerRef(), timeStamp);
			}
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x0005A6BA File Offset: 0x000588BA
		public bool IsStateValid(byte state)
		{
			return state <= 1;
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x0016EB10 File Offset: 0x0016CD10
		public void FunctionalPieceUpdate()
		{
			if (this.lastAnimateTime + this.animateCooldown < Time.time)
			{
				this.myPiece.SetFunctionalPieceState(0, NetworkSystem.Instance.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				BuilderTable.instance.UnregisterFunctionalPiece(this);
			}
		}

		// Token: 0x04004196 RID: 16790
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x04004197 RID: 16791
		[SerializeField]
		private Collider trigger;

		// Token: 0x04004198 RID: 16792
		[SerializeField]
		private Animation anim;

		// Token: 0x04004199 RID: 16793
		[SerializeField]
		private float animateCooldown = 0.5f;

		// Token: 0x0400419A RID: 16794
		private float lastAnimateTime;

		// Token: 0x0400419B RID: 16795
		private BuilderAnimatePieceOnTriggerEnter.FunctionalState currentState;

		// Token: 0x02000A21 RID: 2593
		private enum FunctionalState
		{
			// Token: 0x0400419D RID: 16797
			Idle,
			// Token: 0x0400419E RID: 16798
			Animating
		}
	}
}
