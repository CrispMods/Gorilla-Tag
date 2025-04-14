using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A0E RID: 2574
	public class BuilderSmallMonkeTrigger : MonoBehaviour
	{
		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06004074 RID: 16500 RVA: 0x0013247A File Offset: 0x0013067A
		public int overlapCount
		{
			get
			{
				return this.overlappingColliders.Count;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06004075 RID: 16501 RVA: 0x00132487 File Offset: 0x00130687
		public bool TriggeredThisFrame
		{
			get
			{
				return this.lastTriggeredFrame == Time.frameCount;
			}
		}

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06004076 RID: 16502 RVA: 0x00132498 File Offset: 0x00130698
		// (remove) Token: 0x06004077 RID: 16503 RVA: 0x001324D0 File Offset: 0x001306D0
		public event Action onTriggerFirstEntered;

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06004078 RID: 16504 RVA: 0x00132508 File Offset: 0x00130708
		// (remove) Token: 0x06004079 RID: 16505 RVA: 0x00132540 File Offset: 0x00130740
		public event Action onTriggerLastExited;

		// Token: 0x0600407A RID: 16506 RVA: 0x00132578 File Offset: 0x00130778
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

		// Token: 0x0600407B RID: 16507 RVA: 0x00132694 File Offset: 0x00130894
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

		// Token: 0x0600407C RID: 16508 RVA: 0x00132754 File Offset: 0x00130954
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

		// Token: 0x04004199 RID: 16793
		private int lastTriggeredFrame = -1;

		// Token: 0x0400419A RID: 16794
		private List<Collider> overlappingColliders = new List<Collider>(20);
	}
}
