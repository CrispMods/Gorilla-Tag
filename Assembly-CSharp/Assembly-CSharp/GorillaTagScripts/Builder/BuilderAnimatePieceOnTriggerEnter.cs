using System;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x020009F6 RID: 2550
	public class BuilderAnimatePieceOnTriggerEnter : MonoBehaviour, IBuilderPieceComponent, IBuilderPieceFunctional
	{
		// Token: 0x06003FA3 RID: 16291 RVA: 0x0012DC8D File Offset: 0x0012BE8D
		private void OnTriggerEnter(Collider other)
		{
			if (this.lastAnimateTime + this.animateCooldown < Time.time)
			{
				BuilderTableNetworking.instance.RequestFunctionalPieceStateChange(this.myPiece.pieceId, 1);
			}
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x0012DCB9 File Offset: 0x0012BEB9
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.currentState = BuilderAnimatePieceOnTriggerEnter.FunctionalState.Idle;
			this.trigger.enabled = false;
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceDestroy()
		{
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x0012DCCE File Offset: 0x0012BECE
		public void OnPieceActivate()
		{
			this.trigger.enabled = true;
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x0012DCDC File Offset: 0x0012BEDC
		public void OnPieceDeactivate()
		{
			this.trigger.enabled = false;
			if (this.currentState == BuilderAnimatePieceOnTriggerEnter.FunctionalState.Animating)
			{
				this.myPiece.SetFunctionalPieceState(0, NetworkSystem.Instance.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				BuilderTable.instance.UnregisterFunctionalPiece(this);
			}
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x0012DD2C File Offset: 0x0012BF2C
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

		// Token: 0x06003FAA RID: 16298 RVA: 0x0012DD84 File Offset: 0x0012BF84
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

		// Token: 0x06003FAB RID: 16299 RVA: 0x0012DDDC File Offset: 0x0012BFDC
		public bool IsStateValid(byte state)
		{
			return state <= 1;
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x0012DDE8 File Offset: 0x0012BFE8
		public void FunctionalPieceUpdate()
		{
			if (this.lastAnimateTime + this.animateCooldown < Time.time)
			{
				this.myPiece.SetFunctionalPieceState(0, NetworkSystem.Instance.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				BuilderTable.instance.UnregisterFunctionalPiece(this);
			}
		}

		// Token: 0x040040AE RID: 16558
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x040040AF RID: 16559
		[SerializeField]
		private Collider trigger;

		// Token: 0x040040B0 RID: 16560
		[SerializeField]
		private Animation anim;

		// Token: 0x040040B1 RID: 16561
		[SerializeField]
		private float animateCooldown = 0.5f;

		// Token: 0x040040B2 RID: 16562
		private float lastAnimateTime;

		// Token: 0x040040B3 RID: 16563
		private BuilderAnimatePieceOnTriggerEnter.FunctionalState currentState;

		// Token: 0x020009F7 RID: 2551
		private enum FunctionalState
		{
			// Token: 0x040040B5 RID: 16565
			Idle,
			// Token: 0x040040B6 RID: 16566
			Animating
		}
	}
}
