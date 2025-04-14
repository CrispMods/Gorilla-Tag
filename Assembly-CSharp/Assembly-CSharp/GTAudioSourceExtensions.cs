using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public static class GTAudioSourceExtensions
{
	// Token: 0x060009FD RID: 2557 RVA: 0x000377AF File Offset: 0x000359AF
	public static void GTPlayOneShot(this AudioSource sound, IList<AudioClip> clips, float volumeScale = 1f)
	{
		sound.PlayOneShot(clips[Random.Range(0, clips.Count)], volumeScale);
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x000377CA File Offset: 0x000359CA
	public static void GTPlayOneShot(this AudioSource audioSource, AudioClip clip, float volumeScale = 1f)
	{
		audioSource.PlayOneShot(clip, volumeScale);
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x000377D4 File Offset: 0x000359D4
	public static void GTPlay(this AudioSource audioSource)
	{
		audioSource.Play();
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x000377DC File Offset: 0x000359DC
	public static void GTPlay(this AudioSource audioSource, ulong delay)
	{
		audioSource.Play(delay);
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x000377E5 File Offset: 0x000359E5
	public static void GTPause(this AudioSource audioSource)
	{
		audioSource.Pause();
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x000377ED File Offset: 0x000359ED
	public static void GTUnPause(this AudioSource audioSource)
	{
		audioSource.UnPause();
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x000377F5 File Offset: 0x000359F5
	public static void GTStop(this AudioSource audioSource)
	{
		audioSource.Stop();
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x000377FD File Offset: 0x000359FD
	public static void GTPlayDelayed(this AudioSource audioSource, float delay)
	{
		audioSource.PlayDelayed(delay);
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x00037806 File Offset: 0x00035A06
	public static void GTPlayScheduled(this AudioSource audioSource, double time)
	{
		audioSource.PlayScheduled(time);
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x0003780F File Offset: 0x00035A0F
	public static void GTPlayClipAtPoint(AudioClip clip, Vector3 position)
	{
		AudioSource.PlayClipAtPoint(clip, position);
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x00037818 File Offset: 0x00035A18
	public static void GTPlayClipAtPoint(AudioClip clip, Vector3 position, float volume)
	{
		AudioSource.PlayClipAtPoint(clip, position, volume);
	}
}
