using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x02000982 RID: 2434
	public class BucketThrowable : MonoBehaviour
	{
		// Token: 0x06003B77 RID: 15223 RVA: 0x00111FA0 File Offset: 0x001101A0
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

		// Token: 0x06003B78 RID: 15224 RVA: 0x00112004 File Offset: 0x00110204
		private void TriggerEvent()
		{
			UnityAction<bool> onTriggerEntered = this.OnTriggerEntered;
			if (onTriggerEntered == null)
			{
				return;
			}
			onTriggerEntered(false);
		}

		// Token: 0x04003CAB RID: 15531
		public GameObject projectilePrefab;

		// Token: 0x04003CAC RID: 15532
		[Range(0f, 1f)]
		public float weightedChance = 1f;

		// Token: 0x04003CAD RID: 15533
		public AudioSource audioSource;

		// Token: 0x04003CAE RID: 15534
		public AudioClip triggerClip;

		// Token: 0x04003CAF RID: 15535
		public bool destroyAfterRelease;

		// Token: 0x04003CB0 RID: 15536
		public float destroyAfterSeconds = -1f;

		// Token: 0x04003CB1 RID: 15537
		public UnityAction<bool> OnTriggerEntered;
	}
}
