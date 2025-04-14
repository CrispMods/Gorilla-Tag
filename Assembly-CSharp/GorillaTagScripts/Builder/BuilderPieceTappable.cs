using System;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A02 RID: 2562
	[RequireComponent(typeof(Collider))]
	[RequireComponent(typeof(GorillaSurfaceOverride))]
	public class BuilderPieceTappable : MonoBehaviour, IBuilderPieceComponent, IBuilderPieceFunctional
	{
		// Token: 0x0600401D RID: 16413 RVA: 0x00130A26 File Offset: 0x0012EC26
		public virtual bool CanTap()
		{
			return this.isPieceActive && Time.time > this.lastTapTime + this.tapCooldown;
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x00130A46 File Offset: 0x0012EC46
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

		// Token: 0x0600401F RID: 16415 RVA: 0x000023F4 File Offset: 0x000005F4
		public virtual void OnTapReplicated()
		{
		}

		// Token: 0x06004020 RID: 16416 RVA: 0x00130A74 File Offset: 0x0012EC74
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.currentState = BuilderPieceTappable.FunctionalState.Idle;
		}

		// Token: 0x06004021 RID: 16417 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceDestroy()
		{
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x00130A7D File Offset: 0x0012EC7D
		public void OnPieceActivate()
		{
			this.isPieceActive = true;
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x00130A86 File Offset: 0x0012EC86
		public void OnPieceDeactivate()
		{
			this.isPieceActive = false;
			if (this.currentState == BuilderPieceTappable.FunctionalState.Tap)
			{
				this.myPiece.SetFunctionalPieceState(0, NetworkSystem.Instance.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				BuilderTable.instance.UnregisterFunctionalPiece(this);
			}
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x00130AC3 File Offset: 0x0012ECC3
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

		// Token: 0x06004026 RID: 16422 RVA: 0x00130B00 File Offset: 0x0012ED00
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

		// Token: 0x06004027 RID: 16423 RVA: 0x0012D814 File Offset: 0x0012BA14
		public bool IsStateValid(byte state)
		{
			return state <= 1;
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x00130B50 File Offset: 0x0012ED50
		public void FunctionalPieceUpdate()
		{
			if (this.lastTapTime + this.tapCooldown < Time.time)
			{
				this.myPiece.SetFunctionalPieceState(0, NetworkSystem.Instance.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				BuilderTable.instance.UnregisterFunctionalPiece(this);
			}
		}

		// Token: 0x0400413F RID: 16703
		[SerializeField]
		protected BuilderPiece myPiece;

		// Token: 0x04004140 RID: 16704
		[SerializeField]
		protected float tapCooldown = 0.5f;

		// Token: 0x04004141 RID: 16705
		private bool isPieceActive;

		// Token: 0x04004142 RID: 16706
		private float lastTapTime;

		// Token: 0x04004143 RID: 16707
		private BuilderPieceTappable.FunctionalState currentState;

		// Token: 0x02000A03 RID: 2563
		private enum FunctionalState
		{
			// Token: 0x04004145 RID: 16709
			Idle,
			// Token: 0x04004146 RID: 16710
			Tap
		}
	}
}
