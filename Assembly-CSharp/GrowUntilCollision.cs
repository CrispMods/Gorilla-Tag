using System;
using UnityEngine;

// Token: 0x020008B3 RID: 2227
public class GrowUntilCollision : MonoBehaviour
{
	// Token: 0x060035E3 RID: 13795 RVA: 0x000FF3F4 File Offset: 0x000FD5F4
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

	// Token: 0x060035E4 RID: 13796 RVA: 0x000FF444 File Offset: 0x000FD644
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

	// Token: 0x060035E5 RID: 13797 RVA: 0x000FF4A5 File Offset: 0x000FD6A5
	private void OnTriggerEnter(Collider other)
	{
		this.tryToTrigger(base.transform.position, other.transform.position);
	}

	// Token: 0x060035E6 RID: 13798 RVA: 0x000FF4A5 File Offset: 0x000FD6A5
	private void OnTriggerExit(Collider other)
	{
		this.tryToTrigger(base.transform.position, other.transform.position);
	}

	// Token: 0x060035E7 RID: 13799 RVA: 0x000FF4C4 File Offset: 0x000FD6C4
	private void OnCollisionEnter(Collision collision)
	{
		this.tryToTrigger(base.transform.position, collision.GetContact(0).point);
	}

	// Token: 0x060035E8 RID: 13800 RVA: 0x000FF4F4 File Offset: 0x000FD6F4
	private void OnCollisionExit(Collision collision)
	{
		this.tryToTrigger(base.transform.position, collision.GetContact(0).point);
	}

	// Token: 0x060035E9 RID: 13801 RVA: 0x000FF521 File Offset: 0x000FD721
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

	// Token: 0x060035EA RID: 13802 RVA: 0x000FF54C File Offset: 0x000FD74C
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

	// Token: 0x04003812 RID: 14354
	[SerializeField]
	private float maxSize = 10f;

	// Token: 0x04003813 RID: 14355
	[SerializeField]
	private float initialRadius = 1f;

	// Token: 0x04003814 RID: 14356
	[SerializeField]
	private float minRetriggerTime = 1f;

	// Token: 0x04003815 RID: 14357
	[SerializeField]
	private LightningDispatcherEvent colliderFound;

	// Token: 0x04003816 RID: 14358
	private AudioSource audioSource;

	// Token: 0x04003817 RID: 14359
	private float maxVolume;

	// Token: 0x04003818 RID: 14360
	private float maxPitch;

	// Token: 0x04003819 RID: 14361
	private float timeSinceTrigger;
}
