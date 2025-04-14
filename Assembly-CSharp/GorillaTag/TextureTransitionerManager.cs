using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B98 RID: 2968
	[ExecuteAlways]
	public class TextureTransitionerManager : MonoBehaviour
	{
		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06004ADD RID: 19165 RVA: 0x0016A542 File Offset: 0x00168742
		// (set) Token: 0x06004ADE RID: 19166 RVA: 0x0016A549 File Offset: 0x00168749
		public static TextureTransitionerManager instance { get; private set; }

		// Token: 0x06004ADF RID: 19167 RVA: 0x0016A551 File Offset: 0x00168751
		protected void Awake()
		{
			if (TextureTransitionerManager.instance != null)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			TextureTransitionerManager.instance = this;
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(base.gameObject);
			}
			this.matPropBlock = new MaterialPropertyBlock();
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x0016A590 File Offset: 0x00168790
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

		// Token: 0x06004AE1 RID: 19169 RVA: 0x0016A720 File Offset: 0x00168920
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

		// Token: 0x06004AE2 RID: 19170 RVA: 0x0016A74A File Offset: 0x0016894A
		public static void Register(TextureTransitioner component)
		{
			TextureTransitionerManager.components.Add(component);
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x0016A757 File Offset: 0x00168957
		public static void Unregister(TextureTransitioner component)
		{
			TextureTransitionerManager.components.Remove(component);
		}

		// Token: 0x04004C5E RID: 19550
		public static readonly List<TextureTransitioner> components = new List<TextureTransitioner>(256);

		// Token: 0x04004C5F RID: 19551
		private MaterialPropertyBlock matPropBlock;
	}
}
