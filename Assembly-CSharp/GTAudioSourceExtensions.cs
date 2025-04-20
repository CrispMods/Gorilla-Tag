using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001AF RID: 431
public static class GTAudioSourceExtensions
{
	// Token: 0x06000A47 RID: 2631 RVA: 0x0003734D File Offset: 0x0003554D
	public static void GTPlayOneShot(this AudioSource sound, IList<AudioClip> clips, float volumeScale = 1f)
	{
		sound.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Count)], volumeScale);
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x00037368 File Offset: 0x00035568
	public static void GTPlayOneShot(this AudioSource audioSource, AudioClip clip, float volumeScale = 1f)
	{
		audioSource.PlayOneShot(clip, volumeScale);
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x00037372 File Offset: 0x00035572
	public static void GTPlay(this AudioSource audioSource)
	{
		audioSource.Play();
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x0003737A File Offset: 0x0003557A
	public static void GTPlay(this AudioSource audioSource, ulong delay)
	{
		audioSource.Play(delay);
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x00037383 File Offset: 0x00035583
	public static void GTPause(this AudioSource audioSource)
	{
		audioSource.Pause();
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x0003738B File Offset: 0x0003558B
	public static void GTUnPause(this AudioSource audioSource)
	{
		audioSource.UnPause();
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x00037393 File Offset: 0x00035593
	public static void GTStop(this AudioSource audioSource)
	{
		audioSource.Stop();
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x0003739B File Offset: 0x0003559B
	public static void GTPlayDelayed(this AudioSource audioSource, float delay)
	{
		audioSource.PlayDelayed(delay);
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x000373A4 File Offset: 0x000355A4
	public static void GTPlayScheduled(this AudioSource audioSource, double time)
	{
		audioSource.PlayScheduled(time);
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x000373AD File Offset: 0x000355AD
	public static void GTPlayClipAtPoint(AudioClip clip, Vector3 position)
	{
		AudioSource.PlayClipAtPoint(clip, position);
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x000373B6 File Offset: 0x000355B6
	public static void GTPlayClipAtPoint(AudioClip clip, Vector3 position, float volume)
	{
		AudioSource.PlayClipAtPoint(clip, position, volume);
	}
}
