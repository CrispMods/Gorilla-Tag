using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000133 RID: 307
public class ApplyMaterialProperty : MonoBehaviour
{
	// Token: 0x06000821 RID: 2081 RVA: 0x00034C8C File Offset: 0x00032E8C
	private void Start()
	{
		if (this.applyOnStart)
		{
			this.Apply();
		}
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x0008B4B0 File Offset: 0x000896B0
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

	// Token: 0x06000823 RID: 2083 RVA: 0x00034C9C File Offset: 0x00032E9C
	public void SetColor(string propertyName, Color color)
	{
		ApplyMaterialProperty.CustomMaterialData orCreateData = this.GetOrCreateData(propertyName);
		orCreateData.dataType = ApplyMaterialProperty.SuportedTypes.Color;
		orCreateData.color = color;
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x00034CB2 File Offset: 0x00032EB2
	public void SetFloat(string propertyName, float value)
	{
		ApplyMaterialProperty.CustomMaterialData orCreateData = this.GetOrCreateData(propertyName);
		orCreateData.dataType = ApplyMaterialProperty.SuportedTypes.Float;
		orCreateData.@float = value;
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x0008B4F4 File Offset: 0x000896F4
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

	// Token: 0x06000826 RID: 2086 RVA: 0x0008B550 File Offset: 0x00089750
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

	// Token: 0x06000827 RID: 2087 RVA: 0x0008B690 File Offset: 0x00089890
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

	// Token: 0x04000978 RID: 2424
	public ApplyMaterialProperty.ApplyMode mode;

	// Token: 0x04000979 RID: 2425
	[FormerlySerializedAs("materialToApplyBlock")]
	public Material targetMaterial;

	// Token: 0x0400097A RID: 2426
	[SerializeField]
	private MaterialInstance _instance;

	// Token: 0x0400097B RID: 2427
	[SerializeField]
	private Renderer _renderer;

	// Token: 0x0400097C RID: 2428
	public List<ApplyMaterialProperty.CustomMaterialData> customData;

	// Token: 0x0400097D RID: 2429
	[SerializeField]
	private bool applyOnStart;

	// Token: 0x0400097E RID: 2430
	[NonSerialized]
	private MaterialPropertyBlock _block;

	// Token: 0x02000134 RID: 308
	public enum ApplyMode
	{
		// Token: 0x04000980 RID: 2432
		MaterialInstance,
		// Token: 0x04000981 RID: 2433
		MaterialPropertyBlock
	}

	// Token: 0x02000135 RID: 309
	public enum SuportedTypes
	{
		// Token: 0x04000983 RID: 2435
		Color,
		// Token: 0x04000984 RID: 2436
		Float,
		// Token: 0x04000985 RID: 2437
		Vector2,
		// Token: 0x04000986 RID: 2438
		Vector3,
		// Token: 0x04000987 RID: 2439
		Vector4,
		// Token: 0x04000988 RID: 2440
		Texture2D
	}

	// Token: 0x02000136 RID: 310
	[Serializable]
	public class CustomMaterialData
	{
		// Token: 0x06000829 RID: 2089 RVA: 0x0008B7C8 File Offset: 0x000899C8
		public override int GetHashCode()
		{
			return new ValueTuple<string, ApplyMaterialProperty.SuportedTypes, Color, float, Vector2, Vector3, Vector4, ValueTuple<Texture2D>>(this.name, this.dataType, this.color, this.@float, this.vector2, this.vector3, this.vector4, new ValueTuple<Texture2D>(this.texture2D)).GetHashCode();
		}

		// Token: 0x04000989 RID: 2441
		public string name;

		// Token: 0x0400098A RID: 2442
		public ApplyMaterialProperty.SuportedTypes dataType;

		// Token: 0x0400098B RID: 2443
		public Color color;

		// Token: 0x0400098C RID: 2444
		public float @float;

		// Token: 0x0400098D RID: 2445
		public Vector2 vector2;

		// Token: 0x0400098E RID: 2446
		public Vector3 vector3;

		// Token: 0x0400098F RID: 2447
		public Vector4 vector4;

		// Token: 0x04000990 RID: 2448
		public Texture2D texture2D;
	}
}
