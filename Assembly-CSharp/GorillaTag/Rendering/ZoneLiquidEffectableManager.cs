using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C0B RID: 3083
	public class ZoneLiquidEffectableManager : MonoBehaviour
	{
		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06004D0E RID: 19726 RVA: 0x00176B19 File Offset: 0x00174D19
		// (set) Token: 0x06004D0F RID: 19727 RVA: 0x00176B20 File Offset: 0x00174D20
		public static ZoneLiquidEffectableManager instance { get; private set; }

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06004D10 RID: 19728 RVA: 0x00176B28 File Offset: 0x00174D28
		// (set) Token: 0x06004D11 RID: 19729 RVA: 0x00176B2F File Offset: 0x00174D2F
		public static bool hasInstance { get; private set; }

		// Token: 0x06004D12 RID: 19730 RVA: 0x00176B37 File Offset: 0x00174D37
		protected void Awake()
		{
			if (ZoneLiquidEffectableManager.hasInstance && ZoneLiquidEffectableManager.instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			ZoneLiquidEffectableManager.SetInstance(this);
		}

		// Token: 0x06004D13 RID: 19731 RVA: 0x00176B5F File Offset: 0x00174D5F
		protected void OnDestroy()
		{
			if (ZoneLiquidEffectableManager.instance == this)
			{
				ZoneLiquidEffectableManager.hasInstance = false;
				ZoneLiquidEffectableManager.instance = null;
			}
		}

		// Token: 0x06004D14 RID: 19732 RVA: 0x00176B7C File Offset: 0x00174D7C
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

		// Token: 0x06004D15 RID: 19733 RVA: 0x00176CA0 File Offset: 0x00174EA0
		private static void CreateManager()
		{
			ZoneLiquidEffectableManager.SetInstance(new GameObject("ZoneLiquidEffectableManager").AddComponent<ZoneLiquidEffectableManager>());
		}

		// Token: 0x06004D16 RID: 19734 RVA: 0x00176CB6 File Offset: 0x00174EB6
		private static void SetInstance(ZoneLiquidEffectableManager manager)
		{
			ZoneLiquidEffectableManager.instance = manager;
			ZoneLiquidEffectableManager.hasInstance = true;
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(manager);
			}
		}

		// Token: 0x06004D17 RID: 19735 RVA: 0x00176CD4 File Offset: 0x00174ED4
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

		// Token: 0x06004D18 RID: 19736 RVA: 0x00176D9F File Offset: 0x00174F9F
		public static void Unregister(ZoneLiquidEffectable effect)
		{
			ZoneLiquidEffectableManager.instance.zoneLiquidEffectables.Remove(effect);
		}

		// Token: 0x04004F5A RID: 20314
		private readonly List<ZoneLiquidEffectable> zoneLiquidEffectables = new List<ZoneLiquidEffectable>(32);
	}
}
