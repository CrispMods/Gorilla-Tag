using System;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A0A RID: 2570
	public class BuilderSmallHandTrigger : MonoBehaviour
	{
		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06004065 RID: 16485 RVA: 0x00131D1A File Offset: 0x0012FF1A
		public bool TriggeredThisFrame
		{
			get
			{
				return this.lastTriggeredFrame == Time.frameCount;
			}
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x00131D2C File Offset: 0x0012FF2C
		private void OnTriggerEnter(Collider other)
		{
			if (!base.enabled)
			{
				return;
			}
			GorillaTriggerColliderHandIndicator componentInParent = other.GetComponentInParent<GorillaTriggerColliderHandIndicator>();
			if (componentInParent == null)
			{
				return;
			}
			if (this.onlySmallHands && (double)VRRigCache.Instance.localRig.Rig.scaleFactor > 0.99)
			{
				return;
			}
			if (this.requireMinimumVelocity)
			{
				float num = this.minimumVelocityMagnitude * GorillaTagger.Instance.offlineVRRig.scaleFactor;
				if ((componentInParent.isLeftHand ? GTPlayer.Instance.leftHandCenterVelocityTracker : GTPlayer.Instance.rightHandCenterVelocityTracker).GetAverageVelocity(true, 0.1f, false).sqrMagnitude < num * num)
				{
					return;
				}
			}
			GorillaTagger.Instance.StartVibration(componentInParent.isLeftHand, GorillaTagger.Instance.tapHapticStrength, GorillaTagger.Instance.tapHapticDuration * 1.5f);
			this.lastTriggeredFrame = Time.frameCount;
			UnityEvent triggeredEvent = this.TriggeredEvent;
			if (triggeredEvent != null)
			{
				triggeredEvent.Invoke();
			}
			if (this.timeline != null && (this.timeline.time == 0.0 || this.timeline.time >= this.timeline.duration))
			{
				this.timeline.Play();
			}
			if (this.animation != null && this.animation.clip != null)
			{
				this.animation.Play();
			}
		}

		// Token: 0x04004180 RID: 16768
		[Tooltip("Optional timeline to play to animate the thing getting activated, play sound, particles, etc...")]
		public PlayableDirector timeline;

		// Token: 0x04004181 RID: 16769
		[Tooltip("Optional animation to play")]
		public Animation animation;

		// Token: 0x04004182 RID: 16770
		private int lastTriggeredFrame = -1;

		// Token: 0x04004183 RID: 16771
		public bool onlySmallHands;

		// Token: 0x04004184 RID: 16772
		[SerializeField]
		protected bool requireMinimumVelocity;

		// Token: 0x04004185 RID: 16773
		[SerializeField]
		protected float minimumVelocityMagnitude = 0.1f;

		// Token: 0x04004186 RID: 16774
		internal UnityEvent TriggeredEvent = new UnityEvent();
	}
}
