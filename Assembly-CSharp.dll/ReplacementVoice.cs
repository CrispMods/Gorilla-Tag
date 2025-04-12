using System;
using UnityEngine;

// Token: 0x0200062F RID: 1583
public class ReplacementVoice : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x0600276A RID: 10090 RVA: 0x00030F55 File Offset: 0x0002F155
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x0600276B RID: 10091 RVA: 0x00030F5E File Offset: 0x0002F15E
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x0600276C RID: 10092 RVA: 0x0010A830 File Offset: 0x00108A30
	public void SliceUpdate()
	{
		if (!this.replacementVoiceSource.isPlaying && this.myVRRig.ShouldPlayReplacementVoice())
		{
			if (!Mathf.Approximately(this.myVRRig.voiceAudio.pitch, this.replacementVoiceSource.pitch))
			{
				this.replacementVoiceSource.pitch = this.myVRRig.voiceAudio.pitch;
			}
			if (this.myVRRig.SpeakingLoudness < this.loudReplacementVoiceThreshold)
			{
				this.replacementVoiceSource.clip = this.replacementVoiceClips[UnityEngine.Random.Range(0, this.replacementVoiceClips.Length - 1)];
				this.replacementVoiceSource.volume = this.normalVolume;
			}
			else
			{
				this.replacementVoiceSource.clip = this.replacementVoiceClipsLoud[UnityEngine.Random.Range(0, this.replacementVoiceClipsLoud.Length - 1)];
				this.replacementVoiceSource.volume = this.loudVolume;
			}
			this.replacementVoiceSource.GTPlay();
		}
	}

	// Token: 0x0600276E RID: 10094 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04002B22 RID: 11042
	public AudioSource replacementVoiceSource;

	// Token: 0x04002B23 RID: 11043
	public AudioClip[] replacementVoiceClips;

	// Token: 0x04002B24 RID: 11044
	public AudioClip[] replacementVoiceClipsLoud;

	// Token: 0x04002B25 RID: 11045
	public float loudReplacementVoiceThreshold = 0.1f;

	// Token: 0x04002B26 RID: 11046
	public VRRig myVRRig;

	// Token: 0x04002B27 RID: 11047
	public float normalVolume = 0.5f;

	// Token: 0x04002B28 RID: 11048
	public float loudVolume = 0.8f;
}
