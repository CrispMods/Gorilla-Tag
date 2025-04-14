using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000871 RID: 2161
public class SoundBankPlayer : MonoBehaviour
{
	// Token: 0x17000563 RID: 1379
	// (get) Token: 0x06003444 RID: 13380 RVA: 0x000F89DC File Offset: 0x000F6BDC
	public bool isPlaying
	{
		get
		{
			return Time.realtimeSinceStartup < this.playEndTime;
		}
	}

	// Token: 0x17000564 RID: 1380
	// (get) Token: 0x06003445 RID: 13381 RVA: 0x000F89EB File Offset: 0x000F6BEB
	public float NormalizedTime
	{
		get
		{
			if (this.clipDuration != 0f)
			{
				return Mathf.Clamp01(this.CurrentTime / this.clipDuration);
			}
			return 1f;
		}
	}

	// Token: 0x17000565 RID: 1381
	// (get) Token: 0x06003446 RID: 13382 RVA: 0x000F8A12 File Offset: 0x000F6C12
	public float CurrentTime
	{
		get
		{
			return Time.realtimeSinceStartup - this.playStartTime;
		}
	}

	// Token: 0x06003447 RID: 13383 RVA: 0x000F8A20 File Offset: 0x000F6C20
	protected void Awake()
	{
		if (this.audioSource == null)
		{
			this.audioSource = base.gameObject.AddComponent<AudioSource>();
			this.audioSource.outputAudioMixerGroup = this.outputAudioMixerGroup;
			this.audioSource.spatialize = this.spatialize;
			this.audioSource.spatializePostEffects = this.spatializePostEffects;
			this.audioSource.bypassEffects = this.bypassEffects;
			this.audioSource.bypassListenerEffects = this.bypassListenerEffects;
			this.audioSource.bypassReverbZones = this.bypassReverbZones;
			this.audioSource.priority = this.priority;
			this.audioSource.spatialBlend = this.spatialBlend;
			this.audioSource.dopplerLevel = this.dopplerLevel;
			this.audioSource.spread = this.spread;
			this.audioSource.rolloffMode = this.rolloffMode;
			this.audioSource.minDistance = this.minDistance;
			this.audioSource.maxDistance = this.maxDistance;
			this.audioSource.reverbZoneMix = this.reverbZoneMix;
		}
		this.audioSource.volume = 1f;
		this.audioSource.playOnAwake = false;
		if (this.shuffleOrder)
		{
			int[] array = new int[this.soundBank.sounds.Length / 2];
			this.playlist = new SoundBankPlayer.PlaylistEntry[this.soundBank.sounds.Length * 8];
			for (int i = 0; i < this.playlist.Length; i++)
			{
				int num = 0;
				for (int j = 0; j < 100; j++)
				{
					num = Random.Range(0, this.soundBank.sounds.Length);
					if (Array.IndexOf<int>(array, num) == -1)
					{
						break;
					}
				}
				if (array.Length != 0)
				{
					array[i % array.Length] = num;
				}
				this.playlist[i] = new SoundBankPlayer.PlaylistEntry
				{
					index = num,
					volume = Random.Range(this.soundBank.volumeRange.x, this.soundBank.volumeRange.y),
					pitch = Random.Range(this.soundBank.pitchRange.x, this.soundBank.pitchRange.y)
				};
			}
			return;
		}
		this.playlist = new SoundBankPlayer.PlaylistEntry[this.soundBank.sounds.Length * 8];
		for (int k = 0; k < this.playlist.Length; k++)
		{
			this.playlist[k] = new SoundBankPlayer.PlaylistEntry
			{
				index = k % this.soundBank.sounds.Length,
				volume = Random.Range(this.soundBank.volumeRange.x, this.soundBank.volumeRange.y),
				pitch = Random.Range(this.soundBank.pitchRange.x, this.soundBank.pitchRange.y)
			};
		}
	}

	// Token: 0x06003448 RID: 13384 RVA: 0x000F8D19 File Offset: 0x000F6F19
	protected void OnEnable()
	{
		if (this.playOnEnable)
		{
			this.Play();
		}
	}

	// Token: 0x06003449 RID: 13385 RVA: 0x000F8D2C File Offset: 0x000F6F2C
	public void Play()
	{
		this.Play(null, null);
	}

	// Token: 0x0600344A RID: 13386 RVA: 0x000F8D54 File Offset: 0x000F6F54
	public void Play(float? volumeOverride = null, float? pitchOverride = null)
	{
		if (!base.enabled || this.soundBank.sounds.Length == 0)
		{
			return;
		}
		SoundBankPlayer.PlaylistEntry playlistEntry = this.playlist[this.nextIndex];
		this.audioSource.pitch = ((pitchOverride != null) ? pitchOverride.Value : playlistEntry.pitch);
		AudioClip audioClip = this.soundBank.sounds[playlistEntry.index];
		if (audioClip != null)
		{
			this.audioSource.GTPlayOneShot(audioClip, (volumeOverride != null) ? volumeOverride.Value : playlistEntry.volume);
			this.clipDuration = audioClip.length;
			this.playStartTime = Time.realtimeSinceStartup;
			this.playEndTime = Mathf.Max(this.playEndTime, this.playStartTime + audioClip.length);
			this.nextIndex = (this.nextIndex + 1) % this.playlist.Length;
			return;
		}
		if (this.missingSoundsAreOk)
		{
			this.clipDuration = 0f;
			this.nextIndex = (this.nextIndex + 1) % this.playlist.Length;
			return;
		}
		Debug.LogErrorFormat("Sounds bank {0} is missing a clip at {1}", new object[]
		{
			base.gameObject.name,
			playlistEntry.index
		});
	}

	// Token: 0x0600344B RID: 13387 RVA: 0x000F8E91 File Offset: 0x000F7091
	public void RestartSequence()
	{
		this.nextIndex = 0;
	}

	// Token: 0x04003721 RID: 14113
	[Tooltip("Optional. AudioSource Settings will be used if this is not defined.")]
	public AudioSource audioSource;

	// Token: 0x04003722 RID: 14114
	public bool playOnEnable = true;

	// Token: 0x04003723 RID: 14115
	public bool shuffleOrder = true;

	// Token: 0x04003724 RID: 14116
	public bool missingSoundsAreOk;

	// Token: 0x04003725 RID: 14117
	public SoundBankSO soundBank;

	// Token: 0x04003726 RID: 14118
	public AudioMixerGroup outputAudioMixerGroup;

	// Token: 0x04003727 RID: 14119
	public bool spatialize;

	// Token: 0x04003728 RID: 14120
	public bool spatializePostEffects;

	// Token: 0x04003729 RID: 14121
	public bool bypassEffects;

	// Token: 0x0400372A RID: 14122
	public bool bypassListenerEffects;

	// Token: 0x0400372B RID: 14123
	public bool bypassReverbZones;

	// Token: 0x0400372C RID: 14124
	public int priority = 128;

	// Token: 0x0400372D RID: 14125
	[Range(0f, 1f)]
	public float spatialBlend = 1f;

	// Token: 0x0400372E RID: 14126
	public float reverbZoneMix = 1f;

	// Token: 0x0400372F RID: 14127
	public float dopplerLevel = 1f;

	// Token: 0x04003730 RID: 14128
	public float spread;

	// Token: 0x04003731 RID: 14129
	public AudioRolloffMode rolloffMode;

	// Token: 0x04003732 RID: 14130
	public float minDistance = 1f;

	// Token: 0x04003733 RID: 14131
	public float maxDistance = 100f;

	// Token: 0x04003734 RID: 14132
	public AnimationCurve customRolloffCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

	// Token: 0x04003735 RID: 14133
	private int nextIndex;

	// Token: 0x04003736 RID: 14134
	private float playStartTime;

	// Token: 0x04003737 RID: 14135
	private float playEndTime;

	// Token: 0x04003738 RID: 14136
	private float clipDuration;

	// Token: 0x04003739 RID: 14137
	private SoundBankPlayer.PlaylistEntry[] playlist;

	// Token: 0x02000872 RID: 2162
	private struct PlaylistEntry
	{
		// Token: 0x0400373A RID: 14138
		public int index;

		// Token: 0x0400373B RID: 14139
		public float volume;

		// Token: 0x0400373C RID: 14140
		public float pitch;
	}
}
