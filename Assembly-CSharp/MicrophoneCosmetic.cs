using System;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class MicrophoneCosmetic : MonoBehaviour
{
	// Token: 0x06000672 RID: 1650 RVA: 0x0008614C File Offset: 0x0008434C
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

	// Token: 0x06000673 RID: 1651 RVA: 0x000861CC File Offset: 0x000843CC
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

	// Token: 0x06000674 RID: 1652 RVA: 0x00034BDE File Offset: 0x00032DDE
	private void OnDisable()
	{
		this.audioSource.GTStop();
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x0008623C File Offset: 0x0008443C
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

	// Token: 0x06000676 RID: 1654 RVA: 0x00030607 File Offset: 0x0002E807
	private void OnAudioFilterRead(float[] data, int channels)
	{
	}

	// Token: 0x0400078F RID: 1935
	[SerializeField]
	private Transform mouthTransform;

	// Token: 0x04000790 RID: 1936
	[SerializeField]
	private Vector2 mouthProximityRampRange = new Vector2(0.6f, 0.3f);

	// Token: 0x04000791 RID: 1937
	private AudioSource audioSource;

	// Token: 0x04000792 RID: 1938
	private float[] zero = new float[1];
}
