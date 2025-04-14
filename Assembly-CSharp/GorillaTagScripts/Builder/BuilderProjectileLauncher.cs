using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A07 RID: 2567
	public class BuilderProjectileLauncher : MonoBehaviour, IBuilderPieceFunctional, IBuilderPieceComponent
	{
		// Token: 0x0600404D RID: 16461 RVA: 0x0013175C File Offset: 0x0012F95C
		private void LaunchProjectile(int timeStamp)
		{
			if (Time.time > this.lastFireTime + this.fireCooldown)
			{
				this.lastFireTime = Time.time;
				int hash = PoolUtils.GameObjHashCode(this.projectilePrefab);
				try
				{
					GameObject gameObject = ObjectPools.instance.Instantiate(hash);
					gameObject.transform.localScale = Vector3.one * this.projectileScale;
					BuilderProjectile component = gameObject.GetComponent<BuilderProjectile>();
					int num = HashCode.Combine<int, int>(this.myPiece.pieceId, timeStamp);
					if (this.allProjectiles.ContainsKey(num))
					{
						this.allProjectiles.Remove(num);
					}
					this.allProjectiles.Add(num, component);
					SlingshotProjectile.AOEKnockbackConfig value = new SlingshotProjectile.AOEKnockbackConfig
					{
						aeoOuterRadius = this.knockbackConfig.aeoOuterRadius * this.projectileScale,
						aeoInnerRadius = this.knockbackConfig.aeoInnerRadius * this.projectileScale,
						applyAOEKnockback = this.knockbackConfig.applyAOEKnockback,
						impactVelocityThreshold = this.knockbackConfig.impactVelocityThreshold * this.projectileScale,
						knockbackVelocity = this.knockbackConfig.knockbackVelocity * this.projectileScale,
						playerProximityEffect = this.knockbackConfig.playerProximityEffect
					};
					component.aoeKnockbackConfig = new SlingshotProjectile.AOEKnockbackConfig?(value);
					component.gravityMultiplier = this.gravityMultiplier;
					component.Launch(this.launchPosition.position, this.launchVelocity * this.projectileScale * this.launchPosition.up, this, num, this.projectileScale, timeStamp);
					if (this.launchSound != null && this.launchSound.clip != null)
					{
						this.launchSound.Play();
					}
				}
				catch (Exception value2)
				{
					Console.WriteLine(value2);
					throw;
				}
			}
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x00131930 File Offset: 0x0012FB30
		public void OnStateChanged(byte newState, NetPlayer instigator, int timeStamp)
		{
			if (!this.IsStateValid(newState))
			{
				return;
			}
			if ((BuilderProjectileLauncher.FunctionalState)newState == this.currentState)
			{
				return;
			}
			this.currentState = (BuilderProjectileLauncher.FunctionalState)newState;
			if (newState == 1)
			{
				this.LaunchProjectile(timeStamp);
				this.myPiece.SetFunctionalPieceState(0, NetworkSystem.Instance.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
			}
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnStateRequest(byte newState, NetPlayer instigator, int timeStamp)
		{
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x0012D814 File Offset: 0x0012BA14
		public bool IsStateValid(byte state)
		{
			return state <= 1;
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x00131984 File Offset: 0x0012FB84
		public void FunctionalPieceUpdate()
		{
			for (int i = this.launchedProjectiles.Count - 1; i >= 0; i--)
			{
				this.launchedProjectiles[i].UpdateProjectile();
			}
			if (PhotonNetwork.IsMasterClient && this.lastFireTime + this.fireCooldown < Time.time)
			{
				BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, 1, PhotonNetwork.LocalPlayer, NetworkSystem.Instance.ServerTimestamp);
			}
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceCreate(int pieceType, int pieceId)
		{
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceDestroy()
		{
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x001319FA File Offset: 0x0012FBFA
		public void OnPieceActivate()
		{
			BuilderTable.instance.RegisterFunctionalPiece(this);
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x00131A08 File Offset: 0x0012FC08
		public void OnPieceDeactivate()
		{
			BuilderTable.instance.UnregisterFunctionalPiece(this);
			for (int i = this.launchedProjectiles.Count - 1; i >= 0; i--)
			{
				this.launchedProjectiles[i].Deactivate();
			}
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x00131A49 File Offset: 0x0012FC49
		public void RegisterProjectile(BuilderProjectile projectile)
		{
			this.launchedProjectiles.Add(projectile);
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x00131A57 File Offset: 0x0012FC57
		public void UnRegisterProjectile(BuilderProjectile projectile)
		{
			this.launchedProjectiles.Remove(projectile);
			this.allProjectiles.Remove(projectile.projectileId);
		}

		// Token: 0x04004165 RID: 16741
		private List<BuilderProjectile> launchedProjectiles = new List<BuilderProjectile>();

		// Token: 0x04004166 RID: 16742
		[SerializeField]
		protected BuilderPiece myPiece;

		// Token: 0x04004167 RID: 16743
		[SerializeField]
		protected float fireCooldown = 2f;

		// Token: 0x04004168 RID: 16744
		[Tooltip("launch in Y direction")]
		[SerializeField]
		private Transform launchPosition;

		// Token: 0x04004169 RID: 16745
		[SerializeField]
		private float launchVelocity;

		// Token: 0x0400416A RID: 16746
		[SerializeField]
		private AudioSource launchSound;

		// Token: 0x0400416B RID: 16747
		[SerializeField]
		protected GameObject projectilePrefab;

		// Token: 0x0400416C RID: 16748
		[SerializeField]
		protected float projectileScale = 0.06f;

		// Token: 0x0400416D RID: 16749
		[SerializeField]
		protected float gravityMultiplier = 1f;

		// Token: 0x0400416E RID: 16750
		public SlingshotProjectile.AOEKnockbackConfig knockbackConfig;

		// Token: 0x0400416F RID: 16751
		private float lastFireTime;

		// Token: 0x04004170 RID: 16752
		private BuilderProjectileLauncher.FunctionalState currentState;

		// Token: 0x04004171 RID: 16753
		private Dictionary<int, BuilderProjectile> allProjectiles = new Dictionary<int, BuilderProjectile>();

		// Token: 0x02000A08 RID: 2568
		private enum FunctionalState
		{
			// Token: 0x04004173 RID: 16755
			Idle,
			// Token: 0x04004174 RID: 16756
			Fire
		}
	}
}
