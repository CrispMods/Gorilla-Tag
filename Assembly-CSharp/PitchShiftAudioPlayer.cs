using System;
using UnityEngine;

// Token: 0x02000605 RID: 1541
public class PitchShiftAudioPlayer : MonoBehaviour
{
	// Token: 0x06002661 RID: 9825 RVA: 0x0004A21B File Offset: 0x0004841B
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

	// Token: 0x06002662 RID: 9826 RVA: 0x0004A251 File Offset: 0x00048451
	private void OnEnable()
	{
		this._pitchMixVars.Rent(out this._pitchMix);
		this._source.outputAudioMixerGroup = this._pitchMix.group;
	}

	// Token: 0x06002663 RID: 9827 RVA: 0x0004A27B File Offset: 0x0004847B
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

	// Token: 0x06002664 RID: 9828 RVA: 0x0004A2A4 File Offset: 0x000484A4
	private void Update()
	{
		if (this.apply)
		{
			this.ApplyPitch();
		}
	}

	// Token: 0x06002665 RID: 9829 RVA: 0x0004A2B4 File Offset: 0x000484B4
	private void ApplyPitch()
	{
		this._pitchMix.value = this._pitch.curved;
	}

	// Token: 0x04002A5D RID: 10845
	public bool apply = true;

	// Token: 0x04002A5E RID: 10846
	[SerializeField]
	private AudioSource _source;

	// Token: 0x04002A5F RID: 10847
	[SerializeField]
	private AudioMixVarPool _pitchMixVars;

	// Token: 0x04002A60 RID: 10848
	[SerializeReference]
	private AudioMixVar _pitchMix;

	// Token: 0x04002A61 RID: 10849
	[SerializeField]
	private RangedFloat _pitch;
}
