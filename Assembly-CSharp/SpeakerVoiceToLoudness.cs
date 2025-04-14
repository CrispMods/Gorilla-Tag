using System;
using Photon.Voice;
using Photon.Voice.Unity;
using UnityEngine;

// Token: 0x020001F6 RID: 502
[RequireComponent(typeof(Speaker))]
public class SpeakerVoiceToLoudness : MonoBehaviour
{
	// Token: 0x06000BCB RID: 3019 RVA: 0x0003E81C File Offset: 0x0003CA1C
	private void Awake()
	{
		Speaker component = base.GetComponent<Speaker>();
		component.CustomAudioOutFactory = this.GetVolumeTracking(component);
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x0003E840 File Offset: 0x0003CA40
	private Func<IAudioOut<float>> GetVolumeTracking(Speaker speaker)
	{
		AudioOutDelayControl.PlayDelayConfig pdc = new AudioOutDelayControl.PlayDelayConfig
		{
			Low = this.playbackDelaySettings.MinDelaySoft,
			High = this.playbackDelaySettings.MaxDelaySoft,
			Max = this.playbackDelaySettings.MaxDelayHard
		};
		return () => new SpeakerVoiceLoudnessAudioOut(this, speaker.GetComponent<AudioSource>(), pdc, speaker.Logger, string.Empty, speaker.Logger.IsDebugEnabled);
	}

	// Token: 0x04000E2E RID: 3630
	[SerializeField]
	private PlaybackDelaySettings playbackDelaySettings = new PlaybackDelaySettings
	{
		MinDelaySoft = 200,
		MaxDelaySoft = 400,
		MaxDelayHard = 1000
	};

	// Token: 0x04000E2F RID: 3631
	public float loudness;
}
