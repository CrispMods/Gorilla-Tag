﻿using System;
using Photon.Voice;
using Photon.Voice.Unity;
using UnityEngine;

// Token: 0x020001F8 RID: 504
public class SpeakerVoiceLoudnessAudioOut : UnityAudioOut
{
	// Token: 0x06000BD0 RID: 3024 RVA: 0x0003E930 File Offset: 0x0003CB30
	public SpeakerVoiceLoudnessAudioOut(SpeakerVoiceToLoudness speaker, AudioSource audioSource, AudioOutDelayControl.PlayDelayConfig playDelayConfig, Photon.Voice.ILogger logger, string logPrefix, bool debugInfo) : base(audioSource, playDelayConfig, logger, logPrefix, debugInfo)
	{
		this.voiceToLoudness = speaker;
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x0003E948 File Offset: 0x0003CB48
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

	// Token: 0x04000E33 RID: 3635
	private SpeakerVoiceToLoudness voiceToLoudness;
}
