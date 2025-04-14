using System;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x020009F3 RID: 2547
	public class BuilderAnimatePieceOnTriggerEnter : MonoBehaviour, IBuilderPieceComponent, IBuilderPieceFunctional
	{
		// Token: 0x06003F97 RID: 16279 RVA: 0x0012D6C5 File Offset: 0x0012B8C5
		private void OnTriggerEnter(Collider other)
		{
			if (this.lastAnimateTime + this.animateCooldown < Time.time)
			{
				BuilderTableNetworking.instance.RequestFunctionalPieceStateChange(this.myPiece.pieceId, 1);
			}
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x0012D6F1 File Offset: 0x0012B8F1
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.currentState = BuilderAnimatePieceOnTriggerEnter.FunctionalState.Idle;
			this.trigger.enabled = false;
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceDestroy()
		{
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x0012D706 File Offset: 0x0012B906
		public void OnPieceActivate()
		{
			this.trigger.enabled = true;
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x0012D714 File Offset: 0x0012B914
		public void OnPieceDeactivate()
		{
			this.trigger.enabled = false;
			if (this.currentState == BuilderAnimatePieceOnTriggerEnter.FunctionalState.Animating)
			{
				this.myPiece.SetFunctionalPieceState(0, NetworkSystem.Instance.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				BuilderTable.instance.UnregisterFunctionalPiece(this);
			}
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x0012D764 File Offset: 0x0012B964
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

		// Token: 0x06003F9E RID: 16286 RVA: 0x0012D7BC File Offset: 0x0012B9BC
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

		// Token: 0x06003F9F RID: 16287 RVA: 0x0012D814 File Offset: 0x0012BA14
		public bool IsStateValid(byte state)
		{
			return state <= 1;
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x0012D820 File Offset: 0x0012BA20
		public void FunctionalPieceUpdate()
		{
			if (this.lastAnimateTime + this.animateCooldown < Time.time)
			{
				this.myPiece.SetFunctionalPieceState(0, NetworkSystem.Instance.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				BuilderTable.instance.UnregisterFunctionalPiece(this);
			}
		}

		// Token: 0x0400409C RID: 16540
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x0400409D RID: 16541
		[SerializeField]
		private Collider trigger;

		// Token: 0x0400409E RID: 16542
		[SerializeField]
		private Animation anim;

		// Token: 0x0400409F RID: 16543
		[SerializeField]
		private float animateCooldown = 0.5f;

		// Token: 0x040040A0 RID: 16544
		private float lastAnimateTime;

		// Token: 0x040040A1 RID: 16545
		private BuilderAnimatePieceOnTriggerEnter.FunctionalState currentState;

		// Token: 0x020009F4 RID: 2548
		private enum FunctionalState
		{
			// Token: 0x040040A3 RID: 16547
			Idle,
			// Token: 0x040040A4 RID: 16548
			Animating
		}
	}
}
