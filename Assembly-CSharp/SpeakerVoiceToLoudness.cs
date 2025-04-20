using System;
using Photon.Voice;
using Photon.Voice.Unity;
using UnityEngine;

// Token: 0x02000201 RID: 513
[RequireComponent(typeof(Speaker))]
public class SpeakerVoiceToLoudness : MonoBehaviour
{
	// Token: 0x06000C16 RID: 3094 RVA: 0x0009D258 File Offset: 0x0009B458
	private void Awake()
	{
		Speaker component = base.GetComponent<Speaker>();
		component.CustomAudioOutFactory = this.GetVolumeTracking(component);
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x0009D27C File Offset: 0x0009B47C
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

	// Token: 0x04000E74 RID: 3700
	[SerializeField]
	private PlaybackDelaySettings playbackDelaySettings = new PlaybackDelaySettings
	{
		MinDelaySoft = 200,
		MaxDelaySoft = 400,
		MaxDelayHard = 1000
	};

	// Token: 0x04000E75 RID: 3701
	public float loudness;
}
