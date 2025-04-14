using System;
using Photon.Voice;
using UnityEngine;

namespace GorillaTag.Audio
{
	// Token: 0x02000C03 RID: 3075
	internal class ProcessVoiceDataToLoudness : IProcessor<float>, IDisposable
	{
		// Token: 0x06004CF4 RID: 19700 RVA: 0x00176429 File Offset: 0x00174629
		public ProcessVoiceDataToLoudness(VoiceToLoudness voiceToLoudness)
		{
			this._voiceToLoudness = voiceToLoudness;
		}

		// Token: 0x06004CF5 RID: 19701 RVA: 0x00176438 File Offset: 0x00174638
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

		// Token: 0x06004CF6 RID: 19702 RVA: 0x000023F4 File Offset: 0x000005F4
		public void Dispose()
		{
		}

		// Token: 0x04004F41 RID: 20289
		private VoiceToLoudness _voiceToLoudness;
	}
}
