using System;
using Photon.Voice;
using UnityEngine;

namespace GorillaTag.Audio
{
	// Token: 0x02000C06 RID: 3078
	internal class ProcessVoiceDataToLoudness : IProcessor<float>, IDisposable
	{
		// Token: 0x06004D00 RID: 19712 RVA: 0x000619A5 File Offset: 0x0005FBA5
		public ProcessVoiceDataToLoudness(VoiceToLoudness voiceToLoudness)
		{
			this._voiceToLoudness = voiceToLoudness;
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x001A77D4 File Offset: 0x001A59D4
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

		// Token: 0x06004D02 RID: 19714 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void Dispose()
		{
		}

		// Token: 0x04004F53 RID: 20307
		private VoiceToLoudness _voiceToLoudness;
	}
}
