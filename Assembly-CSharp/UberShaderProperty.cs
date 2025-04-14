using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000650 RID: 1616
[Serializable]
public class UberShaderProperty
{
	// Token: 0x06002814 RID: 10260 RVA: 0x000C4ACC File Offset: 0x000C2CCC
	public T GetValue<T>(Material target)
	{
		switch (this.type)
		{
		case ShaderPropertyType.Color:
			return UberShaderProperty.ValueAs<Color, T>(target.GetColor(this.nameID));
		case ShaderPropertyType.Vector:
			return UberShaderProperty.ValueAs<Vector4, T>(target.GetVector(this.nameID));
		case ShaderPropertyType.Float:
		case ShaderPropertyType.Range:
			return UberShaderProperty.ValueAs<float, T>(target.GetFloat(this.nameID));
		case ShaderPropertyType.Texture:
			return UberShaderProperty.ValueAs<Texture, T>(target.GetTexture(this.nameID));
		case ShaderPropertyType.Int:
			return UberShaderProperty.ValueAs<int, T>(target.GetInt(this.nameID));
		default:
			return default(T);
		}
	}

	// Token: 0x06002815 RID: 10261 RVA: 0x000C4B64 File Offset: 0x000C2D64
	public void SetValue<T>(Material target, T value)
	{
		switch (this.type)
		{
		case ShaderPropertyType.Color:
			target.SetColor(this.nameID, UberShaderProperty.ValueAs<T, Color>(value));
			break;
		case ShaderPropertyType.Vector:
			target.SetVector(this.nameID, UberShaderProperty.ValueAs<T, Vector4>(value));
			break;
		case ShaderPropertyType.Float:
		case ShaderPropertyType.Range:
			target.SetFloat(this.nameID, UberShaderProperty.ValueAs<T, float>(value));
			break;
		case ShaderPropertyType.Texture:
			target.SetTexture(this.nameID, UberShaderProperty.ValueAs<T, Texture>(value));
			break;
		case ShaderPropertyType.Int:
			target.SetInt(this.nameID, UberShaderProperty.ValueAs<T, int>(value));
			break;
		}
		if (!this.isKeywordToggle)
		{
			return;
		}
		bool flag = false;
		ShaderPropertyType shaderPropertyType = this.type;
		if (shaderPropertyType != ShaderPropertyType.Float)
		{
			if (shaderPropertyType == ShaderPropertyType.Int)
			{
				flag = (UberShaderProperty.ValueAs<T, int>(value) >= 1);
			}
		}
		else
		{
			flag = (UberShaderProperty.ValueAs<T, float>(value) >= 0.5f);
		}
		if (flag)
		{
			target.EnableKeyword(this.keyword);
			return;
		}
		target.DisableKeyword(this.keyword);
	}

	// Token: 0x06002816 RID: 10262 RVA: 0x000C4C50 File Offset: 0x000C2E50
	public void Enable(Material target)
	{
		ShaderPropertyType shaderPropertyType = this.type;
		if (shaderPropertyType != ShaderPropertyType.Float)
		{
			if (shaderPropertyType == ShaderPropertyType.Int)
			{
				target.SetInt(this.nameID, 1);
			}
		}
		else
		{
			target.SetFloat(this.nameID, 1f);
		}
		if (this.isKeywordToggle)
		{
			target.EnableKeyword(this.keyword);
		}
	}

	// Token: 0x06002817 RID: 10263 RVA: 0x000C4CA0 File Offset: 0x000C2EA0
	public void Disable(Material target)
	{
		ShaderPropertyType shaderPropertyType = this.type;
		if (shaderPropertyType != ShaderPropertyType.Float)
		{
			if (shaderPropertyType == ShaderPropertyType.Int)
			{
				target.SetInt(this.nameID, 0);
			}
		}
		else
		{
			target.SetFloat(this.nameID, 0f);
		}
		if (this.isKeywordToggle)
		{
			target.DisableKeyword(this.keyword);
		}
	}

	// Token: 0x06002818 RID: 10264 RVA: 0x000C4CF0 File Offset: 0x000C2EF0
	public bool TryGetKeywordState(Material target, out bool enabled)
	{
		enabled = false;
		if (!this.isKeywordToggle)
		{
			return false;
		}
		enabled = target.IsKeywordEnabled(this.keyword);
		return true;
	}

	// Token: 0x06002819 RID: 10265 RVA: 0x000C4D0E File Offset: 0x000C2F0E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static TOut ValueAs<TIn, TOut>(TIn value)
	{
		return *Unsafe.As<TIn, TOut>(ref value);
	}

	// Token: 0x04002C9D RID: 11421
	public int index;

	// Token: 0x04002C9E RID: 11422
	public int nameID;

	// Token: 0x04002C9F RID: 11423
	public string name;

	// Token: 0x04002CA0 RID: 11424
	public ShaderPropertyType type;

	// Token: 0x04002CA1 RID: 11425
	public ShaderPropertyFlags flags;

	// Token: 0x04002CA2 RID: 11426
	public Vector2 rangeLimits;

	// Token: 0x04002CA3 RID: 11427
	public string[] attributes;

	// Token: 0x04002CA4 RID: 11428
	public bool isKeywordToggle;

	// Token: 0x04002CA5 RID: 11429
	public string keyword;
}
