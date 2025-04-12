using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public static class GTAudioSourceExtensions
{
	// Token: 0x060009FD RID: 2557 RVA: 0x0003608D File Offset: 0x0003428D
	public static void GTPlayOneShot(this AudioSource sound, IList<AudioClip> clips, float volumeScale = 1f)
	{
		sound.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Count)], volumeScale);
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x000360A8 File Offset: 0x000342A8
	public static void GTPlayOneShot(this AudioSource audioSource, AudioClip clip, float volumeScale = 1f)
	{
		audioSource.PlayOneShot(clip, volumeScale);
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x000360B2 File Offset: 0x000342B2
	public static void GTPlay(this AudioSource audioSource)
	{
		audioSource.Play();
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x000360BA File Offset: 0x000342BA
	public static void GTPlay(this AudioSource audioSource, ulong delay)
	{
		audioSource.Play(delay);
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x000360C3 File Offset: 0x000342C3
	public static void GTPause(this AudioSource audioSource)
	{
		audioSource.Pause();
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x000360CB File Offset: 0x000342CB
	public static void GTUnPause(this AudioSource audioSource)
	{
		audioSource.UnPause();
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x000360D3 File Offset: 0x000342D3
	public static void GTStop(this AudioSource audioSource)
	{
		audioSource.Stop();
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x000360DB File Offset: 0x000342DB
	public static void GTPlayDelayed(this AudioSource audioSource, float delay)
	{
		audioSource.PlayDelayed(delay);
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x000360E4 File Offset: 0x000342E4
	public static void GTPlayScheduled(this AudioSource audioSource, double time)
	{
		audioSource.PlayScheduled(time);
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x000360ED File Offset: 0x000342ED
	public static void GTPlayClipAtPoint(AudioClip clip, Vector3 position)
	{
		AudioSource.PlayClipAtPoint(clip, position);
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x000360F6 File Offset: 0x000342F6
	public static void GTPlayClipAtPoint(AudioClip clip, Vector3 position, float volume)
	{
		AudioSource.PlayClipAtPoint(clip, position, volume);
	}
}
