using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BC5 RID: 3013
	[ExecuteAlways]
	public class TextureTransitionerManager : MonoBehaviour
	{
		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06004C28 RID: 19496 RVA: 0x000621CB File Offset: 0x000603CB
		// (set) Token: 0x06004C29 RID: 19497 RVA: 0x000621D2 File Offset: 0x000603D2
		public static TextureTransitionerManager instance { get; private set; }

		// Token: 0x06004C2A RID: 19498 RVA: 0x000621DA File Offset: 0x000603DA
		protected void Awake()
		{
			if (TextureTransitionerManager.instance != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			TextureTransitionerManager.instance = this;
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			this.matPropBlock = new MaterialPropertyBlock();
		}

		// Token: 0x06004C2B RID: 19499 RVA: 0x001A3B88 File Offset: 0x001A1D88
		protected void LateUpdate()
		{
			foreach (TextureTransitioner textureTransitioner in TextureTransitionerManager.components)
			{
				int num = textureTransitioner.textures.Length;
				float num2 = Mathf.Clamp01(textureTransitioner.remapInfo.Remap(textureTransitioner.iDynamicFloat.floatValue));
				TextureTransitioner.DirectionRetentionMode directionRetentionMode = textureTransitioner.directionRetentionMode;
				if (directionRetentionMode != TextureTransitioner.DirectionRetentionMode.IncreaseOnly)
				{
					if (directionRetentionMode == TextureTransitioner.DirectionRetentionMode.DecreaseOnly)
					{
						num2 = Mathf.Min(num2, textureTransitioner.normalizedValue);
					}
				}
				else
				{
					num2 = Mathf.Max(num2, textureTransitioner.normalizedValue);
				}
				float num3 = num2 * (float)(num - 1);
				float num4 = num3 % 1f;
				int num5 = (int)(num4 * 1000f);
				int num6 = (int)num3;
				int num7 = Mathf.Min(num - 1, num6 + 1);
				if (num5 != textureTransitioner.transitionPercent || num6 != textureTransitioner.tex1Index || num7 != textureTransitioner.tex2Index)
				{
					this.matPropBlock.SetFloat(textureTransitioner.texTransitionShaderParam, num4);
					this.matPropBlock.SetTexture(textureTransitioner.tex1ShaderParam, textureTransitioner.textures[num6]);
					this.matPropBlock.SetTexture(textureTransitioner.tex2ShaderParam, textureTransitioner.textures[num7]);
					Renderer[] renderers = textureTransitioner.renderers;
					for (int i = 0; i < renderers.Length; i++)
					{
						renderers[i].SetPropertyBlock(this.matPropBlock);
					}
					textureTransitioner.normalizedValue = num2;
					textureTransitioner.transitionPercent = num5;
					textureTransitioner.tex1Index = num6;
					textureTransitioner.tex2Index = num7;
				}
			}
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x00062218 File Offset: 0x00060418
		public static void EnsureInstanceIsAvailable()
		{
			if (TextureTransitionerManager.instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject();
			TextureTransitionerManager.instance = gameObject.AddComponent<TextureTransitionerManager>();
			gameObject.name = "TextureTransitionerManager (Singleton)";
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x00062242 File Offset: 0x00060442
		public static void Register(TextureTransitioner component)
		{
			TextureTransitionerManager.components.Add(component);
		}

		// Token: 0x06004C2E RID: 19502 RVA: 0x0006224F File Offset: 0x0006044F
		public static void Unregister(TextureTransitioner component)
		{
			TextureTransitionerManager.components.Remove(component);
		}

		// Token: 0x04004D54 RID: 19796
		public static readonly List<TextureTransitioner> components = new List<TextureTransitioner>(256);

		// Token: 0x04004D55 RID: 19797
		private MaterialPropertyBlock matPropBlock;
	}
}
