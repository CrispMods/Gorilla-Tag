using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000874 RID: 2164
public class SoundBankPlayer : MonoBehaviour
{
	// Token: 0x17000564 RID: 1380
	// (get) Token: 0x06003450 RID: 13392 RVA: 0x000F8FA4 File Offset: 0x000F71A4
	public bool isPlaying
	{
		get
		{
			return Time.realtimeSinceStartup < this.playEndTime;
		}
	}

	// Token: 0x17000565 RID: 1381
	// (get) Token: 0x06003451 RID: 13393 RVA: 0x000F8FB3 File Offset: 0x000F71B3
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

	// Token: 0x17000566 RID: 1382
	// (get) Token: 0x06003452 RID: 13394 RVA: 0x000F8FDA File Offset: 0x000F71DA
	public float CurrentTime
	{
		get
		{
			return Time.realtimeSinceStartup - this.playStartTime;
		}
	}

	// Token: 0x06003453 RID: 13395 RVA: 0x000F8FE8 File Offset: 0x000F71E8
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

	// Token: 0x06003454 RID: 13396 RVA: 0x000F92E1 File Offset: 0x000F74E1
	protected void OnEnable()
	{
		if (this.playOnEnable)
		{
			this.Play();
		}
	}

	// Token: 0x06003455 RID: 13397 RVA: 0x000F92F4 File Offset: 0x000F74F4
	public void Play()
	{
		this.Play(null, null);
	}

	// Token: 0x06003456 RID: 13398 RVA: 0x000F931C File Offset: 0x000F751C
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

	// Token: 0x06003457 RID: 13399 RVA: 0x000F9459 File Offset: 0x000F7659
	public void RestartSequence()
	{
		this.nextIndex = 0;
	}

	// Token: 0x04003733 RID: 14131
	[Tooltip("Optional. AudioSource Settings will be used if this is not defined.")]
	public AudioSource audioSource;

	// Token: 0x04003734 RID: 14132
	public bool playOnEnable = true;

	// Token: 0x04003735 RID: 14133
	public bool shuffleOrder = true;

	// Token: 0x04003736 RID: 14134
	public bool missingSoundsAreOk;

	// Token: 0x04003737 RID: 14135
	public SoundBankSO soundBank;

	// Token: 0x04003738 RID: 14136
	public AudioMixerGroup outputAudioMixerGroup;

	// Token: 0x04003739 RID: 14137
	public bool spatialize;

	// Token: 0x0400373A RID: 14138
	public bool spatializePostEffects;

	// Token: 0x0400373B RID: 14139
	public bool bypassEffects;

	// Token: 0x0400373C RID: 14140
	public bool bypassListenerEffects;

	// Token: 0x0400373D RID: 14141
	public bool bypassReverbZones;

	// Token: 0x0400373E RID: 14142
	public int priority = 128;

	// Token: 0x0400373F RID: 14143
	[Range(0f, 1f)]
	public float spatialBlend = 1f;

	// Token: 0x04003740 RID: 14144
	public float reverbZoneMix = 1f;

	// Token: 0x04003741 RID: 14145
	public float dopplerLevel = 1f;

	// Token: 0x04003742 RID: 14146
	public float spread;

	// Token: 0x04003743 RID: 14147
	public AudioRolloffMode rolloffMode;

	// Token: 0x04003744 RID: 14148
	public float minDistance = 1f;

	// Token: 0x04003745 RID: 14149
	public float maxDistance = 100f;

	// Token: 0x04003746 RID: 14150
	public AnimationCurve customRolloffCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

	// Token: 0x04003747 RID: 14151
	private int nextIndex;

	// Token: 0x04003748 RID: 14152
	private float playStartTime;

	// Token: 0x04003749 RID: 14153
	private float playEndTime;

	// Token: 0x0400374A RID: 14154
	private float clipDuration;

	// Token: 0x0400374B RID: 14155
	private SoundBankPlayer.PlaylistEntry[] playlist;

	// Token: 0x02000875 RID: 2165
	private struct PlaylistEntry
	{
		// Token: 0x0400374C RID: 14156
		public int index;

		// Token: 0x0400374D RID: 14157
		public float volume;

		// Token: 0x0400374E RID: 14158
		public float pitch;
	}
}
