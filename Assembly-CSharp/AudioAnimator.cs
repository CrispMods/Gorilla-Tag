using System;
using UnityEngine;

// Token: 0x0200013C RID: 316
public class AudioAnimator : MonoBehaviour
{
	// Token: 0x0600082F RID: 2095 RVA: 0x0002CEEE File Offset: 0x0002B0EE
	private void Start()
	{
		if (!this.didInitBaseVolume)
		{
			this.InitBaseVolume();
		}
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x0002CF00 File Offset: 0x0002B100
	private void InitBaseVolume()
	{
		for (int i = 0; i < this.targets.Length; i++)
		{
			this.targets[i].baseVolume = this.targets[i].audioSource.volume;
		}
		this.didInitBaseVolume = true;
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x0002CF4E File Offset: 0x0002B14E
	public void UpdateValue(float value, bool ignoreSmoothing = false)
	{
		this.UpdatePitchAndVolume(value, value, ignoreSmoothing);
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x0002CF5C File Offset: 0x0002B15C
	public void UpdatePitchAndVolume(float pitchValue, float volumeValue, bool ignoreSmoothing = false)
	{
		if (!this.didInitBaseVolume)
		{
			this.InitBaseVolume();
		}
		for (int i = 0; i < this.targets.Length; i++)
		{
			AudioAnimator.AudioTarget audioTarget = this.targets[i];
			float p = audioTarget.pitchCurve.Evaluate(pitchValue);
			float pitch = Mathf.Pow(1.05946f, p);
			audioTarget.audioSource.pitch = pitch;
			float num = audioTarget.volumeCurve.Evaluate(volumeValue);
			float volume = audioTarget.audioSource.volume;
			float num2 = audioTarget.baseVolume * num;
			if (ignoreSmoothing)
			{
				audioTarget.audioSource.volume = num2;
			}
			else if (volume > num2)
			{
				audioTarget.audioSource.volume = Mathf.MoveTowards(audioTarget.audioSource.volume, audioTarget.baseVolume * num, (1f - audioTarget.lowerSmoothing) * audioTarget.baseVolume * Time.deltaTime * 90f);
			}
			else
			{
				audioTarget.audioSource.volume = Mathf.MoveTowards(audioTarget.audioSource.volume, audioTarget.baseVolume * num, (1f - audioTarget.riseSmoothing) * audioTarget.baseVolume * Time.deltaTime * 90f);
			}
		}
	}

	// Token: 0x04000993 RID: 2451
	private bool didInitBaseVolume;

	// Token: 0x04000994 RID: 2452
	[SerializeField]
	private AudioAnimator.AudioTarget[] targets;

	// Token: 0x0200013D RID: 317
	[Serializable]
	private struct AudioTarget
	{
		// Token: 0x04000995 RID: 2453
		public AudioSource audioSource;

		// Token: 0x04000996 RID: 2454
		public AnimationCurve pitchCurve;

		// Token: 0x04000997 RID: 2455
		public AnimationCurve volumeCurve;

		// Token: 0x04000998 RID: 2456
		[NonSerialized]
		public float baseVolume;

		// Token: 0x04000999 RID: 2457
		public float riseSmoothing;

		// Token: 0x0400099A RID: 2458
		public float lowerSmoothing;
	}
}
