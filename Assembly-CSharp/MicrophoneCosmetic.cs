using System;
using UnityEngine;

// Token: 0x020000EC RID: 236
public class MicrophoneCosmetic : MonoBehaviour
{
	// Token: 0x06000631 RID: 1585 RVA: 0x00023B98 File Offset: 0x00021D98
	private void Awake()
	{
		this.audioSource = base.GetComponent<AudioSource>();
		if (!Application.isEditor && Application.platform == RuntimePlatform.Android && Microphone.devices.Length != 0)
		{
			this.audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, 16000);
		}
		else
		{
			int sampleRate = AudioSettings.GetConfiguration().sampleRate;
			this.audioSource.clip = Microphone.Start(null, true, 10, sampleRate);
		}
		this.audioSource.loop = true;
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x00023C18 File Offset: 0x00021E18
	private void OnEnable()
	{
		int num = (Application.platform == RuntimePlatform.Android && Microphone.devices.Length != 0) ? Microphone.GetPosition(Microphone.devices[0]) : Microphone.GetPosition(null);
		num -= 10;
		if ((float)num < 0f)
		{
			num = this.audioSource.clip.samples + num - 1;
		}
		this.audioSource.GTPlay();
		this.audioSource.timeSamples = num;
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x00023C85 File Offset: 0x00021E85
	private void OnDisable()
	{
		this.audioSource.GTStop();
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x00023C94 File Offset: 0x00021E94
	private void Update()
	{
		Vector3 vector = this.mouthTransform.position - base.transform.position;
		float sqrMagnitude = vector.sqrMagnitude;
		float num = 0f;
		if (sqrMagnitude < this.mouthProximityRampRange.x * this.mouthProximityRampRange.x)
		{
			float magnitude = vector.magnitude;
			num = Mathf.InverseLerp(this.mouthProximityRampRange.x, this.mouthProximityRampRange.y, magnitude);
		}
		if (num != this.audioSource.volume)
		{
			this.audioSource.volume = num;
		}
		int num2 = this.audioSource.timeSamples -= 10;
		if ((float)num2 < 0f)
		{
			num2 = this.audioSource.clip.samples + num2 - 1;
		}
		this.audioSource.clip.SetData(this.zero, num2);
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x000023F4 File Offset: 0x000005F4
	private void OnAudioFilterRead(float[] data, int channels)
	{
	}

	// Token: 0x0400074E RID: 1870
	[SerializeField]
	private Transform mouthTransform;

	// Token: 0x0400074F RID: 1871
	[SerializeField]
	private Vector2 mouthProximityRampRange = new Vector2(0.6f, 0.3f);

	// Token: 0x04000750 RID: 1872
	private AudioSource audioSource;

	// Token: 0x04000751 RID: 1873
	private float[] zero = new float[1];
}
