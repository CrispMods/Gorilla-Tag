using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A3A RID: 2618
	public class KnockbackTrigger : MonoBehaviour
	{
		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x060041BF RID: 16831 RVA: 0x0005AF9C File Offset: 0x0005919C
		public bool TriggeredThisFrame
		{
			get
			{
				return this.lastTriggeredFrame == Time.frameCount;
			}
		}

		// Token: 0x060041C0 RID: 16832 RVA: 0x00173200 File Offset: 0x00171400
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

		// Token: 0x060041C1 RID: 16833 RVA: 0x0005AFAB File Offset: 0x000591AB
		private void OnTriggerExit(Collider other)
		{
			if (!other.gameObject.IsOnLayer(UnityLayer.GorillaBodyCollider) && !other.gameObject.IsOnLayer(UnityLayer.GorillaHead) && !other.gameObject.IsOnLayer(UnityLayer.GorillaHand))
			{
				return;
			}
			this.collidersEntered.Remove(other);
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x0005AFE7 File Offset: 0x000591E7
		private void OnDisable()
		{
			this.collidersEntered.Clear();
		}

		// Token: 0x0400429B RID: 17051
		[SerializeField]
		private BoxCollider triggerVolume;

		// Token: 0x0400429C RID: 17052
		[SerializeField]
		private float knockbackVelocity;

		// Token: 0x0400429D RID: 17053
		[SerializeField]
		private Vector3 localAxis;

		// Token: 0x0400429E RID: 17054
		[SerializeField]
		private GameObject impactFX;

		// Token: 0x0400429F RID: 17055
		[SerializeField]
		private bool onlySmallMonke;

		// Token: 0x040042A0 RID: 17056
		private int lastTriggeredFrame = -1;

		// Token: 0x040042A1 RID: 17057
		private List<Collider> collidersEntered = new List<Collider>(4);
	}
}
