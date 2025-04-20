using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A38 RID: 2616
	public class BuilderSmallMonkeTrigger : MonoBehaviour
	{
		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x060041AD RID: 16813 RVA: 0x0005AF16 File Offset: 0x00059116
		public int overlapCount
		{
			get
			{
				return this.overlappingColliders.Count;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x060041AE RID: 16814 RVA: 0x0005AF23 File Offset: 0x00059123
		public bool TriggeredThisFrame
		{
			get
			{
				return this.lastTriggeredFrame == Time.frameCount;
			}
		}

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x060041AF RID: 16815 RVA: 0x001728A8 File Offset: 0x00170AA8
		// (remove) Token: 0x060041B0 RID: 16816 RVA: 0x001728E0 File Offset: 0x00170AE0
		public event Action onTriggerFirstEntered;

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x060041B1 RID: 16817 RVA: 0x00172918 File Offset: 0x00170B18
		// (remove) Token: 0x060041B2 RID: 16818 RVA: 0x00172950 File Offset: 0x00170B50
		public event Action onTriggerLastExited;

		// Token: 0x060041B3 RID: 16819 RVA: 0x00172988 File Offset: 0x00170B88
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

		// Token: 0x060041B4 RID: 16820 RVA: 0x00172AA4 File Offset: 0x00170CA4
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

		// Token: 0x060041B5 RID: 16821 RVA: 0x0005AF32 File Offset: 0x00059132
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

		// Token: 0x04004281 RID: 17025
		private int lastTriggeredFrame = -1;

		// Token: 0x04004282 RID: 17026
		private List<Collider> overlappingColliders = new List<Collider>(20);
	}
}
