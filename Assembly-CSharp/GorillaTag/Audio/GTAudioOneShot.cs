using System;
using UnityEngine;

namespace GorillaTag.Audio
{
	// Token: 0x02000C2E RID: 3118
	internal static class GTAudioOneShot
	{
		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06004E35 RID: 20021 RVA: 0x000632A1 File Offset: 0x000614A1
		// (set) Token: 0x06004E36 RID: 20022 RVA: 0x000632A8 File Offset: 0x000614A8
		[OnEnterPlay_Set(false)]
		internal static bool isInitialized { get; private set; }

		// Token: 0x06004E37 RID: 20023 RVA: 0x001AE688 File Offset: 0x001AC888
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			if (GTAudioOneShot.isInitialized)
			{
				return;
			}
			AudioSource audioSource = Resources.Load<AudioSource>("AudioSourceSingleton_Prefab");
			if (audioSource == null)
			{
				Debug.LogError("GTAudioOneShot: Failed to load AudioSourceSingleton_Prefab from resources!!!");
				return;
			}
			GTAudioOneShot.audioSource = UnityEngine.Object.Instantiate<AudioSource>(audioSource);
			GTAudioOneShot.defaultCurve = GTAudioOneShot.audioSource.GetCustomCurve(AudioSourceCurveType.CustomRolloff);
			UnityEngine.Object.DontDestroyOnLoad(GTAudioOneShot.audioSource);
			GTAudioOneShot.isInitialized = true;
		}

		// Token: 0x06004E38 RID: 20024 RVA: 0x000632B0 File Offset: 0x000614B0
		internal static void Play(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
		{
			if (ApplicationQuittingState.IsQuitting || !GTAudioOneShot.isInitialized)
			{
				return;
			}
			GTAudioOneShot.audioSource.pitch = pitch;
			GTAudioOneShot.audioSource.transform.position = position;
			GTAudioOneShot.audioSource.GTPlayOneShot(clip, volume);
		}

		// Token: 0x06004E39 RID: 20025 RVA: 0x000632E8 File Offset: 0x000614E8
		internal static void Play(AudioClip clip, Vector3 position, AnimationCurve curve, float volume = 1f, float pitch = 1f)
		{
			if (ApplicationQuittingState.IsQuitting || !GTAudioOneShot.isInitialized)
			{
				return;
			}
			GTAudioOneShot.audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, curve);
			GTAudioOneShot.Play(clip, position, volume, pitch);
			GTAudioOneShot.audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, GTAudioOneShot.defaultCurve);
		}

		// Token: 0x0400502F RID: 20527
		[OnEnterPlay_SetNull]
		internal static AudioSource audioSource;

		// Token: 0x04005030 RID: 20528
		[OnEnterPlay_SetNull]
		internal static AnimationCurve defaultCurve;
	}
}
