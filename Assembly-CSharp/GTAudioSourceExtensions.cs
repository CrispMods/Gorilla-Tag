using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public static class GTAudioSourceExtensions
{
	// Token: 0x060009FB RID: 2555 RVA: 0x0003748B File Offset: 0x0003568B
	public static void GTPlayOneShot(this AudioSource sound, IList<AudioClip> clips, float volumeScale = 1f)
	{
		sound.PlayOneShot(clips[Random.Range(0, clips.Count)], volumeScale);
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x000374A6 File Offset: 0x000356A6
	public static void GTPlayOneShot(this AudioSource audioSource, AudioClip clip, float volumeScale = 1f)
	{
		audioSource.PlayOneShot(clip, volumeScale);
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x000374B0 File Offset: 0x000356B0
	public static void GTPlay(this AudioSource audioSource)
	{
		audioSource.Play();
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x000374B8 File Offset: 0x000356B8
	public static void GTPlay(this AudioSource audioSource, ulong delay)
	{
		audioSource.Play(delay);
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x000374C1 File Offset: 0x000356C1
	public static void GTPause(this AudioSource audioSource)
	{
		audioSource.Pause();
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x000374C9 File Offset: 0x000356C9
	public static void GTUnPause(this AudioSource audioSource)
	{
		audioSource.UnPause();
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x000374D1 File Offset: 0x000356D1
	public static void GTStop(this AudioSource audioSource)
	{
		audioSource.Stop();
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x000374D9 File Offset: 0x000356D9
	public static void GTPlayDelayed(this AudioSource audioSource, float delay)
	{
		audioSource.PlayDelayed(delay);
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x000374E2 File Offset: 0x000356E2
	public static void GTPlayScheduled(this AudioSource audioSource, double time)
	{
		audioSource.PlayScheduled(time);
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x000374EB File Offset: 0x000356EB
	public static void GTPlayClipAtPoint(AudioClip clip, Vector3 position)
	{
		AudioSource.PlayClipAtPoint(clip, position);
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x000374F4 File Offset: 0x000356F4
	public static void GTPlayClipAtPoint(AudioClip clip, Vector3 position, float volume)
	{
		AudioSource.PlayClipAtPoint(clip, position, volume);
	}
}
