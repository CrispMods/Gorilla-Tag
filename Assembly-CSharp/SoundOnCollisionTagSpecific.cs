using System;
using UnityEngine;

// Token: 0x02000200 RID: 512
public class SoundOnCollisionTagSpecific : MonoBehaviour
{
	// Token: 0x06000C14 RID: 3092 RVA: 0x0009D1F4 File Offset: 0x0009B3F4
	private void OnTriggerEnter(Collider collider)
	{
		if (Time.time > this.nextSound && collider.gameObject.CompareTag(this.tagName))
		{
			this.nextSound = Time.time + this.noiseCooldown;
			this.audioSource.GTPlayOneShot(this.collisionSounds[UnityEngine.Random.Range(0, this.collisionSounds.Length)], 0.5f);
		}
	}

	// Token: 0x04000E6F RID: 3695
	public string tagName;

	// Token: 0x04000E70 RID: 3696
	public float noiseCooldown = 1f;

	// Token: 0x04000E71 RID: 3697
	private float nextSound;

	// Token: 0x04000E72 RID: 3698
	public AudioSource audioSource;

	// Token: 0x04000E73 RID: 3699
	public AudioClip[] collisionSounds;
}
