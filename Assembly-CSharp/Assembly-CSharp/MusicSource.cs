using System;
using UnityEngine;

// Token: 0x02000625 RID: 1573
[RequireComponent(typeof(AudioSource))]
public class MusicSource : MonoBehaviour
{
	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x0600272D RID: 10029 RVA: 0x000C0DA0 File Offset: 0x000BEFA0
	public AudioSource AudioSource
	{
		get
		{
			return this.audioSource;
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x0600272E RID: 10030 RVA: 0x000C0DA8 File Offset: 0x000BEFA8
	public float DefaultVolume
	{
		get
		{
			return this.defaultVolume;
		}
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x0600272F RID: 10031 RVA: 0x000C0DB0 File Offset: 0x000BEFB0
	public bool VolumeOverridden
	{
		get
		{
			return this.volumeOverride != null;
		}
	}

	// Token: 0x06002730 RID: 10032 RVA: 0x000C0DBD File Offset: 0x000BEFBD
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

	// Token: 0x06002731 RID: 10033 RVA: 0x000C0DF2 File Offset: 0x000BEFF2
	private void OnEnable()
	{
		if (MusicManager.Instance != null)
		{
			MusicManager.Instance.RegisterMusicSource(this);
		}
	}

	// Token: 0x06002732 RID: 10034 RVA: 0x000C0E10 File Offset: 0x000BF010
	private void OnDisable()
	{
		if (MusicManager.Instance != null)
		{
			MusicManager.Instance.UnregisterMusicSource(this);
		}
	}

	// Token: 0x06002733 RID: 10035 RVA: 0x000C0E2E File Offset: 0x000BF02E
	public void SetVolumeOverride(float volume)
	{
		this.volumeOverride = new float?(volume);
		this.audioSource.volume = this.volumeOverride.Value;
	}

	// Token: 0x06002734 RID: 10036 RVA: 0x000C0E52 File Offset: 0x000BF052
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
