using System;
using Photon.Voice;
using Photon.Voice.Unity;
using UnityEngine;

// Token: 0x02000203 RID: 515
public class SpeakerVoiceLoudnessAudioOut : UnityAudioOut
{
	// Token: 0x06000C1B RID: 3099 RVA: 0x0003878A File Offset: 0x0003698A
	public SpeakerVoiceLoudnessAudioOut(SpeakerVoiceToLoudness speaker, AudioSource audioSource, AudioOutDelayControl.PlayDelayConfig playDelayConfig, Photon.Voice.ILogger logger, string logPrefix, bool debugInfo) : base(audioSource, playDelayConfig, logger, logPrefix, debugInfo)
	{
		this.voiceToLoudness = speaker;
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x0009D330 File Offset: 0x0009B530
	public override void OutWrite(float[] data, int offsetSamples)
	{
		float num = 0f;
		for (int i = 0; i < data.Length; i++)
		{
			float num2 = data[i];
			if (!float.IsFinite(num2))
			{
				num2 = 0f;
				data[i] = num2;
			}
			else if (num2 > 1f)
			{
				num2 = 1f;
				data[i] = num2;
			}
			else if (num2 < -1f)
			{
				num2 = -1f;
				data[i] = num2;
			}
			num += Mathf.Abs(num2);
		}
		if (num > 0f)
		{
			this.voiceToLoudness.loudness = num / (float)data.Length;
		}
		base.OutWrite(data, offsetSamples);
	}

	// Token: 0x04000E79 RID: 3705
	private SpeakerVoiceToLoudness voiceToLoudness;
}
