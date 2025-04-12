using System;
using UnityEngine;

// Token: 0x02000627 RID: 1575
public class PitchShiftAudioPlayer : MonoBehaviour
{
	// Token: 0x0600273E RID: 10046 RVA: 0x00049C86 File Offset: 0x00047E86
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

	// Token: 0x0600273F RID: 10047 RVA: 0x00049CBC File Offset: 0x00047EBC
	private void OnEnable()
	{
		this._pitchMixVars.Rent(out this._pitchMix);
		this._source.outputAudioMixerGroup = this._pitchMix.group;
	}

	// Token: 0x06002740 RID: 10048 RVA: 0x00049CE6 File Offset: 0x00047EE6
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

	// Token: 0x06002741 RID: 10049 RVA: 0x00049D0F File Offset: 0x00047F0F
	private void Update()
	{
		if (this.apply)
		{
			this.ApplyPitch();
		}
	}

	// Token: 0x06002742 RID: 10050 RVA: 0x00049D1F File Offset: 0x00047F1F
	private void ApplyPitch()
	{
		this._pitchMix.value = this._pitch.curved;
	}

	// Token: 0x04002AFD RID: 11005
	public bool apply = true;

	// Token: 0x04002AFE RID: 11006
	[SerializeField]
	private AudioSource _source;

	// Token: 0x04002AFF RID: 11007
	[SerializeField]
	private AudioMixVarPool _pitchMixVars;

	// Token: 0x04002B00 RID: 11008
	[SerializeReference]
	private AudioMixVar _pitchMix;

	// Token: 0x04002B01 RID: 11009
	[SerializeField]
	private RangedFloat _pitch;
}
