using System;
using UnityEngine;

// Token: 0x0200060D RID: 1549
public class ReplacementVoice : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x0600268D RID: 9869 RVA: 0x000320BF File Offset: 0x000302BF
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x0600268E RID: 9870 RVA: 0x000320C8 File Offset: 0x000302C8
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x0600268F RID: 9871 RVA: 0x00108C1C File Offset: 0x00106E1C
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

	// Token: 0x06002691 RID: 9873 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04002A82 RID: 10882
	public AudioSource replacementVoiceSource;

	// Token: 0x04002A83 RID: 10883
	public AudioClip[] replacementVoiceClips;

	// Token: 0x04002A84 RID: 10884
	public AudioClip[] replacementVoiceClipsLoud;

	// Token: 0x04002A85 RID: 10885
	public float loudReplacementVoiceThreshold = 0.1f;

	// Token: 0x04002A86 RID: 10886
	public VRRig myVRRig;

	// Token: 0x04002A87 RID: 10887
	public float normalVolume = 0.5f;

	// Token: 0x04002A88 RID: 10888
	public float loudVolume = 0.8f;
}
