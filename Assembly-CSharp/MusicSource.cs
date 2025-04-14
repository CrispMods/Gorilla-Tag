using System;
using UnityEngine;

// Token: 0x02000624 RID: 1572
[RequireComponent(typeof(AudioSource))]
public class MusicSource : MonoBehaviour
{
	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x06002725 RID: 10021 RVA: 0x000C0920 File Offset: 0x000BEB20
	public AudioSource AudioSource
	{
		get
		{
			return this.audioSource;
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x06002726 RID: 10022 RVA: 0x000C0928 File Offset: 0x000BEB28
	public float DefaultVolume
	{
		get
		{
			return this.defaultVolume;
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x06002727 RID: 10023 RVA: 0x000C0930 File Offset: 0x000BEB30
	public bool VolumeOverridden
	{
		get
		{
			return this.volumeOverride != null;
		}
	}

	// Token: 0x06002728 RID: 10024 RVA: 0x000C093D File Offset: 0x000BEB3D
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

	// Token: 0x06002729 RID: 10025 RVA: 0x000C0972 File Offset: 0x000BEB72
	private void OnEnable()
	{
		if (MusicManager.Instance != null)
		{
			MusicManager.Instance.RegisterMusicSource(this);
		}
	}

	// Token: 0x0600272A RID: 10026 RVA: 0x000C0990 File Offset: 0x000BEB90
	private void OnDisable()
	{
		if (MusicManager.Instance != null)
		{
			MusicManager.Instance.UnregisterMusicSource(this);
		}
	}

	// Token: 0x0600272B RID: 10027 RVA: 0x000C09AE File Offset: 0x000BEBAE
	public void SetVolumeOverride(float volume)
	{
		this.volumeOverride = new float?(volume);
		this.audioSource.volume = this.volumeOverride.Value;
	}

	// Token: 0x0600272C RID: 10028 RVA: 0x000C09D2 File Offset: 0x000BEBD2
	public void UnsetVolumeOverride()
	{
		this.volumeOverride = null;
		this.audioSource.volume = this.defaultVolume;
	}

	// Token: 0x04002AF2 RID: 10994
	[SerializeField]
	private float defaultVolume = 1f;

	// Token: 0x04002AF3 RID: 10995
	[SerializeField]
	private bool setDefaultVolumeFromAudioSourceOnAwake = true;

	// Token: 0x04002AF4 RID: 10996
	private AudioSource audioSource;

	// Token: 0x04002AF5 RID: 10997
	private float? volumeOverride;
}
