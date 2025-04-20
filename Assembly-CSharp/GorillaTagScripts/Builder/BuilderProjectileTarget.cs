using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A36 RID: 2614
	public class BuilderProjectileTarget : MonoBehaviour, IBuilderPieceFunctional
	{
		// Token: 0x0600419F RID: 16799 RVA: 0x00172588 File Offset: 0x00170788
		private void Awake()
		{
			this.hitNotifier.OnProjectileHit += this.OnProjectileHit;
			foreach (Collider collider in this.colliders)
			{
				collider.contactOffset = 0.0001f;
			}
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x0005AE3C File Offset: 0x0005903C
		private void OnDestroy()
		{
			this.hitNotifier.OnProjectileHit -= this.OnProjectileHit;
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x0005AE55 File Offset: 0x00059055
		private void OnDisable()
		{
			this.hitCount = 0;
			if (this.scoreText != null)
			{
				this.scoreText.text = this.hitCount.ToString("D2");
			}
		}

		// Token: 0x060041A2 RID: 16802 RVA: 0x001725F4 File Offset: 0x001707F4
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

		// Token: 0x060041A3 RID: 16803 RVA: 0x0005AE87 File Offset: 0x00059087
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

		// Token: 0x060041A4 RID: 16804 RVA: 0x00172658 File Offset: 0x00170858
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

		// Token: 0x060041A5 RID: 16805 RVA: 0x0005AEBE File Offset: 0x000590BE
		public bool IsStateValid(byte state)
		{
			return state <= 11;
		}

		// Token: 0x060041A6 RID: 16806 RVA: 0x001726C8 File Offset: 0x001708C8
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

		// Token: 0x060041A7 RID: 16807 RVA: 0x00030607 File Offset: 0x0002E807
		public void FunctionalPieceUpdate()
		{
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x0005AEC8 File Offset: 0x000590C8
		public float GetInteractionDistace()
		{
			return 20f;
		}

		// Token: 0x0400426F RID: 17007
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x04004270 RID: 17008
		[SerializeField]
		private SlingshotProjectileHitNotifier hitNotifier;

		// Token: 0x04004271 RID: 17009
		[SerializeField]
		protected float hitCooldown = 2f;

		// Token: 0x04004272 RID: 17010
		[Tooltip("Optional Sounds to play on hit")]
		[SerializeField]
		protected SoundBankPlayer hitSoundbank;

		// Token: 0x04004273 RID: 17011
		[Tooltip("Optional Sounds to play on hit")]
		[SerializeField]
		protected Animation hitAnimation;

		// Token: 0x04004274 RID: 17012
		[SerializeField]
		protected List<Collider> colliders;

		// Token: 0x04004275 RID: 17013
		[SerializeField]
		private TMP_Text scoreText;

		// Token: 0x04004276 RID: 17014
		private double lastHitTime;

		// Token: 0x04004277 RID: 17015
		private int hitCount;

		// Token: 0x04004278 RID: 17016
		private const byte MAX_SCORE = 10;

		// Token: 0x04004279 RID: 17017
		private const byte HIT = 11;
	}
}
