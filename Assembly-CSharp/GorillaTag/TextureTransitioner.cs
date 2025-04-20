using System;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BC3 RID: 3011
	[ExecuteAlways]
	public class TextureTransitioner : MonoBehaviour, IResettableItem
	{
		// Token: 0x06004C22 RID: 19490 RVA: 0x00062110 File Offset: 0x00060310
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

		// Token: 0x06004C23 RID: 19491 RVA: 0x001A3A98 File Offset: 0x001A1C98
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

		// Token: 0x06004C24 RID: 19492 RVA: 0x00062143 File Offset: 0x00060343
		protected void OnDisable()
		{
			TextureTransitionerManager.Unregister(this);
		}

		// Token: 0x06004C25 RID: 19493 RVA: 0x0006214B File Offset: 0x0006034B
		private void RefreshShaderParams()
		{
			this.texTransitionShaderParam = Shader.PropertyToID(this.texTransitionShaderParamName);
			this.tex1ShaderParam = Shader.PropertyToID(this.tex1ShaderParamName);
			this.tex2ShaderParam = Shader.PropertyToID(this.tex2ShaderParamName);
		}

		// Token: 0x06004C26 RID: 19494 RVA: 0x00062180 File Offset: 0x00060380
		public void ResetToDefaultState()
		{
			this.normalizedValue = 0f;
			this.transitionPercent = 0;
			this.tex1Index = 0;
			this.tex2Index = 0;
		}

		// Token: 0x04004D3E RID: 19774
		public bool editorPreview;

		// Token: 0x04004D3F RID: 19775
		[Tooltip("The component that will drive the texture transitions.")]
		public MonoBehaviour dynamicFloatComponent;

		// Token: 0x04004D40 RID: 19776
		[Tooltip("Set these values so that after remap 0 is the first texture in the textures list and 1 is the last.")]
		public GorillaMath.RemapFloatInfo remapInfo;

		// Token: 0x04004D41 RID: 19777
		public TextureTransitioner.DirectionRetentionMode directionRetentionMode;

		// Token: 0x04004D42 RID: 19778
		public string texTransitionShaderParamName = "_TexTransition";

		// Token: 0x04004D43 RID: 19779
		public string tex1ShaderParamName = "_MainTex";

		// Token: 0x04004D44 RID: 19780
		public string tex2ShaderParamName = "_Tex2";

		// Token: 0x04004D45 RID: 19781
		public Texture[] textures;

		// Token: 0x04004D46 RID: 19782
		public Renderer[] renderers;

		// Token: 0x04004D47 RID: 19783
		[NonSerialized]
		public IDynamicFloat iDynamicFloat;

		// Token: 0x04004D48 RID: 19784
		[NonSerialized]
		public int texTransitionShaderParam;

		// Token: 0x04004D49 RID: 19785
		[NonSerialized]
		public int tex1ShaderParam;

		// Token: 0x04004D4A RID: 19786
		[NonSerialized]
		public int tex2ShaderParam;

		// Token: 0x04004D4B RID: 19787
		[DebugReadout]
		[NonSerialized]
		public float normalizedValue;

		// Token: 0x04004D4C RID: 19788
		[DebugReadout]
		[NonSerialized]
		public int transitionPercent;

		// Token: 0x04004D4D RID: 19789
		[DebugReadout]
		[NonSerialized]
		public int tex1Index;

		// Token: 0x04004D4E RID: 19790
		[DebugReadout]
		[NonSerialized]
		public int tex2Index;

		// Token: 0x02000BC4 RID: 3012
		public enum DirectionRetentionMode
		{
			// Token: 0x04004D50 RID: 19792
			None,
			// Token: 0x04004D51 RID: 19793
			IncreaseOnly,
			// Token: 0x04004D52 RID: 19794
			DecreaseOnly
		}
	}
}
