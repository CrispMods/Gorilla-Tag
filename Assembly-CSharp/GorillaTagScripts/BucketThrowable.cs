using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x0200097F RID: 2431
	public class BucketThrowable : MonoBehaviour
	{
		// Token: 0x06003B6B RID: 15211 RVA: 0x001119D8 File Offset: 0x0010FBD8
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

		// Token: 0x06003B6C RID: 15212 RVA: 0x00111A3C File Offset: 0x0010FC3C
		private void TriggerEvent()
		{
			UnityAction<bool> onTriggerEntered = this.OnTriggerEntered;
			if (onTriggerEntered == null)
			{
				return;
			}
			onTriggerEntered(false);
		}

		// Token: 0x04003C99 RID: 15513
		public GameObject projectilePrefab;

		// Token: 0x04003C9A RID: 15514
		[Range(0f, 1f)]
		public float weightedChance = 1f;

		// Token: 0x04003C9B RID: 15515
		public AudioSource audioSource;

		// Token: 0x04003C9C RID: 15516
		public AudioClip triggerClip;

		// Token: 0x04003C9D RID: 15517
		public bool destroyAfterRelease;

		// Token: 0x04003C9E RID: 15518
		public float destroyAfterSeconds = -1f;

		// Token: 0x04003C9F RID: 15519
		public UnityAction<bool> OnTriggerEntered;
	}
}
