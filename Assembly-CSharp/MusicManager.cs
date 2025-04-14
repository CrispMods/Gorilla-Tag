using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000621 RID: 1569
public class MusicManager : MonoBehaviour
{
	// Token: 0x06002711 RID: 10001 RVA: 0x000C053B File Offset: 0x000BE73B
	private void Awake()
	{
		if (MusicManager.Instance == null)
		{
			MusicManager.Instance = this;
			return;
		}
		Object.Destroy(this);
	}

	// Token: 0x06002712 RID: 10002 RVA: 0x000C055B File Offset: 0x000BE75B
	public void RegisterMusicSource(MusicSource musicSource)
	{
		if (!this.activeSources.Contains(musicSource))
		{
			this.activeSources.Add(musicSource);
		}
	}

	// Token: 0x06002713 RID: 10003 RVA: 0x000C0578 File Offset: 0x000BE778
	public void UnregisterMusicSource(MusicSource musicSource)
	{
		if (this.activeSources.Contains(musicSource))
		{
			this.activeSources.Remove(musicSource);
			musicSource.UnsetVolumeOverride();
		}
	}

	// Token: 0x06002714 RID: 10004 RVA: 0x000C059C File Offset: 0x000BE79C
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

	// Token: 0x06002715 RID: 10005 RVA: 0x000C0610 File Offset: 0x000BE810
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

	// Token: 0x06002716 RID: 10006 RVA: 0x000C0680 File Offset: 0x000BE880
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

	// Token: 0x06002717 RID: 10007 RVA: 0x000C0696 File Offset: 0x000BE896
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

	// Token: 0x04002AE6 RID: 10982
	[OnEnterPlay_SetNull]
	public static volatile MusicManager Instance;

	// Token: 0x04002AE7 RID: 10983
	private HashSet<MusicSource> activeSources = new HashSet<MusicSource>();
}
