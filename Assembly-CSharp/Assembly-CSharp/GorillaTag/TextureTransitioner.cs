using System;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B99 RID: 2969
	[ExecuteAlways]
	public class TextureTransitioner : MonoBehaviour, IResettableItem
	{
		// Token: 0x06004AE3 RID: 19171 RVA: 0x0016A95F File Offset: 0x00168B5F
		protected void Awake()
		{
			if (Application.isPlaying || this.editorPreview)
			{
				TextureTransitionerManager.EnsureInstanceIsAvailable();
			}
			this.RefreshShaderParams();
			this.iDynamicFloat = (IDynamicFloat)this.dynamicFloatComponent;
			this.ResetToDefaultState();
		}

		// Token: 0x06004AE4 RID: 19172 RVA: 0x0016A994 File Offset: 0x00168B94
		protected void OnEnable()
		{
			TextureTransitionerManager.Register(this);
			if (Application.isPlaying && !this.remapInfo.IsValid())
			{
				Debug.LogError("Bad min/max values for remapRanges: " + this.GetComponentPath(int.MaxValue), this);
				base.enabled = false;
			}
			if (Application.isPlaying && this.textures.Length == 0)
			{
				Debug.LogError("Textures array is empty: " + this.GetComponentPath(int.MaxValue), this);
				base.enabled = false;
			}
			if (Application.isPlaying && this.iDynamicFloat == null)
			{
				if (this.dynamicFloatComponent == null)
				{
					Debug.LogError("dynamicFloatComponent cannot be null: " + this.GetComponentPath(int.MaxValue), this);
				}
				this.iDynamicFloat = (IDynamicFloat)this.dynamicFloatComponent;
				if (this.iDynamicFloat == null)
				{
					Debug.LogError("Component assigned to dynamicFloatComponent does not implement IDynamicFloat: " + this.GetComponentPath(int.MaxValue), this);
					base.enabled = false;
				}
			}
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x0016AA82 File Offset: 0x00168C82
		protected void OnDisable()
		{
			TextureTransitionerManager.Unregister(this);
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x0016AA8A File Offset: 0x00168C8A
		private void RefreshShaderParams()
		{
			this.texTransitionShaderParam = Shader.PropertyToID(this.texTransitionShaderParamName);
			this.tex1ShaderParam = Shader.PropertyToID(this.tex1ShaderParamName);
			this.tex2ShaderParam = Shader.PropertyToID(this.tex2ShaderParamName);
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x0016AABF File Offset: 0x00168CBF
		public void ResetToDefaultState()
		{
			this.normalizedValue = 0f;
			this.transitionPercent = 0;
			this.tex1Index = 0;
			this.tex2Index = 0;
		}

		// Token: 0x04004C5A RID: 19546
		public bool editorPreview;

		// Token: 0x04004C5B RID: 19547
		[Tooltip("The component that will drive the texture transitions.")]
		public MonoBehaviour dynamicFloatComponent;

		// Token: 0x04004C5C RID: 19548
		[Tooltip("Set these values so that after remap 0 is the first texture in the textures list and 1 is the last.")]
		public GorillaMath.RemapFloatInfo remapInfo;

		// Token: 0x04004C5D RID: 19549
		public TextureTransitioner.DirectionRetentionMode directionRetentionMode;

		// Token: 0x04004C5E RID: 19550
		public string texTransitionShaderParamName = "_TexTransition";

		// Token: 0x04004C5F RID: 19551
		public string tex1ShaderParamName = "_MainTex";

		// Token: 0x04004C60 RID: 19552
		public string tex2ShaderParamName = "_Tex2";

		// Token: 0x04004C61 RID: 19553
		public Texture[] textures;

		// Token: 0x04004C62 RID: 19554
		public Renderer[] renderers;

		// Token: 0x04004C63 RID: 19555
		[NonSerialized]
		public IDynamicFloat iDynamicFloat;

		// Token: 0x04004C64 RID: 19556
		[NonSerialized]
		public int texTransitionShaderParam;

		// Token: 0x04004C65 RID: 19557
		[NonSerialized]
		public int tex1ShaderParam;

		// Token: 0x04004C66 RID: 19558
		[NonSerialized]
		public int tex2ShaderParam;

		// Token: 0x04004C67 RID: 19559
		[DebugReadout]
		[NonSerialized]
		public float normalizedValue;

		// Token: 0x04004C68 RID: 19560
		[DebugReadout]
		[NonSerialized]
		public int transitionPercent;

		// Token: 0x04004C69 RID: 19561
		[DebugReadout]
		[NonSerialized]
		public int tex1Index;

		// Token: 0x04004C6A RID: 19562
		[DebugReadout]
		[NonSerialized]
		public int tex2Index;

		// Token: 0x02000B9A RID: 2970
		public enum DirectionRetentionMode
		{
			// Token: 0x04004C6C RID: 19564
			None,
			// Token: 0x04004C6D RID: 19565
			IncreaseOnly,
			// Token: 0x04004C6E RID: 19566
			DecreaseOnly
		}
	}
}
