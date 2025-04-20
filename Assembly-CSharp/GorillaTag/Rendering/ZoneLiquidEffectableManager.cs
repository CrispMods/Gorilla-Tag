using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C39 RID: 3129
	public class ZoneLiquidEffectableManager : MonoBehaviour
	{
		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06004E5A RID: 20058 RVA: 0x000633B5 File Offset: 0x000615B5
		// (set) Token: 0x06004E5B RID: 20059 RVA: 0x000633BC File Offset: 0x000615BC
		public static ZoneLiquidEffectableManager instance { get; private set; }

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06004E5C RID: 20060 RVA: 0x000633C4 File Offset: 0x000615C4
		// (set) Token: 0x06004E5D RID: 20061 RVA: 0x000633CB File Offset: 0x000615CB
		public static bool hasInstance { get; private set; }

		// Token: 0x06004E5E RID: 20062 RVA: 0x000633D3 File Offset: 0x000615D3
		protected void Awake()
		{
			if (ZoneLiquidEffectableManager.hasInstance && ZoneLiquidEffectableManager.instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			ZoneLiquidEffectableManager.SetInstance(this);
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x000633FB File Offset: 0x000615FB
		protected void OnDestroy()
		{
			if (ZoneLiquidEffectableManager.instance == this)
			{
				ZoneLiquidEffectableManager.hasInstance = false;
				ZoneLiquidEffectableManager.instance = null;
			}
		}

		// Token: 0x06004E60 RID: 20064 RVA: 0x001AEE44 File Offset: 0x001AD044
		protected void LateUpdate()
		{
			int layerMask = UnityLayer.Water.ToLayerMask();
			foreach (ZoneLiquidEffectable zoneLiquidEffectable in this.zoneLiquidEffectables)
			{
				Transform transform = zoneLiquidEffectable.transform;
				zoneLiquidEffectable.inLiquidVolume = Physics.CheckSphere(transform.position, zoneLiquidEffectable.radius * transform.lossyScale.x, layerMask);
				if (zoneLiquidEffectable.inLiquidVolume != zoneLiquidEffectable.wasInLiquidVolume)
				{
					for (int i = 0; i < zoneLiquidEffectable.childRenderers.Length; i++)
					{
						if (zoneLiquidEffectable.inLiquidVolume)
						{
							zoneLiquidEffectable.childRenderers[i].material.EnableKeyword("_WATER_EFFECT");
							zoneLiquidEffectable.childRenderers[i].material.EnableKeyword("_HEIGHT_BASED_WATER_EFFECT");
						}
						else
						{
							zoneLiquidEffectable.childRenderers[i].material.DisableKeyword("_WATER_EFFECT");
							zoneLiquidEffectable.childRenderers[i].material.DisableKeyword("_HEIGHT_BASED_WATER_EFFECT");
						}
					}
				}
				zoneLiquidEffectable.wasInLiquidVolume = zoneLiquidEffectable.inLiquidVolume;
			}
		}

		// Token: 0x06004E61 RID: 20065 RVA: 0x00063416 File Offset: 0x00061616
		private static void CreateManager()
		{
			ZoneLiquidEffectableManager.SetInstance(new GameObject("ZoneLiquidEffectableManager").AddComponent<ZoneLiquidEffectableManager>());
		}

		// Token: 0x06004E62 RID: 20066 RVA: 0x0006342C File Offset: 0x0006162C
		private static void SetInstance(ZoneLiquidEffectableManager manager)
		{
			ZoneLiquidEffectableManager.instance = manager;
			ZoneLiquidEffectableManager.hasInstance = true;
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(manager);
			}
		}

		// Token: 0x06004E63 RID: 20067 RVA: 0x001AEF68 File Offset: 0x001AD168
		public static void Register(ZoneLiquidEffectable effect)
		{
			if (!ZoneLiquidEffectableManager.hasInstance)
			{
				ZoneLiquidEffectableManager.CreateManager();
			}
			if (effect == null)
			{
				return;
			}
			if (ZoneLiquidEffectableManager.instance.zoneLiquidEffectables.Contains(effect))
			{
				return;
			}
			ZoneLiquidEffectableManager.instance.zoneLiquidEffectables.Add(effect);
			effect.inLiquidVolume = false;
			for (int i = 0; i < effect.childRenderers.Length; i++)
			{
				if (!(effect.childRenderers[i] == null))
				{
					Material sharedMaterial = effect.childRenderers[i].sharedMaterial;
					if (!(sharedMaterial == null) || sharedMaterial.shader.keywordSpace.FindKeyword("_WATER_EFFECT").isValid)
					{
						effect.inLiquidVolume = (sharedMaterial.IsKeywordEnabled("_WATER_EFFECT") && sharedMaterial.IsKeywordEnabled("_HEIGHT_BASED_WATER_EFFECT"));
						return;
					}
				}
			}
		}

		// Token: 0x06004E64 RID: 20068 RVA: 0x00063447 File Offset: 0x00061647
		public static void Unregister(ZoneLiquidEffectable effect)
		{
			ZoneLiquidEffectableManager.instance.zoneLiquidEffectables.Remove(effect);
		}

		// Token: 0x04005050 RID: 20560
		private readonly List<ZoneLiquidEffectable> zoneLiquidEffectables = new List<ZoneLiquidEffectable>(32);
	}
}
