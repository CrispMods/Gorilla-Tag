using System;
using UnityEngine;

// Token: 0x0200062E RID: 1582
public class ReplacementVoice : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06002762 RID: 10082 RVA: 0x0000F862 File Offset: 0x0000DA62
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06002763 RID: 10083 RVA: 0x0000F86B File Offset: 0x0000DA6B
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06002764 RID: 10084 RVA: 0x000C1284 File Offset: 0x000BF484
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
				this.replacementVoiceSource.clip = this.replacementVoiceClips[Random.Range(0, this.replacementVoiceClips.Length - 1)];
				this.replacementVoiceSource.volume = this.normalVolume;
			}
			else
			{
				this.replacementVoiceSource.clip = this.replacementVoiceClipsLoud[Random.Range(0, this.replacementVoiceClipsLoud.Length - 1)];
				this.replacementVoiceSource.volume = this.loudVolume;
			}
			this.replacementVoiceSource.GTPlay();
		}
	}

	// Token: 0x06002766 RID: 10086 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04002B1C RID: 11036
	public AudioSource replacementVoiceSource;

	// Token: 0x04002B1D RID: 11037
	public AudioClip[] replacementVoiceClips;

	// Token: 0x04002B1E RID: 11038
	public AudioClip[] replacementVoiceClipsLoud;

	// Token: 0x04002B1F RID: 11039
	public float loudReplacementVoiceThreshold = 0.1f;

	// Token: 0x04002B20 RID: 11040
	public VRRig myVRRig;

	// Token: 0x04002B21 RID: 11041
	public float normalVolume = 0.5f;

	// Token: 0x04002B22 RID: 11042
	public float loudVolume = 0.8f;
}
