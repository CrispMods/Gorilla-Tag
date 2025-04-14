using System;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A0D RID: 2573
	public class BuilderSmallHandTrigger : MonoBehaviour
	{
		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06004071 RID: 16497 RVA: 0x001322E2 File Offset: 0x001304E2
		public bool TriggeredThisFrame
		{
			get
			{
				return this.lastTriggeredFrame == Time.frameCount;
			}
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x001322F4 File Offset: 0x001304F4
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

		// Token: 0x04004192 RID: 16786
		[Tooltip("Optional timeline to play to animate the thing getting activated, play sound, particles, etc...")]
		public PlayableDirector timeline;

		// Token: 0x04004193 RID: 16787
		[Tooltip("Optional animation to play")]
		public Animation animation;

		// Token: 0x04004194 RID: 16788
		private int lastTriggeredFrame = -1;

		// Token: 0x04004195 RID: 16789
		public bool onlySmallHands;

		// Token: 0x04004196 RID: 16790
		[SerializeField]
		protected bool requireMinimumVelocity;

		// Token: 0x04004197 RID: 16791
		[SerializeField]
		protected float minimumVelocityMagnitude = 0.1f;

		// Token: 0x04004198 RID: 16792
		internal UnityEvent TriggeredEvent = new UnityEvent();
	}
}
