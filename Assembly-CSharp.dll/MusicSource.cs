using System;
using UnityEngine;

// Token: 0x02000625 RID: 1573
[RequireComponent(typeof(AudioSource))]
public class MusicSource : MonoBehaviour
{
	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x0600272D RID: 10029 RVA: 0x00049B08 File Offset: 0x00047D08
	public AudioSource AudioSource
	{
		get
		{
			return this.audioSource;
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x0600272E RID: 10030 RVA: 0x00049B10 File Offset: 0x00047D10
	public float DefaultVolume
	{
		get
		{
			return this.defaultVolume;
		}
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x0600272F RID: 10031 RVA: 0x00049B18 File Offset: 0x00047D18
	public bool VolumeOverridden
	{
		get
		{
			return this.volumeOverride != null;
		}
	}

	// Token: 0x06002730 RID: 10032 RVA: 0x00049B25 File Offset: 0x00047D25
	private void Awake()
	{
		if (this.audioSource == null)
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		if (this.setDefaultVolumeFromAudioSourceOnAwake)
		{
			this.defaultVolume = this.audioSource.volume;
		}
	}

	// Token: 0x06002731 RID: 10033 RVA: 0x00049B5A File Offset: 0x00047D5A
	private void OnEnable()
	{
		if (MusicManager.Instance != null)
		{
			MusicManager.Instance.RegisterMusicSource(this);
		}
	}

	// Token: 0x06002732 RID: 10034 RVA: 0x00049B78 File Offset: 0x00047D78
	private void OnDisable()
	{
		if (MusicManager.Instance != null)
		{
			MusicManager.Instance.UnregisterMusicSource(this);
		}
	}

	// Token: 0x06002733 RID: 10035 RVA: 0x00049B96 File Offset: 0x00047D96
	public void SetVolumeOverride(float volume)
	{
		this.volumeOverride = new float?(volume);
		this.audioSource.volume = this.volumeOverride.Value;
	}

	// Token: 0x06002734 RID: 10036 RVA: 0x00049BBA File Offset: 0x00047DBA
	public void UnsetVolumeOverride()
	{
		this.volumeOverride = null;
		this.audioSource.volume = this.defaultVolume;
	}

	// Token: 0x04002AF8 RID: 11000
	[SerializeField]
	private float defaultVolume = 1f;

	// Token: 0x04002AF9 RID: 11001
	[SerializeField]
	private bool setDefaultVolumeFromAudioSourceOnAwake = true;

	// Token: 0x04002AFA RID: 11002
	private AudioSource audioSource;

	// Token: 0x04002AFB RID: 11003
	private float? volumeOverride;
}
