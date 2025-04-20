using System;
using UnityEngine;

// Token: 0x02000603 RID: 1539
[RequireComponent(typeof(AudioSource))]
public class MusicSource : MonoBehaviour
{
	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x06002650 RID: 9808 RVA: 0x0004A09D File Offset: 0x0004829D
	public AudioSource AudioSource
	{
		get
		{
			return this.audioSource;
		}
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x06002651 RID: 9809 RVA: 0x0004A0A5 File Offset: 0x000482A5
	public float DefaultVolume
	{
		get
		{
			return this.defaultVolume;
		}
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x06002652 RID: 9810 RVA: 0x0004A0AD File Offset: 0x000482AD
	public bool VolumeOverridden
	{
		get
		{
			return this.volumeOverride != null;
		}
	}

	// Token: 0x06002653 RID: 9811 RVA: 0x0004A0BA File Offset: 0x000482BA
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

	// Token: 0x06002654 RID: 9812 RVA: 0x0004A0EF File Offset: 0x000482EF
	private void OnEnable()
	{
		if (MusicManager.Instance != null)
		{
			MusicManager.Instance.RegisterMusicSource(this);
		}
	}

	// Token: 0x06002655 RID: 9813 RVA: 0x0004A10D File Offset: 0x0004830D
	private void OnDisable()
	{
		if (MusicManager.Instance != null)
		{
			MusicManager.Instance.UnregisterMusicSource(this);
		}
	}

	// Token: 0x06002656 RID: 9814 RVA: 0x0004A12B File Offset: 0x0004832B
	public void SetVolumeOverride(float volume)
	{
		this.volumeOverride = new float?(volume);
		this.audioSource.volume = this.volumeOverride.Value;
	}

	// Token: 0x06002657 RID: 9815 RVA: 0x0004A14F File Offset: 0x0004834F
	public void UnsetVolumeOverride()
	{
		this.volumeOverride = null;
		this.audioSource.volume = this.defaultVolume;
	}

	// Token: 0x04002A58 RID: 10840
	[SerializeField]
	private float defaultVolume = 1f;

	// Token: 0x04002A59 RID: 10841
	[SerializeField]
	private bool setDefaultVolumeFromAudioSourceOnAwake = true;

	// Token: 0x04002A5A RID: 10842
	private AudioSource audioSource;

	// Token: 0x04002A5B RID: 10843
	private float? volumeOverride;
}
