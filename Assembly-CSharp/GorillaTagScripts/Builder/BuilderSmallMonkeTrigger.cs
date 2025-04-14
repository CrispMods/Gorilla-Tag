using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A0B RID: 2571
	public class BuilderSmallMonkeTrigger : MonoBehaviour
	{
		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06004068 RID: 16488 RVA: 0x00131EB2 File Offset: 0x001300B2
		public int overlapCount
		{
			get
			{
				return this.overlappingColliders.Count;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06004069 RID: 16489 RVA: 0x00131EBF File Offset: 0x001300BF
		public bool TriggeredThisFrame
		{
			get
			{
				return this.lastTriggeredFrame == Time.frameCount;
			}
		}

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x0600406A RID: 16490 RVA: 0x00131ED0 File Offset: 0x001300D0
		// (remove) Token: 0x0600406B RID: 16491 RVA: 0x00131F08 File Offset: 0x00130108
		public event Action onTriggerFirstEntered;

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x0600406C RID: 16492 RVA: 0x00131F40 File Offset: 0x00130140
		// (remove) Token: 0x0600406D RID: 16493 RVA: 0x00131F78 File Offset: 0x00130178
		public event Action onTriggerLastExited;

		// Token: 0x0600406E RID: 16494 RVA: 0x00131FB0 File Offset: 0x001301B0
		public void ValidateOverlappingColliders()
		{
			for (int i = this.overlappingColliders.Count - 1; i >= 0; i--)
			{
				if (this.overlappingColliders[i] == null || !this.overlappingColliders[i].gameObject.activeInHierarchy || !this.overlappingColliders[i].enabled)
				{
					this.overlappingColliders.RemoveAt(i);
				}
				else
				{
					VRRig vrrig = this.overlappingColliders[i].attachedRigidbody.gameObject.GetComponent<VRRig>();
					if (vrrig == null)
					{
						if (GTPlayer.Instance.bodyCollider == this.overlappingColliders[i] || GTPlayer.Instance.headCollider == this.overlappingColliders[i])
						{
							vrrig = GorillaTagger.Instance.offlineVRRig;
						}
						else
						{
							this.overlappingColliders.RemoveAt(i);
						}
					}
					if (vrrig != null && (double)vrrig.scaleFactor > 0.99)
					{
						this.overlappingColliders.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x001320CC File Offset: 0x001302CC
		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody == null)
			{
				return;
			}
			VRRig vrrig = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
			if (vrrig == null)
			{
				if (!(GTPlayer.Instance.bodyCollider == other) && !(GTPlayer.Instance.headCollider == other))
				{
					return;
				}
				vrrig = GorillaTagger.Instance.offlineVRRig;
			}
			if ((double)vrrig.scaleFactor > 0.99)
			{
				return;
			}
			bool flag = this.overlappingColliders.Count == 0;
			if (!this.overlappingColliders.Contains(other))
			{
				this.overlappingColliders.Add(other);
			}
			this.lastTriggeredFrame = Time.frameCount;
			if (flag)
			{
				Action action = this.onTriggerFirstEntered;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x0013218C File Offset: 0x0013038C
		private void OnTriggerExit(Collider other)
		{
			if (this.overlappingColliders.Remove(other) && this.overlappingColliders.Count == 0)
			{
				Action action = this.onTriggerLastExited;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		// Token: 0x04004187 RID: 16775
		private int lastTriggeredFrame = -1;

		// Token: 0x04004188 RID: 16776
		private List<Collider> overlappingColliders = new List<Collider>(20);
	}
}
