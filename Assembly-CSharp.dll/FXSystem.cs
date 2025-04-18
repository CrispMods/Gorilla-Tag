﻿using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x020007EC RID: 2028
public static class FXSystem
{
	// Token: 0x0600321A RID: 12826 RVA: 0x00133748 File Offset: 0x00131948
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

	// Token: 0x0600321B RID: 12827 RVA: 0x00133784 File Offset: 0x00131984
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

	// Token: 0x0600321C RID: 12828 RVA: 0x001337C0 File Offset: 0x001319C0
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

	// Token: 0x0600321D RID: 12829 RVA: 0x001337FC File Offset: 0x001319FC
	public static void PlayFXForRig<T>(FXType fxType, IFXEffectContext<T> context, PhotonMessageInfoWrapped info) where T : IFXEffectContextObject
	{
		FXSystemSettings settings = context.settings;
		if (!settings.forLocalRig && !FXSystem.CheckCallSpam(settings, (int)fxType, info.SentServerTime))
		{
			return;
		}
		FXSystem.PlayFX(context.effectContext);
	}

	// Token: 0x0600321E RID: 12830 RVA: 0x0013383C File Offset: 0x00131A3C
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

	// Token: 0x0600321F RID: 12831 RVA: 0x001338EC File Offset: 0x00131AEC
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
