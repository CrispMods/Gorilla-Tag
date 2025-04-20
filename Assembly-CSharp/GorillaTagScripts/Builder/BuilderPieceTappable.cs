using System;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A2F RID: 2607
	[RequireComponent(typeof(Collider))]
	[RequireComponent(typeof(GorillaSurfaceOverride))]
	public class BuilderPieceTappable : MonoBehaviour, IBuilderPieceComponent, IBuilderPieceFunctional
	{
		// Token: 0x06004162 RID: 16738 RVA: 0x0005AB47 File Offset: 0x00058D47
		public virtual bool CanTap()
		{
			return this.isPieceActive && Time.time > this.lastTapTime + this.tapCooldown;
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x0005AB67 File Offset: 0x00058D67
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

		// Token: 0x06004164 RID: 16740 RVA: 0x00030607 File Offset: 0x0002E807
		public virtual void OnTapReplicated()
		{
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x0005AB95 File Offset: 0x00058D95
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.currentState = BuilderPieceTappable.FunctionalState.Idle;
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPieceDestroy()
		{
		}

		// Token: 0x06004167 RID: 16743 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06004168 RID: 16744 RVA: 0x0005AB9E File Offset: 0x00058D9E
		public void OnPieceActivate()
		{
			this.isPieceActive = true;
		}

		// Token: 0x06004169 RID: 16745 RVA: 0x0005ABA7 File Offset: 0x00058DA7
		public void OnPieceDeactivate()
		{
			this.isPieceActive = false;
			if (this.currentState == BuilderPieceTappable.FunctionalState.Tap)
			{
				this.myPiece.SetFunctionalPieceState(0, NetworkSystem.Instance.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				BuilderTable.instance.UnregisterFunctionalPiece(this);
			}
		}

		// Token: 0x0600416A RID: 16746 RVA: 0x0005ABE4 File Offset: 0x00058DE4
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

		// Token: 0x0600416B RID: 16747 RVA: 0x001718A8 File Offset: 0x0016FAA8
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

		// Token: 0x0600416C RID: 16748 RVA: 0x0005A6BA File Offset: 0x000588BA
		public bool IsStateValid(byte state)
		{
			return state <= 1;
		}

		// Token: 0x0600416D RID: 16749 RVA: 0x001718F8 File Offset: 0x0016FAF8
		public void FunctionalPieceUpdate()
		{
			if (this.lastTapTime + this.tapCooldown < Time.time)
			{
				this.myPiece.SetFunctionalPieceState(0, NetworkSystem.Instance.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
				BuilderTable.instance.UnregisterFunctionalPiece(this);
			}
		}

		// Token: 0x04004239 RID: 16953
		[SerializeField]
		protected BuilderPiece myPiece;

		// Token: 0x0400423A RID: 16954
		[SerializeField]
		protected float tapCooldown = 0.5f;

		// Token: 0x0400423B RID: 16955
		private bool isPieceActive;

		// Token: 0x0400423C RID: 16956
		private float lastTapTime;

		// Token: 0x0400423D RID: 16957
		private BuilderPieceTappable.FunctionalState currentState;

		// Token: 0x02000A30 RID: 2608
		private enum FunctionalState
		{
			// Token: 0x0400423F RID: 16959
			Idle,
			// Token: 0x04004240 RID: 16960
			Tap
		}
	}
}
