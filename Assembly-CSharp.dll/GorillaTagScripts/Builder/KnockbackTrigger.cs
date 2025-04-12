using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A10 RID: 2576
	public class KnockbackTrigger : MonoBehaviour
	{
		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06004086 RID: 16518 RVA: 0x0005959A File Offset: 0x0005779A
		public bool TriggeredThisFrame
		{
			get
			{
				return this.lastTriggeredFrame == Time.frameCount;
			}
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x0016C37C File Offset: 0x0016A57C
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

		// Token: 0x06004088 RID: 16520 RVA: 0x000595A9 File Offset: 0x000577A9
		private void OnTriggerExit(Collider other)
		{
			if (!other.gameObject.IsOnLayer(UnityLayer.GorillaBodyCollider) && !other.gameObject.IsOnLayer(UnityLayer.GorillaHead) && !other.gameObject.IsOnLayer(UnityLayer.GorillaHand))
			{
				return;
			}
			this.collidersEntered.Remove(other);
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x000595E5 File Offset: 0x000577E5
		private void OnDisable()
		{
			this.collidersEntered.Clear();
		}

		// Token: 0x040041B3 RID: 16819
		[SerializeField]
		private BoxCollider triggerVolume;

		// Token: 0x040041B4 RID: 16820
		[SerializeField]
		private float knockbackVelocity;

		// Token: 0x040041B5 RID: 16821
		[SerializeField]
		private Vector3 localAxis;

		// Token: 0x040041B6 RID: 16822
		[SerializeField]
		private GameObject impactFX;

		// Token: 0x040041B7 RID: 16823
		[SerializeField]
		private bool onlySmallMonke;

		// Token: 0x040041B8 RID: 16824
		private int lastTriggeredFrame = -1;

		// Token: 0x040041B9 RID: 16825
		private List<Collider> collidersEntered = new List<Collider>(4);
	}
}
