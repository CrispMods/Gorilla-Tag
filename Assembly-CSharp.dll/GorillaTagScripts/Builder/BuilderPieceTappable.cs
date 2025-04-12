using System;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A05 RID: 2565
	[RequireComponent(typeof(Collider))]
	[RequireComponent(typeof(GorillaSurfaceOverride))]
	public class BuilderPieceTappable : MonoBehaviour, IBuilderPieceComponent, IBuilderPieceFunctional
	{
		// Token: 0x06004029 RID: 16425 RVA: 0x00059145 File Offset: 0x00057345
		public virtual bool CanTap()
		{
			return this.isPieceActive && Time.time > this.lastTapTime + this.tapCooldown;
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x00059165 File Offset: 0x00057365
		public void OnTapLocal(float tapStrength)
		{
			if (!NetworkSystem.Instance.InRoom)
			{
				return;
			}
			if (!this.CanTap())
			{
				return;
			}
			BuilderTableNetworking.instance.RequestFunctionalPieceStateChange(this.myPiece.pieceId, 1);
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x0002F75F File Offset: 0x0002D95F
		public virtual void OnTapReplicated()
		{
		}

		// Token: 0x0600402C RID: 16428 RVA: 0x00059193 File Offset: 0x00057393
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.currentState = BuilderPieceTappable.FunctionalState.Idle;
		}

		// Token: 0x0600402D RID: 16429 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnPieceDestroy()
		{
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x0005919C File Offset: 0x0005739C
		public void OnPieceActivate()
		{
			this.isPieceActive = true;
		}

		// Token: 0x06004030 RID: 16432 RVA: 0x000591A5 File Offset: 0x000573A5
		public void OnPieceDeactivate()
		{
			this.isPieceActive = false;
			if (this.currentState == BuilderPieceTappable.FunctionalState.Tap)
			{
				this.myPiece.SetFunctionalPieceState(0, NetworkSystem.Instance.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				BuilderTable.instance.UnregisterFunctionalPiece(this);
			}
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x000591E2 File Offset: 0x000573E2
		public void OnStateChanged(byte newState, NetPlayer instigator, int timeStamp)
		{
			if (!this.IsStateValid(newState))
			{
				return;
			}
			if (newState == 1 && this.currentState != BuilderPieceTappable.FunctionalState.Tap)
			{
				this.lastTapTime = Time.time;
				this.OnTapReplicated();
				BuilderTable.instance.RegisterFunctionalPiece(this);
			}
			this.currentState = (BuilderPieceTappable.FunctionalState)newState;
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x0016AA24 File Offset: 0x00168C24
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
			if (newState == 1 && this.CanTap())
			{
				BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, newState, instigator.GetPlayerRef(), timeStamp);
			}
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x00058CB8 File Offset: 0x00056EB8
		public bool IsStateValid(byte state)
		{
			return state <= 1;
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x0016AA74 File Offset: 0x00168C74
		public void FunctionalPieceUpdate()
		{
			if (this.lastTapTime + this.tapCooldown < Time.time)
			{
				this.myPiece.SetFunctionalPieceState(0, NetworkSystem.Instance.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				BuilderTable.instance.UnregisterFunctionalPiece(this);
			}
		}

		// Token: 0x04004151 RID: 16721
		[SerializeField]
		protected BuilderPiece myPiece;

		// Token: 0x04004152 RID: 16722
		[SerializeField]
		protected float tapCooldown = 0.5f;

		// Token: 0x04004153 RID: 16723
		private bool isPieceActive;

		// Token: 0x04004154 RID: 16724
		private float lastTapTime;

		// Token: 0x04004155 RID: 16725
		private BuilderPieceTappable.FunctionalState currentState;

		// Token: 0x02000A06 RID: 2566
		private enum FunctionalState
		{
			// Token: 0x04004157 RID: 16727
			Idle,
			// Token: 0x04004158 RID: 16728
			Tap
		}
	}
}
