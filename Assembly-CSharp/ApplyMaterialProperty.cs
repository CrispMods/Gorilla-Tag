using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200013D RID: 317
public class ApplyMaterialProperty : MonoBehaviour
{
	// Token: 0x06000863 RID: 2147 RVA: 0x00035F02 File Offset: 0x00034102
	private void Start()
	{
		if (this.applyOnStart)
		{
			this.Apply();
		}
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x0008DE38 File Offset: 0x0008C038
	public void Apply()
	{
		if (!this._renderer)
		{
			this._renderer = base.GetComponent<Renderer>();
		}
		ApplyMaterialProperty.ApplyMode applyMode = this.mode;
		if (applyMode == ApplyMaterialProperty.ApplyMode.MaterialInstance)
		{
			this.ApplyMaterialInstance();
			return;
		}
		if (applyMode != ApplyMaterialProperty.ApplyMode.MaterialPropertyBlock)
		{
			return;
		}
		this.ApplyMaterialPropertyBlock();
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00035F12 File Offset: 0x00034112
	public void SetColor(string propertyName, Color color)
	{
		ApplyMaterialProperty.CustomMaterialData orCreateData = this.GetOrCreateData(propertyName);
		orCreateData.dataType = ApplyMaterialProperty.SuportedTypes.Color;
		orCreateData.color = color;
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00035F28 File Offset: 0x00034128
	public void SetFloat(string propertyName, float value)
	{
		ApplyMaterialProperty.CustomMaterialData orCreateData = this.GetOrCreateData(propertyName);
		orCreateData.dataType = ApplyMaterialProperty.SuportedTypes.Float;
		orCreateData.@float = value;
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0008DE7C File Offset: 0x0008C07C
	private ApplyMaterialProperty.CustomMaterialData GetOrCreateData(string propertyName)
	{
		for (int i = 0; i < this.customData.Count; i++)
		{
			ApplyMaterialProperty.CustomMaterialData customMaterialData = this.customData[i];
			if (customMaterialData.name == propertyName)
			{
				return customMaterialData;
			}
		}
		ApplyMaterialProperty.CustomMaterialData customMaterialData2 = new ApplyMaterialProperty.CustomMaterialData
		{
			name = propertyName
		};
		this.customData.Add(customMaterialData2);
		return customMaterialData2;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x0008DED8 File Offset: 0x0008C0D8
	private void ApplyMaterialInstance()
	{
		if (!this._instance)
		{
			this._instance = base.GetComponent<MaterialInstance>();
			if (this._instance == null)
			{
				this._instance = base.gameObject.AddComponent<MaterialInstance>();
			}
		}
		Material material = this.targetMaterial = this._instance.Material;
		for (int i = 0; i < this.customData.Count; i++)
		{
			ApplyMaterialProperty.CustomMaterialData customMaterialData = this.customData[i];
			switch (customMaterialData.dataType)
			{
			case ApplyMaterialProperty.SuportedTypes.Color:
				material.SetColor(customMaterialData.name, customMaterialData.color);
				break;
			case ApplyMaterialProperty.SuportedTypes.Float:
				material.SetFloat(customMaterialData.name, customMaterialData.@float);
				break;
			case ApplyMaterialProperty.SuportedTypes.Vector2:
				material.SetVector(customMaterialData.name, customMaterialData.vector2);
				break;
			case ApplyMaterialProperty.SuportedTypes.Vector3:
				material.SetVector(customMaterialData.name, customMaterialData.vector3);
				break;
			case ApplyMaterialProperty.SuportedTypes.Vector4:
				material.SetVector(customMaterialData.name, customMaterialData.vector4);
				break;
			case ApplyMaterialProperty.SuportedTypes.Texture2D:
				material.SetTexture(customMaterialData.name, customMaterialData.texture2D);
				break;
			}
		}
		this._renderer.SetPropertyBlock(this._block);
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x0008E018 File Offset: 0x0008C218
	private void ApplyMaterialPropertyBlock()
	{
		if (this._block == null)
		{
			this._block = new MaterialPropertyBlock();
		}
		this._renderer.GetPropertyBlock(this._block);
		for (int i = 0; i < this.customData.Count; i++)
		{
			ApplyMaterialProperty.CustomMaterialData customMaterialData = this.customData[i];
			switch (customMaterialData.dataType)
			{
			case ApplyMaterialProperty.SuportedTypes.Color:
				this._block.SetColor(customMaterialData.name, customMaterialData.color);
				break;
			case ApplyMaterialProperty.SuportedTypes.Float:
				this._block.SetFloat(customMaterialData.name, customMaterialData.@float);
				break;
			case ApplyMaterialProperty.SuportedTypes.Vector2:
				this._block.SetVector(customMaterialData.name, customMaterialData.vector2);
				break;
			case ApplyMaterialProperty.SuportedTypes.Vector3:
				this._block.SetVector(customMaterialData.name, customMaterialData.vector3);
				break;
			case ApplyMaterialProperty.SuportedTypes.Vector4:
				this._block.SetVector(customMaterialData.name, customMaterialData.vector4);
				break;
			case ApplyMaterialProperty.SuportedTypes.Texture2D:
				this._block.SetTexture(customMaterialData.name, customMaterialData.texture2D);
				break;
			}
		}
		this._renderer.SetPropertyBlock(this._block);
	}

	// Token: 0x040009BA RID: 2490
	public ApplyMaterialProperty.ApplyMode mode;

	// Token: 0x040009BB RID: 2491
	[FormerlySerializedAs("materialToApplyBlock")]
	public Material targetMaterial;

	// Token: 0x040009BC RID: 2492
	[SerializeField]
	private MaterialInstance _instance;

	// Token: 0x040009BD RID: 2493
	[SerializeField]
	private Renderer _renderer;

	// Token: 0x040009BE RID: 2494
	public List<ApplyMaterialProperty.CustomMaterialData> customData;

	// Token: 0x040009BF RID: 2495
	[SerializeField]
	private bool applyOnStart;

	// Token: 0x040009C0 RID: 2496
	[NonSerialized]
	private MaterialPropertyBlock _block;

	// Token: 0x0200013E RID: 318
	public enum ApplyMode
	{
		// Token: 0x040009C2 RID: 2498
		MaterialInstance,
		// Token: 0x040009C3 RID: 2499
		MaterialPropertyBlock
	}

	// Token: 0x0200013F RID: 319
	public enum SuportedTypes
	{
		// Token: 0x040009C5 RID: 2501
		Color,
		// Token: 0x040009C6 RID: 2502
		Float,
		// Token: 0x040009C7 RID: 2503
		Vector2,
		// Token: 0x040009C8 RID: 2504
		Vector3,
		// Token: 0x040009C9 RID: 2505
		Vector4,
		// Token: 0x040009CA RID: 2506
		Texture2D
	}

	// Token: 0x02000140 RID: 320
	[Serializable]
	public class CustomMaterialData
	{
		// Token: 0x0600086B RID: 2155 RVA: 0x0008E150 File Offset: 0x0008C350
		public override int GetHashCode()
		{
			return new ValueTuple<string, ApplyMaterialProperty.SuportedTypes, Color, float, Vector2, Vector3, Vector4, ValueTuple<Texture2D>>(this.name, this.dataType, this.color, this.@float, this.vector2, this.vector3, this.vector4, new ValueTuple<Texture2D>(this.texture2D)).GetHashCode();
		}

		// Token: 0x040009CB RID: 2507
		public string name;

		// Token: 0x040009CC RID: 2508
		public ApplyMaterialProperty.SuportedTypes dataType;

		// Token: 0x040009CD RID: 2509
		public Color color;

		// Token: 0x040009CE RID: 2510
		public float @float;

		// Token: 0x040009CF RID: 2511
		public Vector2 vector2;

		// Token: 0x040009D0 RID: 2512
		public Vector3 vector3;

		// Token: 0x040009D1 RID: 2513
		public Vector4 vector4;

		// Token: 0x040009D2 RID: 2514
		public Texture2D texture2D;
	}
}
