using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A09 RID: 2569
	public class BuilderProjectileTarget : MonoBehaviour, IBuilderPieceFunctional
	{
		// Token: 0x0600405A RID: 16474 RVA: 0x00131AB8 File Offset: 0x0012FCB8
		private void Awake()
		{
			this.hitNotifier.OnProjectileHit += this.OnProjectileHit;
			foreach (Collider collider in this.colliders)
			{
				collider.contactOffset = 0.0001f;
			}
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x00131B24 File Offset: 0x0012FD24
		private void OnDestroy()
		{
			this.hitNotifier.OnProjectileHit -= this.OnProjectileHit;
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x00131B3D File Offset: 0x0012FD3D
		private void OnDisable()
		{
			this.hitCount = 0;
			if (this.scoreText != null)
			{
				this.scoreText.text = this.hitCount.ToString("D2");
			}
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x00131B70 File Offset: 0x0012FD70
		private void OnProjectileHit(SlingshotProjectile projectile, Collision collision)
		{
			if (this.myPiece.state != BuilderPiece.State.AttachedAndPlaced)
			{
				return;
			}
			if (projectile.projectileOwner == null || projectile.projectileOwner != NetworkSystem.Instance.LocalPlayer)
			{
				return;
			}
			if (this.lastHitTime + (double)this.hitCooldown < (double)Time.time)
			{
				BuilderTableNetworking.instance.RequestFunctionalPieceStateChange(this.myPiece.pieceId, 11);
			}
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x00131BD3 File Offset: 0x0012FDD3
		public void OnStateChanged(byte newState, NetPlayer instigator, int timeStamp)
		{
			if (instigator == null)
			{
				return;
			}
			if (!this.IsStateValid(newState))
			{
				return;
			}
			if (newState == 11)
			{
				return;
			}
			this.lastHitTime = (double)Time.time;
			this.hitCount = Mathf.Clamp((int)newState, 0, 10);
			this.PlayHitEffects();
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x00131C0C File Offset: 0x0012FE0C
		public void OnStateRequest(byte newState, NetPlayer instigator, int timeStamp)
		{
			if (!NetworkSystem.Instance.IsMasterClient)
			{
				return;
			}
			if (!this.IsStateValid(newState))
			{
				return;
			}
			if (instigator == null)
			{
				return;
			}
			if (newState != 11)
			{
				return;
			}
			this.hitCount++;
			this.hitCount %= 11;
			BuilderTableNetworking.instance.FunctionalPieceStateChangeMaster(this.myPiece.pieceId, (byte)this.hitCount, instigator.GetPlayerRef(), timeStamp);
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x00131C7A File Offset: 0x0012FE7A
		public bool IsStateValid(byte state)
		{
			return state <= 11;
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x00131C84 File Offset: 0x0012FE84
		private void PlayHitEffects()
		{
			if (this.hitSoundbank != null)
			{
				this.hitSoundbank.Play();
			}
			if (this.hitAnimation != null && this.hitAnimation.clip != null)
			{
				this.hitAnimation.Play();
			}
			if (this.scoreText != null)
			{
				this.scoreText.text = this.hitCount.ToString("D2");
			}
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x000023F4 File Offset: 0x000005F4
		public void FunctionalPieceUpdate()
		{
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x00131D00 File Offset: 0x0012FF00
		public float GetInteractionDistace()
		{
			return 20f;
		}

		// Token: 0x04004175 RID: 16757
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x04004176 RID: 16758
		[SerializeField]
		private SlingshotProjectileHitNotifier hitNotifier;

		// Token: 0x04004177 RID: 16759
		[SerializeField]
		protected float hitCooldown = 2f;

		// Token: 0x04004178 RID: 16760
		[Tooltip("Optional Sounds to play on hit")]
		[SerializeField]
		protected SoundBankPlayer hitSoundbank;

		// Token: 0x04004179 RID: 16761
		[Tooltip("Optional Sounds to play on hit")]
		[SerializeField]
		protected Animation hitAnimation;

		// Token: 0x0400417A RID: 16762
		[SerializeField]
		protected List<Collider> colliders;

		// Token: 0x0400417B RID: 16763
		[SerializeField]
		private TMP_Text scoreText;

		// Token: 0x0400417C RID: 16764
		private double lastHitTime;

		// Token: 0x0400417D RID: 16765
		private int hitCount;

		// Token: 0x0400417E RID: 16766
		private const byte MAX_SCORE = 10;

		// Token: 0x0400417F RID: 16767
		private const byte HIT = 11;
	}
}
