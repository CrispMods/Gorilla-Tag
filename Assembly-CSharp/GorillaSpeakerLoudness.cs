using System;
using GorillaNetworking;
using GorillaTag;
using GorillaTag.Audio;
using Oculus.VoiceSDK.Utilities;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

// Token: 0x02000591 RID: 1425
public class GorillaSpeakerLoudness : MonoBehaviour, IGorillaSliceableSimple, IDynamicFloat
{
	// Token: 0x17000397 RID: 919
	// (get) Token: 0x06002341 RID: 9025 RVA: 0x000AEC3F File Offset: 0x000ACE3F
	public bool IsSpeaking
	{
		get
		{
			return this.isSpeaking;
		}
	}

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x06002342 RID: 9026 RVA: 0x000AEC47 File Offset: 0x000ACE47
	public float Loudness
	{
		get
		{
			return this.loudness;
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x06002343 RID: 9027 RVA: 0x000AEC4F File Offset: 0x000ACE4F
	public float LoudnessNormalized
	{
		get
		{
			return Mathf.Min(this.loudness / this.normalizedMax, 1f);
		}
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x06002344 RID: 9028 RVA: 0x000AEC68 File Offset: 0x000ACE68
	public float floatValue
	{
		get
		{
			return this.LoudnessNormalized;
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x06002345 RID: 9029 RVA: 0x000AEC70 File Offset: 0x000ACE70
	public bool IsMicEnabled
	{
		get
		{
			return this.isMicEnabled;
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x06002346 RID: 9030 RVA: 0x000AEC78 File Offset: 0x000ACE78
	public float SmoothedLoudness
	{
		get
		{
			return this.smoothedLoudness;
		}
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x000AEC80 File Offset: 0x000ACE80
	private void Start()
	{
		this.rigContainer = base.GetComponent<RigContainer>();
		this.timeLastUpdated = Time.time;
		this.deltaTime = Time.deltaTime;
	}

	// Token: 0x06002348 RID: 9032 RVA: 0x0000F862 File Offset: 0x0000DA62
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06002349 RID: 9033 RVA: 0x0000F86B File Offset: 0x0000DA6B
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x0600234A RID: 9034 RVA: 0x000AECA4 File Offset: 0x000ACEA4
	public void SliceUpdate()
	{
		this.deltaTime = Time.time - this.timeLastUpdated;
		this.timeLastUpdated = Time.time;
		this.UpdateMicEnabled();
		this.UpdateLoudness();
		this.UpdateSmoothedLoudness();
	}

	// Token: 0x0600234B RID: 9035 RVA: 0x000AECD8 File Offset: 0x000ACED8
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

	// Token: 0x0600234C RID: 9036 RVA: 0x000AED74 File Offset: 0x000ACF74
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

	// Token: 0x0600234D RID: 9037 RVA: 0x000AEF5C File Offset: 0x000AD15C
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

	// Token: 0x0600234F RID: 9039 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x040026DD RID: 9949
	private bool isSpeaking;

	// Token: 0x040026DE RID: 9950
	private float loudness;

	// Token: 0x040026DF RID: 9951
	[SerializeField]
	private float normalizedMax = 0.175f;

	// Token: 0x040026E0 RID: 9952
	private bool isMicEnabled;

	// Token: 0x040026E1 RID: 9953
	private RigContainer rigContainer;

	// Token: 0x040026E2 RID: 9954
	private Speaker speaker;

	// Token: 0x040026E3 RID: 9955
	private SpeakerVoiceToLoudness speakerVoiceToLoudness;

	// Token: 0x040026E4 RID: 9956
	private Recorder recorder;

	// Token: 0x040026E5 RID: 9957
	private VoiceToLoudness voiceToLoudness;

	// Token: 0x040026E6 RID: 9958
	private float smoothedLoudness;

	// Token: 0x040026E7 RID: 9959
	private float lastLoudness;

	// Token: 0x040026E8 RID: 9960
	private float timeSinceLoudnessChange;

	// Token: 0x040026E9 RID: 9961
	private float loudnessUpdateCheckRate = 0.2f;

	// Token: 0x040026EA RID: 9962
	private float loudnessBlendStrength = 2f;

	// Token: 0x040026EB RID: 9963
	private bool permission;

	// Token: 0x040026EC RID: 9964
	private bool micConnected;

	// Token: 0x040026ED RID: 9965
	private float timeLastUpdated;

	// Token: 0x040026EE RID: 9966
	private float deltaTime;
}
