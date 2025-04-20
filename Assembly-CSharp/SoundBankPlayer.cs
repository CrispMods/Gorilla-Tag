using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200088D RID: 2189
public class SoundBankPlayer : MonoBehaviour
{
	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x06003510 RID: 13584 RVA: 0x00052F96 File Offset: 0x00051196
	public bool isPlaying
	{
		get
		{
			return Time.realtimeSinceStartup < this.playEndTime;
		}
	}

	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x06003511 RID: 13585 RVA: 0x00052FA5 File Offset: 0x000511A5
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

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x06003512 RID: 13586 RVA: 0x00052FCC File Offset: 0x000511CC
	public float CurrentTime
	{
		get
		{
			return Time.realtimeSinceStartup - this.playStartTime;
		}
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x0013F994 File Offset: 0x0013DB94
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
					num = UnityEngine.Random.Range(0, this.soundBank.sounds.Length);
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
					volume = UnityEngine.Random.Range(this.soundBank.volumeRange.x, this.soundBank.volumeRange.y),
					pitch = UnityEngine.Random.Range(this.soundBank.pitchRange.x, this.soundBank.pitchRange.y)
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
				volume = UnityEngine.Random.Range(this.soundBank.volumeRange.x, this.soundBank.volumeRange.y),
				pitch = UnityEngine.Random.Range(this.soundBank.pitchRange.x, this.soundBank.pitchRange.y)
			};
		}
	}

	// Token: 0x06003514 RID: 13588 RVA: 0x00052FDA File Offset: 0x000511DA
	protected void OnEnable()
	{
		if (this.playOnEnable)
		{
			this.Play();
		}
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x0013FC90 File Offset: 0x0013DE90
	public void Play()
	{
		this.Play(null, null);
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x0013FCB8 File Offset: 0x0013DEB8
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

	// Token: 0x06003517 RID: 13591 RVA: 0x00052FEA File Offset: 0x000511EA
	public void RestartSequence()
	{
		this.nextIndex = 0;
	}

	// Token: 0x040037E1 RID: 14305
	[Tooltip("Optional. AudioSource Settings will be used if this is not defined.")]
	public AudioSource audioSource;

	// Token: 0x040037E2 RID: 14306
	public bool playOnEnable = true;

	// Token: 0x040037E3 RID: 14307
	public bool shuffleOrder = true;

	// Token: 0x040037E4 RID: 14308
	public bool missingSoundsAreOk;

	// Token: 0x040037E5 RID: 14309
	public SoundBankSO soundBank;

	// Token: 0x040037E6 RID: 14310
	public AudioMixerGroup outputAudioMixerGroup;

	// Token: 0x040037E7 RID: 14311
	public bool spatialize;

	// Token: 0x040037E8 RID: 14312
	public bool spatializePostEffects;

	// Token: 0x040037E9 RID: 14313
	public bool bypassEffects;

	// Token: 0x040037EA RID: 14314
	public bool bypassListenerEffects;

	// Token: 0x040037EB RID: 14315
	public bool bypassReverbZones;

	// Token: 0x040037EC RID: 14316
	public int priority = 128;

	// Token: 0x040037ED RID: 14317
	[Range(0f, 1f)]
	public float spatialBlend = 1f;

	// Token: 0x040037EE RID: 14318
	public float reverbZoneMix = 1f;

	// Token: 0x040037EF RID: 14319
	public float dopplerLevel = 1f;

	// Token: 0x040037F0 RID: 14320
	public float spread;

	// Token: 0x040037F1 RID: 14321
	public AudioRolloffMode rolloffMode;

	// Token: 0x040037F2 RID: 14322
	public float minDistance = 1f;

	// Token: 0x040037F3 RID: 14323
	public float maxDistance = 100f;

	// Token: 0x040037F4 RID: 14324
	public AnimationCurve customRolloffCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

	// Token: 0x040037F5 RID: 14325
	private int nextIndex;

	// Token: 0x040037F6 RID: 14326
	private float playStartTime;

	// Token: 0x040037F7 RID: 14327
	private float playEndTime;

	// Token: 0x040037F8 RID: 14328
	private float clipDuration;

	// Token: 0x040037F9 RID: 14329
	private SoundBankPlayer.PlaylistEntry[] playlist;

	// Token: 0x0200088E RID: 2190
	private struct PlaylistEntry
	{
		// Token: 0x040037FA RID: 14330
		public int index;

		// Token: 0x040037FB RID: 14331
		public float volume;

		// Token: 0x040037FC RID: 14332
		public float pitch;
	}
}
