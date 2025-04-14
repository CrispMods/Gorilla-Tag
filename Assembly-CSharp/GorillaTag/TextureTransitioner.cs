using System;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B96 RID: 2966
	[ExecuteAlways]
	public class TextureTransitioner : MonoBehaviour, IResettableItem
	{
		// Token: 0x06004AD7 RID: 19159 RVA: 0x0016A397 File Offset: 0x00168597
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

		// Token: 0x06004AD8 RID: 19160 RVA: 0x0016A3CC File Offset: 0x001685CC
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

		// Token: 0x06004AD9 RID: 19161 RVA: 0x0016A4BA File Offset: 0x001686BA
		protected void OnDisable()
		{
			TextureTransitionerManager.Unregister(this);
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x0016A4C2 File Offset: 0x001686C2
		private void RefreshShaderParams()
		{
			this.texTransitionShaderParam = Shader.PropertyToID(this.texTransitionShaderParamName);
			this.tex1ShaderParam = Shader.PropertyToID(this.tex1ShaderParamName);
			this.tex2ShaderParam = Shader.PropertyToID(this.tex2ShaderParamName);
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x0016A4F7 File Offset: 0x001686F7
		public void ResetToDefaultState()
		{
			this.normalizedValue = 0f;
			this.transitionPercent = 0;
			this.tex1Index = 0;
			this.tex2Index = 0;
		}

		// Token: 0x04004C48 RID: 19528
		public bool editorPreview;

		// Token: 0x04004C49 RID: 19529
		[Tooltip("The component that will drive the texture transitions.")]
		public MonoBehaviour dynamicFloatComponent;

		// Token: 0x04004C4A RID: 19530
		[Tooltip("Set these values so that after remap 0 is the first texture in the textures list and 1 is the last.")]
		public GorillaMath.RemapFloatInfo remapInfo;

		// Token: 0x04004C4B RID: 19531
		public TextureTransitioner.DirectionRetentionMode directionRetentionMode;

		// Token: 0x04004C4C RID: 19532
		public string texTransitionShaderParamName = "_TexTransition";

		// Token: 0x04004C4D RID: 19533
		public string tex1ShaderParamName = "_MainTex";

		// Token: 0x04004C4E RID: 19534
		public string tex2ShaderParamName = "_Tex2";

		// Token: 0x04004C4F RID: 19535
		public Texture[] textures;

		// Token: 0x04004C50 RID: 19536
		public Renderer[] renderers;

		// Token: 0x04004C51 RID: 19537
		[NonSerialized]
		public IDynamicFloat iDynamicFloat;

		// Token: 0x04004C52 RID: 19538
		[NonSerialized]
		public int texTransitionShaderParam;

		// Token: 0x04004C53 RID: 19539
		[NonSerialized]
		public int tex1ShaderParam;

		// Token: 0x04004C54 RID: 19540
		[NonSerialized]
		public int tex2ShaderParam;

		// Token: 0x04004C55 RID: 19541
		[DebugReadout]
		[NonSerialized]
		public float normalizedValue;

		// Token: 0x04004C56 RID: 19542
		[DebugReadout]
		[NonSerialized]
		public int transitionPercent;

		// Token: 0x04004C57 RID: 19543
		[DebugReadout]
		[NonSerialized]
		public int tex1Index;

		// Token: 0x04004C58 RID: 19544
		[DebugReadout]
		[NonSerialized]
		public int tex2Index;

		// Token: 0x02000B97 RID: 2967
		public enum DirectionRetentionMode
		{
			// Token: 0x04004C5A RID: 19546
			None,
			// Token: 0x04004C5B RID: 19547
			IncreaseOnly,
			// Token: 0x04004C5C RID: 19548
			DecreaseOnly
		}
	}
}
