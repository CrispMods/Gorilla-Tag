using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A0D RID: 2573
	public class KnockbackTrigger : MonoBehaviour
	{
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x0600407A RID: 16506 RVA: 0x001328F3 File Offset: 0x00130AF3
		public bool TriggeredThisFrame
		{
			get
			{
				return this.lastTriggeredFrame == Time.frameCount;
			}
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x00132904 File Offset: 0x00130B04
		private void OnTriggerEnter(Collider other)
		{
			if (!other.gameObject.IsOnLayer(UnityLayer.GorillaBodyCollider) && !other.gameObject.IsOnLayer(UnityLayer.GorillaHead) && !other.gameObject.IsOnLayer(UnityLayer.GorillaHand))
			{
				return;
			}
			if (this.onlySmallMonke && (double)VRRigCache.Instance.localRig.Rig.scaleFactor > 0.99)
			{
				return;
			}
			this.collidersEntered.Add(other);
			if (this.collidersEntered.Count > 1)
			{
				return;
			}
			Vector3 vector = this.triggerVolume.ClosestPoint(GorillaTagger.Instance.headCollider.transform.position);
			Vector3 vector2 = vector - base.transform.TransformPoint(this.triggerVolume.center);
			vector2 -= Vector3.Project(vector2, base.transform.TransformDirection(this.localAxis));
			float magnitude = vector2.magnitude;
			Vector3 direction = Vector3.up;
			if (magnitude >= 0.01f)
			{
				direction = vector2 / magnitude;
			}
			GTPlayer.Instance.SetMaximumSlipThisFrame();
			GTPlayer.Instance.ApplyKnockback(direction, this.knockbackVelocity, false);
			if (this.impactFX != null)
			{
				ObjectPools.instance.Instantiate(this.impactFX, vector);
			}
			GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength / 2f, Time.fixedDeltaTime);
			GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength / 2f, Time.fixedDeltaTime);
			this.lastTriggeredFrame = Time.frameCount;
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x00132A82 File Offset: 0x00130C82
		private void OnTriggerExit(Collider other)
		{
			if (!other.gameObject.IsOnLayer(UnityLayer.GorillaBodyCollider) && !other.gameObject.IsOnLayer(UnityLayer.GorillaHead) && !other.gameObject.IsOnLayer(UnityLayer.GorillaHand))
			{
				return;
			}
			this.collidersEntered.Remove(other);
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x00132ABE File Offset: 0x00130CBE
		private void OnDisable()
		{
			this.collidersEntered.Clear();
		}

		// Token: 0x040041A1 RID: 16801
		[SerializeField]
		private BoxCollider triggerVolume;

		// Token: 0x040041A2 RID: 16802
		[SerializeField]
		private float knockbackVelocity;

		// Token: 0x040041A3 RID: 16803
		[SerializeField]
		private Vector3 localAxis;

		// Token: 0x040041A4 RID: 16804
		[SerializeField]
		private GameObject impactFX;

		// Token: 0x040041A5 RID: 16805
		[SerializeField]
		private bool onlySmallMonke;

		// Token: 0x040041A6 RID: 16806
		private int lastTriggeredFrame = -1;

		// Token: 0x040041A7 RID: 16807
		private List<Collider> collidersEntered = new List<Collider>(4);
	}
}
