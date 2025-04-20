using System;
using Photon.Voice;
using UnityEngine;

namespace GorillaTag.Audio
{
	// Token: 0x02000C31 RID: 3121
	internal class ProcessVoiceDataToLoudness : IProcessor<float>, IDisposable
	{
		// Token: 0x06004E40 RID: 20032 RVA: 0x00063366 File Offset: 0x00061566
		public ProcessVoiceDataToLoudness(VoiceToLoudness voiceToLoudness)
		{
			this._voiceToLoudness = voiceToLoudness;
		}

		// Token: 0x06004E41 RID: 20033 RVA: 0x001AE7A0 File Offset: 0x001AC9A0
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

		// Token: 0x06004E42 RID: 20034 RVA: 0x00030607 File Offset: 0x0002E807
		public void Dispose()
		{
		}

		// Token: 0x04005037 RID: 20535
		private VoiceToLoudness _voiceToLoudness;
	}
}
