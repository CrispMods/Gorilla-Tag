using System;
using Photon.Voice;
using Photon.Voice.Unity;
using UnityEngine;

namespace GorillaTag.Audio
{
	// Token: 0x02000C02 RID: 3074
	[RequireComponent(typeof(Recorder))]
	public class VoiceToLoudness : MonoBehaviour
	{
		// Token: 0x06004CF1 RID: 19697 RVA: 0x001763DD File Offset: 0x001745DD
		protected void Awake()
		{
			this._recorder = base.GetComponent<Recorder>();
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x001763EC File Offset: 0x001745EC
		protected void PhotonVoiceCreated(PhotonVoiceCreatedParams photonVoiceCreatedParams)
		{
			VoiceInfo info = photonVoiceCreatedParams.Voice.Info;
			LocalVoiceAudioFloat localVoiceAudioFloat = photonVoiceCreatedParams.Voice as LocalVoiceAudioFloat;
			if (localVoiceAudioFloat != null)
			{
				localVoiceAudioFloat.AddPostProcessor(new IProcessor<float>[]
				{
					new ProcessVoiceDataToLoudness(this)
				});
			}
		}

		// Token: 0x04004F3F RID: 20287
		[NonSerialized]
		public float loudness;

		// Token: 0x04004F40 RID: 20288
		private Recorder _recorder;
	}
}
