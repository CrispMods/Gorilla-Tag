using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A0A RID: 2570
	public class BuilderProjectileLauncher : MonoBehaviour, IBuilderPieceFunctional, IBuilderPieceComponent
	{
		// Token: 0x06004059 RID: 16473 RVA: 0x00131D24 File Offset: 0x0012FF24
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

		// Token: 0x0600405A RID: 16474 RVA: 0x00131EF8 File Offset: 0x001300F8
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

		// Token: 0x0600405B RID: 16475 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnStateRequest(byte newState, NetPlayer instigator, int timeStamp)
		{
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x0012DDDC File Offset: 0x0012BFDC
		public bool IsStateValid(byte state)
		{
			return state <= 1;
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x00131F4C File Offset: 0x0013014C
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

		// Token: 0x0600405E RID: 16478 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceCreate(int pieceType, int pieceId)
		{
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceDestroy()
		{
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x00131FC2 File Offset: 0x001301C2
		public void OnPieceActivate()
		{
			BuilderTable.instance.RegisterFunctionalPiece(this);
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x00131FD0 File Offset: 0x001301D0
		public void OnPieceDeactivate()
		{
			BuilderTable.instance.UnregisterFunctionalPiece(this);
			for (int i = this.launchedProjectiles.Count - 1; i >= 0; i--)
			{
				this.launchedProjectiles[i].Deactivate();
			}
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x00132011 File Offset: 0x00130211
		public void RegisterProjectile(BuilderProjectile projectile)
		{
			this.launchedProjectiles.Add(projectile);
		}

		// Token: 0x06004064 RID: 16484 RVA: 0x0013201F File Offset: 0x0013021F
		public void UnRegisterProjectile(BuilderProjectile projectile)
		{
			this.launchedProjectiles.Remove(projectile);
			this.allProjectiles.Remove(projectile.projectileId);
		}

		// Token: 0x04004177 RID: 16759
		private List<BuilderProjectile> launchedProjectiles = new List<BuilderProjectile>();

		// Token: 0x04004178 RID: 16760
		[SerializeField]
		protected BuilderPiece myPiece;

		// Token: 0x04004179 RID: 16761
		[SerializeField]
		protected float fireCooldown = 2f;

		// Token: 0x0400417A RID: 16762
		[Tooltip("launch in Y direction")]
		[SerializeField]
		private Transform launchPosition;

		// Token: 0x0400417B RID: 16763
		[SerializeField]
		private float launchVelocity;

		// Token: 0x0400417C RID: 16764
		[SerializeField]
		private AudioSource launchSound;

		// Token: 0x0400417D RID: 16765
		[SerializeField]
		protected GameObject projectilePrefab;

		// Token: 0x0400417E RID: 16766
		[SerializeField]
		protected float projectileScale = 0.06f;

		// Token: 0x0400417F RID: 16767
		[SerializeField]
		protected float gravityMultiplier = 1f;

		// Token: 0x04004180 RID: 16768
		public SlingshotProjectile.AOEKnockbackConfig knockbackConfig;

		// Token: 0x04004181 RID: 16769
		private float lastFireTime;

		// Token: 0x04004182 RID: 16770
		private BuilderProjectileLauncher.FunctionalState currentState;

		// Token: 0x04004183 RID: 16771
		private Dictionary<int, BuilderProjectile> allProjectiles = new Dictionary<int, BuilderProjectile>();

		// Token: 0x02000A0B RID: 2571
		private enum FunctionalState
		{
			// Token: 0x04004185 RID: 16773
			Idle,
			// Token: 0x04004186 RID: 16774
			Fire
		}
	}
}
