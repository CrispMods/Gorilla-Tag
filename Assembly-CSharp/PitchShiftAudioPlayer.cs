using System;
using UnityEngine;

// Token: 0x02000626 RID: 1574
public class PitchShiftAudioPlayer : MonoBehaviour
{
	// Token: 0x06002736 RID: 10038 RVA: 0x000C0A9E File Offset: 0x000BEC9E
	private void Awake()
	{
		if (this._source == null)
		{
			this._source = base.GetComponent<AudioSource>();
		}
		if (this._pitch == null)
		{
			this._pitch = base.GetComponent<RangedFloat>();
		}
	}

	// Token: 0x06002737 RID: 10039 RVA: 0x000C0AD4 File Offset: 0x000BECD4
	private void OnEnable()
	{
		this._pitchMixVars.Rent(out this._pitchMix);
		this._source.outputAudioMixerGroup = this._pitchMix.group;
	}

	// Token: 0x06002738 RID: 10040 RVA: 0x000C0AFE File Offset: 0x000BECFE
	private void OnDisable()
	{
		this._source.Stop();
		this._source.outputAudioMixerGroup = null;
		AudioMixVar pitchMix = this._pitchMix;
		if (pitchMix == null)
		{
			return;
		}
		pitchMix.ReturnToPool();
	}

	// Token: 0x06002739 RID: 10041 RVA: 0x000C0B27 File Offset: 0x000BED27
	private void Update()
	{
		if (this.apply)
		{
			this.ApplyPitch();
		}
	}

	// Token: 0x0600273A RID: 10042 RVA: 0x000C0B37 File Offset: 0x000BED37
	private void ApplyPitch()
	{
		this._pitchMix.value = this._pitch.curved;
	}

	// Token: 0x04002AF7 RID: 10999
	public bool apply = true;

	// Token: 0x04002AF8 RID: 11000
	[SerializeField]
	private AudioSource _source;

	// Token: 0x04002AF9 RID: 11001
	[SerializeField]
	private AudioMixVarPool _pitchMixVars;

	// Token: 0x04002AFA RID: 11002
	[SerializeReference]
	private AudioMixVar _pitchMix;

	// Token: 0x04002AFB RID: 11003
	[SerializeField]
	private RangedFloat _pitch;
}
