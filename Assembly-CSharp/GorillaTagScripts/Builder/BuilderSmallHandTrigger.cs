using System;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A37 RID: 2615
	public class BuilderSmallHandTrigger : MonoBehaviour
	{
		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x060041AA RID: 16810 RVA: 0x0005AEE2 File Offset: 0x000590E2
		public bool TriggeredThisFrame
		{
			get
			{
				return this.lastTriggeredFrame == Time.frameCount;
			}
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x00172744 File Offset: 0x00170944
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

		// Token: 0x0400427A RID: 17018
		[Tooltip("Optional timeline to play to animate the thing getting activated, play sound, particles, etc...")]
		public PlayableDirector timeline;

		// Token: 0x0400427B RID: 17019
		[Tooltip("Optional animation to play")]
		public Animation animation;

		// Token: 0x0400427C RID: 17020
		private int lastTriggeredFrame = -1;

		// Token: 0x0400427D RID: 17021
		public bool onlySmallHands;

		// Token: 0x0400427E RID: 17022
		[SerializeField]
		protected bool requireMinimumVelocity;

		// Token: 0x0400427F RID: 17023
		[SerializeField]
		protected float minimumVelocityMagnitude = 0.1f;

		// Token: 0x04004280 RID: 17024
		internal UnityEvent TriggeredEvent = new UnityEvent();
	}
}
