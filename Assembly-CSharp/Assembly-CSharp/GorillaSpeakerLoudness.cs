using System;
using GorillaNetworking;
using GorillaTag;
using GorillaTag.Audio;
using Oculus.VoiceSDK.Utilities;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

// Token: 0x02000592 RID: 1426
public class GorillaSpeakerLoudness : MonoBehaviour, IGorillaSliceableSimple, IDynamicFloat
{
	// Token: 0x17000398 RID: 920
	// (get) Token: 0x06002349 RID: 9033 RVA: 0x000AF0BF File Offset: 0x000AD2BF
	public bool IsSpeaking
	{
		get
		{
			return this.isSpeaking;
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x0600234A RID: 9034 RVA: 0x000AF0C7 File Offset: 0x000AD2C7
	public float Loudness
	{
		get
		{
			return this.loudness;
		}
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x0600234B RID: 9035 RVA: 0x000AF0CF File Offset: 0x000AD2CF
	public float LoudnessNormalized
	{
		get
		{
			return Mathf.Min(this.loudness / this.normalizedMax, 1f);
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x0600234C RID: 9036 RVA: 0x000AF0E8 File Offset: 0x000AD2E8
	public float floatValue
	{
		get
		{
			return this.LoudnessNormalized;
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x0600234D RID: 9037 RVA: 0x000AF0F0 File Offset: 0x000AD2F0
	public bool IsMicEnabled
	{
		get
		{
			return this.isMicEnabled;
		}
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x0600234E RID: 9038 RVA: 0x000AF0F8 File Offset: 0x000AD2F8
	public float SmoothedLoudness
	{
		get
		{
			return this.smoothedLoudness;
		}
	}

	// Token: 0x0600234F RID: 9039 RVA: 0x000AF100 File Offset: 0x000AD300
	private void Start()
	{
		this.rigContainer = base.GetComponent<RigContainer>();
		this.timeLastUpdated = Time.time;
		this.deltaTime = Time.deltaTime;
	}

	// Token: 0x06002350 RID: 9040 RVA: 0x0000FC06 File Offset: 0x0000DE06
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06002351 RID: 9041 RVA: 0x0000FC0F File Offset: 0x0000DE0F
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06002352 RID: 9042 RVA: 0x000AF124 File Offset: 0x000AD324
	public void SliceUpdate()
	{
		this.deltaTime = Time.time - this.timeLastUpdated;
		this.timeLastUpdated = Time.time;
		this.UpdateMicEnabled();
		this.UpdateLoudness();
		this.UpdateSmoothedLoudness();
	}

	// Token: 0x06002353 RID: 9043 RVA: 0x000AF158 File Offset: 0x000AD358
	private void UpdateMicEnabled()
	{
		if (this.rigContainer == null)
		{
			return;
		}
		VRRig rig = this.rigContainer.Rig;
		if (rig.isOfflineVRRig)
		{
			this.permission = (this.permission || MicPermissionsManager.HasMicPermission());
			if (this.permission && !this.micConnected && Microphone.devices != null)
			{
				this.micConnected = (Microphone.devices.Length != 0);
			}
			this.isMicEnabled = (this.permission && this.micConnected);
			rig.IsMicEnabled = this.isMicEnabled;
			return;
		}
		this.isMicEnabled = rig.IsMicEnabled;
	}

	// Token: 0x06002354 RID: 9044 RVA: 0x000AF1F4 File Offset: 0x000AD3F4
	private void UpdateLoudness()
	{
		if (this.rigContainer == null)
		{
			return;
		}
		PhotonVoiceView voice = this.rigContainer.Voice;
		if (voice != null && this.speaker == null)
		{
			this.speaker = voice.SpeakerInUse;
		}
		if (this.recorder == null)
		{
			this.recorder = ((voice != null) ? voice.RecorderInUse : null);
		}
		VRRig rig = this.rigContainer.Rig;
		if ((rig.remoteUseReplacementVoice || rig.localUseReplacementVoice || GorillaComputer.instance.voiceChatOn == "FALSE") && rig.SpeakingLoudness > 0f && !this.rigContainer.ForceMute && !this.rigContainer.Muted)
		{
			this.isSpeaking = true;
			this.loudness = rig.SpeakingLoudness;
			return;
		}
		if (voice != null && voice.IsSpeaking)
		{
			this.isSpeaking = true;
			if (!(this.speaker != null))
			{
				this.loudness = 0f;
				return;
			}
			if (this.speakerVoiceToLoudness == null)
			{
				this.speakerVoiceToLoudness = this.speaker.GetComponent<SpeakerVoiceToLoudness>();
			}
			if (this.speakerVoiceToLoudness != null)
			{
				this.loudness = this.speakerVoiceToLoudness.loudness;
				return;
			}
		}
		else if (voice != null && this.recorder != null && NetworkSystem.Instance.IsObjectLocallyOwned(voice.gameObject) && this.recorder.IsCurrentlyTransmitting)
		{
			if (this.voiceToLoudness == null)
			{
				this.voiceToLoudness = this.recorder.GetComponent<VoiceToLoudness>();
			}
			this.isSpeaking = true;
			if (this.voiceToLoudness != null)
			{
				this.loudness = this.voiceToLoudness.loudness;
				return;
			}
			this.loudness = 0f;
			return;
		}
		else
		{
			this.isSpeaking = false;
			this.loudness = 0f;
		}
	}

	// Token: 0x06002355 RID: 9045 RVA: 0x000AF3DC File Offset: 0x000AD5DC
	private void UpdateSmoothedLoudness()
	{
		if (!this.isSpeaking)
		{
			this.smoothedLoudness = 0f;
			return;
		}
		if (!Mathf.Approximately(this.loudness, this.lastLoudness))
		{
			this.timeSinceLoudnessChange = 0f;
			this.smoothedLoudness = Mathf.Lerp(this.smoothedLoudness, this.loudness, Mathf.Clamp01(this.loudnessBlendStrength * this.deltaTime));
			this.lastLoudness = this.loudness;
			return;
		}
		if (this.timeSinceLoudnessChange > this.loudnessUpdateCheckRate)
		{
			this.smoothedLoudness = 0.001f;
			return;
		}
		this.smoothedLoudness = Mathf.Lerp(this.smoothedLoudness, this.loudness, Mathf.Clamp01(this.loudnessBlendStrength * this.deltaTime));
		this.timeSinceLoudnessChange += this.deltaTime;
	}

	// Token: 0x06002357 RID: 9047 RVA: 0x0000FD18 File Offset: 0x0000DF18
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x040026E3 RID: 9955
	private bool isSpeaking;

	// Token: 0x040026E4 RID: 9956
	private float loudness;

	// Token: 0x040026E5 RID: 9957
	[SerializeField]
	private float normalizedMax = 0.175f;

	// Token: 0x040026E6 RID: 9958
	private bool isMicEnabled;

	// Token: 0x040026E7 RID: 9959
	private RigContainer rigContainer;

	// Token: 0x040026E8 RID: 9960
	private Speaker speaker;

	// Token: 0x040026E9 RID: 9961
	private SpeakerVoiceToLoudness speakerVoiceToLoudness;

	// Token: 0x040026EA RID: 9962
	private Recorder recorder;

	// Token: 0x040026EB RID: 9963
	private VoiceToLoudness voiceToLoudness;

	// Token: 0x040026EC RID: 9964
	private float smoothedLoudness;

	// Token: 0x040026ED RID: 9965
	private float lastLoudness;

	// Token: 0x040026EE RID: 9966
	private float timeSinceLoudnessChange;

	// Token: 0x040026EF RID: 9967
	private float loudnessUpdateCheckRate = 0.2f;

	// Token: 0x040026F0 RID: 9968
	private float loudnessBlendStrength = 2f;

	// Token: 0x040026F1 RID: 9969
	private bool permission;

	// Token: 0x040026F2 RID: 9970
	private bool micConnected;

	// Token: 0x040026F3 RID: 9971
	private float timeLastUpdated;

	// Token: 0x040026F4 RID: 9972
	private float deltaTime;
}
