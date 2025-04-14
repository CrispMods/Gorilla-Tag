﻿using System;
using Photon.Voice;
using UnityEngine;

namespace GorillaTag.Audio
{
	// Token: 0x02000C06 RID: 3078
	internal class ProcessVoiceDataToLoudness : IProcessor<float>, IDisposable
	{
		// Token: 0x06004D00 RID: 19712 RVA: 0x001769F1 File Offset: 0x00174BF1
		public ProcessVoiceDataToLoudness(VoiceToLoudness voiceToLoudness)
		{
			this._voiceToLoudness = voiceToLoudness;
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x00176A00 File Offset: 0x00174C00
		public float[] Process(float[] buf)
		{
			float num = 0f;
			for (int i = 0; i < buf.Length; i++)
			{
				num += Mathf.Abs(buf[i]);
			}
			this._voiceToLoudness.loudness = num / (float)buf.Length;
			return buf;
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x000023F4 File Offset: 0x000005F4
		public void Dispose()
		{
		}

		// Token: 0x04004F53 RID: 20307
		private VoiceToLoudness _voiceToLoudness;
	}
}
