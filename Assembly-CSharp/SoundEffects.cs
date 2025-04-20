using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000459 RID: 1113
public class SoundEffects : MonoBehaviour
{
	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x06001B74 RID: 7028 RVA: 0x00042B57 File Offset: 0x00040D57
	public bool isPlaying
	{
		get
		{
			return this._lastClipIndex >= 0 && this._lastClipLength >= 0.0 && this._lastClipElapsedTime < this._lastClipLength;
		}
	}

	// Token: 0x06001B75 RID: 7029 RVA: 0x00042B8A File Offset: 0x00040D8A
	public void Clear()
	{
		this.audioClips.Clear();
		this._lastClipIndex = -1;
		this._lastClipLength = -1.0;
	}

	// Token: 0x06001B76 RID: 7030 RVA: 0x00042BAD File Offset: 0x00040DAD
	public void Stop()
	{
		if (this.source)
		{
			this.source.GTStop();
		}
		this._lastClipLength = -1.0;
	}

	// Token: 0x06001B77 RID: 7031 RVA: 0x000DA624 File Offset: 0x000D8824
	public void PlayNext(float delayMin, float delayMax, float volMin, float volMax)
	{
		float delay = this._rnd.NextFloat(delayMin, delayMax);
		float volume = this._rnd.NextFloat(volMin, volMax);
		this.PlayNext(delay, volume);
	}

	// Token: 0x06001B78 RID: 7032 RVA: 0x000DA658 File Offset: 0x000D8858
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

	// Token: 0x06001B79 RID: 7033 RVA: 0x00042BD6 File Offset: 0x00040DD6
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

	// Token: 0x04001E61 RID: 7777
	public AudioSource source;

	// Token: 0x04001E62 RID: 7778
	[Space]
	public List<AudioClip> audioClips = new List<AudioClip>();

	// Token: 0x04001E63 RID: 7779
	public string seed = "0x1337C0D3";

	// Token: 0x04001E64 RID: 7780
	[Space]
	public bool distinct = true;

	// Token: 0x04001E65 RID: 7781
	[SerializeField]
	private float _minDelay;

	// Token: 0x04001E66 RID: 7782
	[Space]
	[SerializeField]
	private SRand _rnd;

	// Token: 0x04001E67 RID: 7783
	[NonSerialized]
	private int _lastClipIndex = -1;

	// Token: 0x04001E68 RID: 7784
	[NonSerialized]
	private double _lastClipLength = -1.0;

	// Token: 0x04001E69 RID: 7785
	[NonSerialized]
	private TimeSince _lastClipElapsedTime;
}
