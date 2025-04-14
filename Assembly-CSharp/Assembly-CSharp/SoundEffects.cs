using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200044D RID: 1101
public class SoundEffects : MonoBehaviour
{
	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06001B23 RID: 6947 RVA: 0x00086032 File Offset: 0x00084232
	public bool isPlaying
	{
		get
		{
			return this._lastClipIndex >= 0 && this._lastClipLength >= 0.0 && this._lastClipElapsedTime < this._lastClipLength;
		}
	}

	// Token: 0x06001B24 RID: 6948 RVA: 0x00086065 File Offset: 0x00084265
	public void Clear()
	{
		this.audioClips.Clear();
		this._lastClipIndex = -1;
		this._lastClipLength = -1.0;
	}

	// Token: 0x06001B25 RID: 6949 RVA: 0x00086088 File Offset: 0x00084288
	public void Stop()
	{
		if (this.source)
		{
			this.source.GTStop();
		}
		this._lastClipLength = -1.0;
	}

	// Token: 0x06001B26 RID: 6950 RVA: 0x000860B4 File Offset: 0x000842B4
	public void PlayNext(float delayMin, float delayMax, float volMin, float volMax)
	{
		float delay = this._rnd.NextFloat(delayMin, delayMax);
		float volume = this._rnd.NextFloat(volMin, volMax);
		this.PlayNext(delay, volume);
	}

	// Token: 0x06001B27 RID: 6951 RVA: 0x000860E8 File Offset: 0x000842E8
	public void PlayNext(float delay = 0f, float volume = 1f)
	{
		if (!this.source)
		{
			return;
		}
		if (this.audioClips == null || this.audioClips.Count == 0)
		{
			return;
		}
		if (this.source.isPlaying)
		{
			this.source.GTStop();
		}
		int num = this._rnd.NextInt(this.audioClips.Count);
		while (this.distinct && this._lastClipIndex == num)
		{
			num = this._rnd.NextInt(this.audioClips.Count);
		}
		AudioClip audioClip = this.audioClips[num];
		this._lastClipIndex = num;
		this._lastClipLength = (double)audioClip.length;
		float num2 = delay;
		if (num2 < this._minDelay)
		{
			num2 = this._minDelay;
		}
		if (num2 < 0.0001f)
		{
			this.source.GTPlayOneShot(audioClip, volume);
			this._lastClipElapsedTime = 0f;
			return;
		}
		this.source.clip = audioClip;
		this.source.volume = volume;
		this.source.GTPlayDelayed(num2);
		this._lastClipElapsedTime = -num2;
	}

	// Token: 0x06001B28 RID: 6952 RVA: 0x000861FC File Offset: 0x000843FC
	[Conditional("UNITY_EDITOR")]
	private void OnValidate()
	{
		if (string.IsNullOrEmpty(this.seed))
		{
			this.seed = "0x1337C0D3";
		}
		this._rnd = new SRand(this.seed);
		if (this.audioClips == null)
		{
			this.audioClips = new List<AudioClip>();
		}
	}

	// Token: 0x04001E13 RID: 7699
	public AudioSource source;

	// Token: 0x04001E14 RID: 7700
	[Space]
	public List<AudioClip> audioClips = new List<AudioClip>();

	// Token: 0x04001E15 RID: 7701
	public string seed = "0x1337C0D3";

	// Token: 0x04001E16 RID: 7702
	[Space]
	public bool distinct = true;

	// Token: 0x04001E17 RID: 7703
	[SerializeField]
	private float _minDelay;

	// Token: 0x04001E18 RID: 7704
	[Space]
	[SerializeField]
	private SRand _rnd;

	// Token: 0x04001E19 RID: 7705
	[NonSerialized]
	private int _lastClipIndex = -1;

	// Token: 0x04001E1A RID: 7706
	[NonSerialized]
	private double _lastClipLength = -1.0;

	// Token: 0x04001E1B RID: 7707
	[NonSerialized]
	private TimeSince _lastClipElapsedTime;
}
