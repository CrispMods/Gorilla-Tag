using System;
using Photon.Voice;
using Photon.Voice.Unity;
using UnityEngine;

namespace GorillaTag.Audio
{
	// Token: 0x02000C30 RID: 3120
	[RequireComponent(typeof(Recorder))]
	public class VoiceToLoudness : MonoBehaviour
	{
		// Token: 0x06004E3D RID: 20029 RVA: 0x00063358 File Offset: 0x00061558
		protected void Awake()
		{
			this._recorder = base.GetComponent<Recorder>();
		}

		// Token: 0x06004E3E RID: 20030 RVA: 0x001AE760 File Offset: 0x001AC960
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

		// Token: 0x04005035 RID: 20533
		[NonSerialized]
		public float loudness;

		// Token: 0x04005036 RID: 20534
		private Recorder _recorder;
	}
}
