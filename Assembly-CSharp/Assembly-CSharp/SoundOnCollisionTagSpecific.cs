using System;
using UnityEngine;

// Token: 0x020001F5 RID: 501
public class SoundOnCollisionTagSpecific : MonoBehaviour
{
	// Token: 0x06000BCB RID: 3019 RVA: 0x0003EAE8 File Offset: 0x0003CCE8
	private void OnTriggerEnter(Collider collider)
	{
		if (Time.time > this.nextSound && collider.gameObject.CompareTag(this.tagName))
		{
			this.nextSound = Time.time + this.noiseCooldown;
			this.audioSource.GTPlayOneShot(this.collisionSounds[Random.Range(0, this.collisionSounds.Length)], 0.5f);
		}
	}

	// Token: 0x04000E2A RID: 3626
	public string tagName;

	// Token: 0x04000E2B RID: 3627
	public float noiseCooldown = 1f;

	// Token: 0x04000E2C RID: 3628
	private float nextSound;

	// Token: 0x04000E2D RID: 3629
	public AudioSource audioSource;

	// Token: 0x04000E2E RID: 3630
	public AudioClip[] collisionSounds;
}
