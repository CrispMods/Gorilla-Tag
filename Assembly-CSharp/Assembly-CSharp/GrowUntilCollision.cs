using System;
using UnityEngine;

// Token: 0x020008B6 RID: 2230
public class GrowUntilCollision : MonoBehaviour
{
	// Token: 0x060035EF RID: 13807 RVA: 0x000FF9BC File Offset: 0x000FDBBC
	private void Start()
	{
		this.audioSource = base.GetComponent<AudioSource>();
		if (this.audioSource != null)
		{
			this.maxVolume = this.audioSource.volume;
			this.maxPitch = this.audioSource.pitch;
		}
		this.zero();
	}

	// Token: 0x060035F0 RID: 13808 RVA: 0x000FFA0C File Offset: 0x000FDC0C
	private void zero()
	{
		base.transform.localScale = Vector3.one * this.initialRadius;
		if (this.audioSource != null)
		{
			this.audioSource.volume = 0f;
			this.audioSource.pitch = 1f;
		}
		this.timeSinceTrigger = 0f;
	}

	// Token: 0x060035F1 RID: 13809 RVA: 0x000FFA6D File Offset: 0x000FDC6D
	private void OnTriggerEnter(Collider other)
	{
		this.tryToTrigger(base.transform.position, other.transform.position);
	}

	// Token: 0x060035F2 RID: 13810 RVA: 0x000FFA6D File Offset: 0x000FDC6D
	private void OnTriggerExit(Collider other)
	{
		this.tryToTrigger(base.transform.position, other.transform.position);
	}

	// Token: 0x060035F3 RID: 13811 RVA: 0x000FFA8C File Offset: 0x000FDC8C
	private void OnCollisionEnter(Collision collision)
	{
		this.tryToTrigger(base.transform.position, collision.GetContact(0).point);
	}

	// Token: 0x060035F4 RID: 13812 RVA: 0x000FFABC File Offset: 0x000FDCBC
	private void OnCollisionExit(Collision collision)
	{
		this.tryToTrigger(base.transform.position, collision.GetContact(0).point);
	}

	// Token: 0x060035F5 RID: 13813 RVA: 0x000FFAE9 File Offset: 0x000FDCE9
	private void tryToTrigger(Vector3 p1, Vector3 p2)
	{
		if (this.timeSinceTrigger > this.minRetriggerTime)
		{
			if (this.colliderFound != null)
			{
				this.colliderFound.Invoke(p1, p2);
			}
			this.zero();
		}
	}

	// Token: 0x060035F6 RID: 13814 RVA: 0x000FFB14 File Offset: 0x000FDD14
	private void Update()
	{
		float num = Mathf.Max(new float[]
		{
			base.transform.lossyScale.x,
			base.transform.lossyScale.y,
			base.transform.lossyScale.z
		});
		if (base.transform.localScale.x < this.maxSize * num)
		{
			base.transform.localScale += Vector3.one * Time.deltaTime * num;
			if (this.audioSource != null)
			{
				this.audioSource.volume = this.maxVolume * (base.transform.localScale.x / this.maxSize);
				this.audioSource.pitch = 1f + this.maxPitch * (base.transform.localScale.x / this.maxSize);
			}
		}
		this.timeSinceTrigger += Time.deltaTime;
	}

	// Token: 0x04003824 RID: 14372
	[SerializeField]
	private float maxSize = 10f;

	// Token: 0x04003825 RID: 14373
	[SerializeField]
	private float initialRadius = 1f;

	// Token: 0x04003826 RID: 14374
	[SerializeField]
	private float minRetriggerTime = 1f;

	// Token: 0x04003827 RID: 14375
	[SerializeField]
	private LightningDispatcherEvent colliderFound;

	// Token: 0x04003828 RID: 14376
	private AudioSource audioSource;

	// Token: 0x04003829 RID: 14377
	private float maxVolume;

	// Token: 0x0400382A RID: 14378
	private float maxPitch;

	// Token: 0x0400382B RID: 14379
	private float timeSinceTrigger;
}
