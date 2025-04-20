using System;
using GorillaNetworking;
using GorillaTag;
using GorillaTag.Audio;
using Oculus.VoiceSDK.Utilities;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

// Token: 0x0200059F RID: 1439
public class GorillaSpeakerLoudness : MonoBehaviour, IGorillaSliceableSimple, IDynamicFloat
{
	// Token: 0x1700039F RID: 927
	// (get) Token: 0x060023A1 RID: 9121 RVA: 0x00048126 File Offset: 0x00046326
	public bool IsSpeaking
	{
		get
		{
			return this.isSpeaking;
		}
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x060023A2 RID: 9122 RVA: 0x0004812E File Offset: 0x0004632E
	public float Loudness
	{
		get
		{
			return this.loudness;
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x060023A3 RID: 9123 RVA: 0x00048136 File Offset: 0x00046336
	public float LoudnessNormalized
	{
		get
		{
			return Mathf.Min(this.loudness / this.normalizedMax, 1f);
		}
	}

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x060023A4 RID: 9124 RVA: 0x0004814F File Offset: 0x0004634F
	public float floatValue
	{
		get
		{
			return this.LoudnessNormalized;
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x060023A5 RID: 9125 RVA: 0x00048157 File Offset: 0x00046357
	public bool IsMicEnabled
	{
		get
		{
			return this.isMicEnabled;
		}
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x060023A6 RID: 9126 RVA: 0x0004815F File Offset: 0x0004635F
	public float SmoothedLoudness
	{
		get
		{
			return this.smoothedLoudness;
		}
	}

	// Token: 0x060023A7 RID: 9127 RVA: 0x00048167 File Offset: 0x00046367
	private void Start()
	{
		this.rigContainer = base.GetComponent<RigContainer>();
		this.timeLastUpdated = Time.time;
		this.deltaTime = Time.deltaTime;
	}

	// Token: 0x060023A8 RID: 9128 RVA: 0x000320BF File Offset: 0x000302BF
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060023A9 RID: 9129 RVA: 0x000320C8 File Offset: 0x000302C8
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060023AA RID: 9130 RVA: 0x0004818B File Offset: 0x0004638B
	public void SliceUpdate()
	{
		this.deltaTime = Time.time - this.timeLastUpdated;
		this.timeLastUpdated = Time.time;
		this.UpdateMicEnabled();
		this.UpdateLoudness();
		this.UpdateSmoothedLoudness();
	}

	// Token: 0x060023AB RID: 9131 RVA: 0x000FE23C File Offset: 0x000FC43C
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

	// Token: 0x060023AC RID: 9132 RVA: 0x000FE2D8 File Offset: 0x000FC4D8
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

	// Token: 0x060023AD RID: 9133 RVA: 0x000FE4C0 File Offset: 0x000FC6C0
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

	// Token: 0x060023AF RID: 9135 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04002738 RID: 10040
	private bool isSpeaking;

	// Token: 0x04002739 RID: 10041
	private float loudness;

	// Token: 0x0400273A RID: 10042
	[SerializeField]
	private float normalizedMax = 0.175f;

	// Token: 0x0400273B RID: 10043
	private bool isMicEnabled;

	// Token: 0x0400273C RID: 10044
	private RigContainer rigContainer;

	// Token: 0x0400273D RID: 10045
	private Speaker speaker;

	// Token: 0x0400273E RID: 10046
	private SpeakerVoiceToLoudness speakerVoiceToLoudness;

	// Token: 0x0400273F RID: 10047
	private Recorder recorder;

	// Token: 0x04002740 RID: 10048
	private VoiceToLoudness voiceToLoudness;

	// Token: 0x04002741 RID: 10049
	private float smoothedLoudness;

	// Token: 0x04002742 RID: 10050
	private float lastLoudness;

	// Token: 0x04002743 RID: 10051
	private float timeSinceLoudnessChange;

	// Token: 0x04002744 RID: 10052
	private float loudnessUpdateCheckRate = 0.2f;

	// Token: 0x04002745 RID: 10053
	private float loudnessBlendStrength = 2f;

	// Token: 0x04002746 RID: 10054
	private bool permission;

	// Token: 0x04002747 RID: 10055
	private bool micConnected;

	// Token: 0x04002748 RID: 10056
	private float timeLastUpdated;

	// Token: 0x04002749 RID: 10057
	private float deltaTime;
}
