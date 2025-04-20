using System;
using UnityEngine;

// Token: 0x020008CF RID: 2255
public class GrowUntilCollision : MonoBehaviour
{
	// Token: 0x060036AB RID: 13995 RVA: 0x00145034 File Offset: 0x00143234
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

	// Token: 0x060036AC RID: 13996 RVA: 0x00145084 File Offset: 0x00143284
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

	// Token: 0x060036AD RID: 13997 RVA: 0x0005410A File Offset: 0x0005230A
	private void OnTriggerEnter(Collider other)
	{
		this.tryToTrigger(base.transform.position, other.transform.position);
	}

	// Token: 0x060036AE RID: 13998 RVA: 0x0005410A File Offset: 0x0005230A
	private void OnTriggerExit(Collider other)
	{
		this.tryToTrigger(base.transform.position, other.transform.position);
	}

	// Token: 0x060036AF RID: 13999 RVA: 0x001450E8 File Offset: 0x001432E8
	private void OnCollisionEnter(Collision collision)
	{
		this.tryToTrigger(base.transform.position, collision.GetContact(0).point);
	}

	// Token: 0x060036B0 RID: 14000 RVA: 0x001450E8 File Offset: 0x001432E8
	private void OnCollisionExit(Collision collision)
	{
		this.tryToTrigger(base.transform.position, collision.GetContact(0).point);
	}

	// Token: 0x060036B1 RID: 14001 RVA: 0x00054128 File Offset: 0x00052328
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

	// Token: 0x060036B2 RID: 14002 RVA: 0x00145118 File Offset: 0x00143318
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

	// Token: 0x040038D3 RID: 14547
	[SerializeField]
	private float maxSize = 10f;

	// Token: 0x040038D4 RID: 14548
	[SerializeField]
	private float initialRadius = 1f;

	// Token: 0x040038D5 RID: 14549
	[SerializeField]
	private float minRetriggerTime = 1f;

	// Token: 0x040038D6 RID: 14550
	[SerializeField]
	private LightningDispatcherEvent colliderFound;

	// Token: 0x040038D7 RID: 14551
	private AudioSource audioSource;

	// Token: 0x040038D8 RID: 14552
	private float maxVolume;

	// Token: 0x040038D9 RID: 14553
	private float maxPitch;

	// Token: 0x040038DA RID: 14554
	private float timeSinceTrigger;
}
