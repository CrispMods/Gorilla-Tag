using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000600 RID: 1536
public class MusicManager : MonoBehaviour
{
	// Token: 0x0600263C RID: 9788 RVA: 0x00049FD0 File Offset: 0x000481D0
	private void Awake()
	{
		if (MusicManager.Instance == null)
		{
			MusicManager.Instance = this;
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x0600263D RID: 9789 RVA: 0x00049FF0 File Offset: 0x000481F0
	public void RegisterMusicSource(MusicSource musicSource)
	{
		if (!this.activeSources.Contains(musicSource))
		{
			this.activeSources.Add(musicSource);
		}
	}

	// Token: 0x0600263E RID: 9790 RVA: 0x0004A00D File Offset: 0x0004820D
	public void UnregisterMusicSource(MusicSource musicSource)
	{
		if (this.activeSources.Contains(musicSource))
		{
			this.activeSources.Remove(musicSource);
			musicSource.UnsetVolumeOverride();
		}
	}

	// Token: 0x0600263F RID: 9791 RVA: 0x00108420 File Offset: 0x00106620
	public void FadeOutMusic(float duration = 3f)
	{
		base.StopAllCoroutines();
		if (duration > 0f)
		{
			base.StartCoroutine(this.FadeOutVolumeCoroutine(duration));
			return;
		}
		foreach (MusicSource musicSource in this.activeSources)
		{
			musicSource.SetVolumeOverride(0f);
		}
	}

	// Token: 0x06002640 RID: 9792 RVA: 0x00108494 File Offset: 0x00106694
	public void FadeInMusic(float duration = 3f)
	{
		base.StopAllCoroutines();
		if (duration > 0f)
		{
			base.StartCoroutine(this.FadeInVolumeCoroutine(duration));
			return;
		}
		foreach (MusicSource musicSource in this.activeSources)
		{
			musicSource.UnsetVolumeOverride();
		}
	}

	// Token: 0x06002641 RID: 9793 RVA: 0x0004A030 File Offset: 0x00048230
	private IEnumerator FadeInVolumeCoroutine(float duration)
	{
		bool complete = false;
		while (!complete)
		{
			complete = true;
			float deltaTime = Time.deltaTime;
			foreach (MusicSource musicSource in this.activeSources)
			{
				float num = musicSource.DefaultVolume / duration;
				float volumeOverride = Mathf.MoveTowards(musicSource.AudioSource.volume, musicSource.DefaultVolume, num * deltaTime);
				musicSource.SetVolumeOverride(volumeOverride);
				if (musicSource.AudioSource.volume != musicSource.DefaultVolume)
				{
					complete = false;
				}
			}
			yield return null;
		}
		using (HashSet<MusicSource>.Enumerator enumerator = this.activeSources.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MusicSource musicSource2 = enumerator.Current;
				musicSource2.UnsetVolumeOverride();
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x06002642 RID: 9794 RVA: 0x0004A046 File Offset: 0x00048246
	private IEnumerator FadeOutVolumeCoroutine(float duration)
	{
		bool complete = false;
		while (!complete)
		{
			complete = true;
			float deltaTime = Time.deltaTime;
			foreach (MusicSource musicSource in this.activeSources)
			{
				float num = musicSource.DefaultVolume / duration;
				float volumeOverride = Mathf.MoveTowards(musicSource.AudioSource.volume, 0f, num * deltaTime);
				musicSource.SetVolumeOverride(volumeOverride);
				if (musicSource.AudioSource.volume != 0f)
				{
					complete = false;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002A4C RID: 10828
	[OnEnterPlay_SetNull]
	public static volatile MusicManager Instance;

	// Token: 0x04002A4D RID: 10829
	private HashSet<MusicSource> activeSources = new HashSet<MusicSource>();
}
