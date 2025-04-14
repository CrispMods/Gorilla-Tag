﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A0C RID: 2572
	public class BuilderProjectileTarget : MonoBehaviour, IBuilderPieceFunctional
	{
		// Token: 0x06004066 RID: 16486 RVA: 0x00132080 File Offset: 0x00130280
		private void Awake()
		{
			this.hitNotifier.OnProjectileHit += this.OnProjectileHit;
			foreach (Collider collider in this.colliders)
			{
				collider.contactOffset = 0.0001f;
			}
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x001320EC File Offset: 0x001302EC
		private void OnDestroy()
		{
			this.hitNotifier.OnProjectileHit -= this.OnProjectileHit;
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x00132105 File Offset: 0x00130305
		private void OnDisable()
		{
			this.hitCount = 0;
			if (this.scoreText != null)
			{
				this.scoreText.text = this.hitCount.ToString("D2");
			}
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x00132138 File Offset: 0x00130338
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

		// Token: 0x0600406A RID: 16490 RVA: 0x0013219B File Offset: 0x0013039B
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

		// Token: 0x0600406B RID: 16491 RVA: 0x001321D4 File Offset: 0x001303D4
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

		// Token: 0x0600406C RID: 16492 RVA: 0x00132242 File Offset: 0x00130442
		public bool IsStateValid(byte state)
		{
			return state <= 11;
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x0013224C File Offset: 0x0013044C
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

		// Token: 0x0600406E RID: 16494 RVA: 0x000023F4 File Offset: 0x000005F4
		public void FunctionalPieceUpdate()
		{
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x001322C8 File Offset: 0x001304C8
		public float GetInteractionDistace()
		{
			return 20f;
		}

		// Token: 0x04004187 RID: 16775
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x04004188 RID: 16776
		[SerializeField]
		private SlingshotProjectileHitNotifier hitNotifier;

		// Token: 0x04004189 RID: 16777
		[SerializeField]
		protected float hitCooldown = 2f;

		// Token: 0x0400418A RID: 16778
		[Tooltip("Optional Sounds to play on hit")]
		[SerializeField]
		protected SoundBankPlayer hitSoundbank;

		// Token: 0x0400418B RID: 16779
		[Tooltip("Optional Sounds to play on hit")]
		[SerializeField]
		protected Animation hitAnimation;

		// Token: 0x0400418C RID: 16780
		[SerializeField]
		protected List<Collider> colliders;

		// Token: 0x0400418D RID: 16781
		[SerializeField]
		private TMP_Text scoreText;

		// Token: 0x0400418E RID: 16782
		private double lastHitTime;

		// Token: 0x0400418F RID: 16783
		private int hitCount;

		// Token: 0x04004190 RID: 16784
		private const byte MAX_SCORE = 10;

		// Token: 0x04004191 RID: 16785
		private const byte HIT = 11;
	}
}
