using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x020007E9 RID: 2025
public static class FXSystem
{
	// Token: 0x0600320E RID: 12814 RVA: 0x000F0620 File Offset: 0x000EE820
	public static void PlayFXForRig(FXType fxType, IFXContext context, PhotonMessageInfoWrapped info = default(PhotonMessageInfoWrapped))
	{
		FXSystemSettings settings = context.settings;
		if (settings.forLocalRig)
		{
			context.OnPlayFX();
			return;
		}
		if (FXSystem.CheckCallSpam(settings, (int)fxType, info.SentServerTime))
		{
			context.OnPlayFX();
		}
	}

	// Token: 0x0600320F RID: 12815 RVA: 0x000F065C File Offset: 0x000EE85C
	public static void PlayFXForRigValidated(List<int> hashes, FXType fxType, IFXContext context, PhotonMessageInfoWrapped info = default(PhotonMessageInfoWrapped))
	{
		for (int i = 0; i < hashes.Count; i++)
		{
			if (!ObjectPools.instance.DoesPoolExist(hashes[i]))
			{
				return;
			}
		}
		FXSystem.PlayFXForRig(fxType, context, info);
	}

	// Token: 0x06003210 RID: 12816 RVA: 0x000F0698 File Offset: 0x000EE898
	public static void PlayFX<T>(FXType fxType, IFXContextParems<T> context, T args, PhotonMessageInfoWrapped info) where T : FXSArgs
	{
		FXSystemSettings settings = context.settings;
		if (settings.forLocalRig)
		{
			context.OnPlayFX(args);
			return;
		}
		if (FXSystem.CheckCallSpam(settings, (int)fxType, info.SentServerTime))
		{
			context.OnPlayFX(args);
		}
	}

	// Token: 0x06003211 RID: 12817 RVA: 0x000F06D4 File Offset: 0x000EE8D4
	public static void PlayFXForRig<T>(FXType fxType, IFXEffectContext<T> context, PhotonMessageInfoWrapped info) where T : IFXEffectContextObject
	{
		FXSystemSettings settings = context.settings;
		if (!settings.forLocalRig && !FXSystem.CheckCallSpam(settings, (int)fxType, info.SentServerTime))
		{
			return;
		}
		FXSystem.PlayFX(context.effectContext);
	}

	// Token: 0x06003212 RID: 12818 RVA: 0x000F0714 File Offset: 0x000EE914
	public static void PlayFX(IFXEffectContextObject effectContext)
	{
		int[] prefabPoolIds = effectContext.PrefabPoolIds;
		if (prefabPoolIds != null)
		{
			int num = prefabPoolIds.Length;
			for (int i = 0; i < num; i++)
			{
				int num2 = prefabPoolIds[i];
				if (num2 != -1)
				{
					GameObject gameObject = ObjectPools.instance.Instantiate(num2);
					gameObject.transform.position = effectContext.Positon;
					gameObject.transform.rotation = effectContext.Rotation;
					effectContext.OnPlayVisualFX(num2, gameObject);
				}
			}
		}
		AudioSource soundSource = effectContext.SoundSource;
		if (soundSource.IsNull())
		{
			return;
		}
		soundSource.volume = effectContext.Volume;
		AudioClip sound = effectContext.Sound;
		if (sound.IsNotNull())
		{
			soundSource.GTPlayOneShot(sound, 1f);
			effectContext.OnPlaySoundFX(soundSource);
		}
	}

	// Token: 0x06003213 RID: 12819 RVA: 0x000F07C4 File Offset: 0x000EE9C4
	public static bool CheckCallSpam(FXSystemSettings settings, int index, double serverTime)
	{
		CallLimitType<CallLimiter> callLimitType = settings.callSettings[index];
		if (!callLimitType.UseNetWorkTime)
		{
			return callLimitType.CallLimitSettings.CheckCallTime(Time.time);
		}
		return callLimitType.CallLimitSettings.CheckCallServerTime(serverTime);
	}
}
