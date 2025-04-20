using System;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class AudioAnimator : MonoBehaviour
{
	// Token: 0x06000873 RID: 2163 RVA: 0x00035F63 File Offset: 0x00034163
	private void Start()
	{
		if (!this.didInitBaseVolume)
		{
			this.InitBaseVolume();
		}
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x0008E1A8 File Offset: 0x0008C3A8
	private void InitBaseVolume()
	{
		for (int i = 0; i < this.targets.Length; i++)
		{
			this.targets[i].baseVolume = this.targets[i].audioSource.volume;
		}
		this.didInitBaseVolume = true;
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x00035F73 File Offset: 0x00034173
	public void UpdateValue(float value, bool ignoreSmoothing = false)
	{
		this.UpdatePitchAndVolume(value, value, ignoreSmoothing);
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x0008E1F8 File Offset: 0x0008C3F8
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

	// Token: 0x040009D6 RID: 2518
	private bool didInitBaseVolume;

	// Token: 0x040009D7 RID: 2519
	[SerializeField]
	private AudioAnimator.AudioTarget[] targets;

	// Token: 0x02000147 RID: 327
	[Serializable]
	private struct AudioTarget
	{
		// Token: 0x040009D8 RID: 2520
		public AudioSource audioSource;

		// Token: 0x040009D9 RID: 2521
		public AnimationCurve pitchCurve;

		// Token: 0x040009DA RID: 2522
		public AnimationCurve volumeCurve;

		// Token: 0x040009DB RID: 2523
		[NonSerialized]
		public float baseVolume;

		// Token: 0x040009DC RID: 2524
		public float riseSmoothing;

		// Token: 0x040009DD RID: 2525
		public float lowerSmoothing;
	}
}
