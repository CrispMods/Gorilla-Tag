using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009A5 RID: 2469
	public class BucketThrowable : MonoBehaviour
	{
		// Token: 0x06003C83 RID: 15491 RVA: 0x00154C80 File Offset: 0x00152E80
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Gorilla Head"))
			{
				if (this.audioSource && this.triggerClip)
				{
					this.audioSource.GTPlayOneShot(this.triggerClip, 1f);
				}
				base.Invoke("TriggerEvent", 0.25f);
			}
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x000577F1 File Offset: 0x000559F1
		private void TriggerEvent()
		{
			UnityAction<bool> onTriggerEntered = this.OnTriggerEntered;
			if (onTriggerEntered == null)
			{
				return;
			}
			onTriggerEntered(false);
		}

		// Token: 0x04003D73 RID: 15731
		public GameObject projectilePrefab;

		// Token: 0x04003D74 RID: 15732
		[Range(0f, 1f)]
		public float weightedChance = 1f;

		// Token: 0x04003D75 RID: 15733
		public AudioSource audioSource;

		// Token: 0x04003D76 RID: 15734
		public AudioClip triggerClip;

		// Token: 0x04003D77 RID: 15735
		public bool destroyAfterRelease;

		// Token: 0x04003D78 RID: 15736
		public float destroyAfterSeconds = -1f;

		// Token: 0x04003D79 RID: 15737
		public UnityAction<bool> OnTriggerEntered;
	}
}
