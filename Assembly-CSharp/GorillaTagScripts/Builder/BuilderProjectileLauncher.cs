using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A34 RID: 2612
	public class BuilderProjectileLauncher : MonoBehaviour, IBuilderPieceFunctional, IBuilderPieceComponent
	{
		// Token: 0x06004192 RID: 16786 RVA: 0x001722A4 File Offset: 0x001704A4
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

		// Token: 0x06004193 RID: 16787 RVA: 0x00172478 File Offset: 0x00170678
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

		// Token: 0x06004194 RID: 16788 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnStateRequest(byte newState, NetPlayer instigator, int timeStamp)
		{
		}

		// Token: 0x06004195 RID: 16789 RVA: 0x0005A6BA File Offset: 0x000588BA
		public bool IsStateValid(byte state)
		{
			return state <= 1;
		}

		// Token: 0x06004196 RID: 16790 RVA: 0x001724CC File Offset: 0x001706CC
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

		// Token: 0x06004197 RID: 16791 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPieceCreate(int pieceType, int pieceId)
		{
		}

		// Token: 0x06004198 RID: 16792 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPieceDestroy()
		{
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x0600419A RID: 16794 RVA: 0x0005ADC1 File Offset: 0x00058FC1
		public void OnPieceActivate()
		{
			BuilderTable.instance.RegisterFunctionalPiece(this);
		}

		// Token: 0x0600419B RID: 16795 RVA: 0x00172544 File Offset: 0x00170744
		public void OnPieceDeactivate()
		{
			BuilderTable.instance.UnregisterFunctionalPiece(this);
			for (int i = this.launchedProjectiles.Count - 1; i >= 0; i--)
			{
				this.launchedProjectiles[i].Deactivate();
			}
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x0005ADCE File Offset: 0x00058FCE
		public void RegisterProjectile(BuilderProjectile projectile)
		{
			this.launchedProjectiles.Add(projectile);
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x0005ADDC File Offset: 0x00058FDC
		public void UnRegisterProjectile(BuilderProjectile projectile)
		{
			this.launchedProjectiles.Remove(projectile);
			this.allProjectiles.Remove(projectile.projectileId);
		}

		// Token: 0x0400425F RID: 16991
		private List<BuilderProjectile> launchedProjectiles = new List<BuilderProjectile>();

		// Token: 0x04004260 RID: 16992
		[SerializeField]
		protected BuilderPiece myPiece;

		// Token: 0x04004261 RID: 16993
		[SerializeField]
		protected float fireCooldown = 2f;

		// Token: 0x04004262 RID: 16994
		[Tooltip("launch in Y direction")]
		[SerializeField]
		private Transform launchPosition;

		// Token: 0x04004263 RID: 16995
		[SerializeField]
		private float launchVelocity;

		// Token: 0x04004264 RID: 16996
		[SerializeField]
		private AudioSource launchSound;

		// Token: 0x04004265 RID: 16997
		[SerializeField]
		protected GameObject projectilePrefab;

		// Token: 0x04004266 RID: 16998
		[SerializeField]
		protected float projectileScale = 0.06f;

		// Token: 0x04004267 RID: 16999
		[SerializeField]
		protected float gravityMultiplier = 1f;

		// Token: 0x04004268 RID: 17000
		public SlingshotProjectile.AOEKnockbackConfig knockbackConfig;

		// Token: 0x04004269 RID: 17001
		private float lastFireTime;

		// Token: 0x0400426A RID: 17002
		private BuilderProjectileLauncher.FunctionalState currentState;

		// Token: 0x0400426B RID: 17003
		private Dictionary<int, BuilderProjectile> allProjectiles = new Dictionary<int, BuilderProjectile>();

		// Token: 0x02000A35 RID: 2613
		private enum FunctionalState
		{
			// Token: 0x0400426D RID: 17005
			Idle,
			// Token: 0x0400426E RID: 17006
			Fire
		}
	}
}
