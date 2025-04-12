using System;
using UnityEngine;

namespace GorillaTag.Audio
{
	// Token: 0x02000C03 RID: 3075
	internal static class GTAudioOneShot
	{
		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06004CF5 RID: 19701 RVA: 0x000618E0 File Offset: 0x0005FAE0
		// (set) Token: 0x06004CF6 RID: 19702 RVA: 0x000618E7 File Offset: 0x0005FAE7
		[OnEnterPlay_Set(false)]
		internal static bool isInitialized { get; private set; }

		// Token: 0x06004CF7 RID: 19703 RVA: 0x001A76BC File Offset: 0x001A58BC
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

		// Token: 0x06004CF8 RID: 19704 RVA: 0x000618EF File Offset: 0x0005FAEF
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

		// Token: 0x06004CF9 RID: 19705 RVA: 0x00061927 File Offset: 0x0005FB27
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

		// Token: 0x04004F4B RID: 20299
		[OnEnterPlay_SetNull]
		internal static AudioSource audioSource;

		// Token: 0x04004F4C RID: 20300
		[OnEnterPlay_SetNull]
		internal static AnimationCurve defaultCurve;
	}
}
