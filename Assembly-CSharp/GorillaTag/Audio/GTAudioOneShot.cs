using System;
using UnityEngine;

namespace GorillaTag.Audio
{
	// Token: 0x02000C00 RID: 3072
	internal static class GTAudioOneShot
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06004CE9 RID: 19689 RVA: 0x0017624F File Offset: 0x0017444F
		// (set) Token: 0x06004CEA RID: 19690 RVA: 0x00176256 File Offset: 0x00174456
		[OnEnterPlay_Set(false)]
		internal static bool isInitialized { get; private set; }

		// Token: 0x06004CEB RID: 19691 RVA: 0x00176260 File Offset: 0x00174460
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
			GTAudioOneShot.audioSource = Object.Instantiate<AudioSource>(audioSource);
			GTAudioOneShot.defaultCurve = GTAudioOneShot.audioSource.GetCustomCurve(AudioSourceCurveType.CustomRolloff);
			Object.DontDestroyOnLoad(GTAudioOneShot.audioSource);
			GTAudioOneShot.isInitialized = true;
		}

		// Token: 0x06004CEC RID: 19692 RVA: 0x001762BF File Offset: 0x001744BF
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

		// Token: 0x06004CED RID: 19693 RVA: 0x001762F7 File Offset: 0x001744F7
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

		// Token: 0x04004F39 RID: 20281
		[OnEnterPlay_SetNull]
		internal static AudioSource audioSource;

		// Token: 0x04004F3A RID: 20282
		[OnEnterPlay_SetNull]
		internal static AnimationCurve defaultCurve;
	}
}
