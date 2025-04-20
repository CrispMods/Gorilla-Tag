using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200062F RID: 1583
[Serializable]
public class UberShaderProperty
{
	// Token: 0x0600273F RID: 10047 RVA: 0x0010BD10 File Offset: 0x00109F10
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

	// Token: 0x06002740 RID: 10048 RVA: 0x0010BDA8 File Offset: 0x00109FA8
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

	// Token: 0x06002741 RID: 10049 RVA: 0x0010BE94 File Offset: 0x0010A094
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

	// Token: 0x06002742 RID: 10050 RVA: 0x0010BEE4 File Offset: 0x0010A0E4
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

	// Token: 0x06002743 RID: 10051 RVA: 0x0004ACA7 File Offset: 0x00048EA7
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

	// Token: 0x06002744 RID: 10052 RVA: 0x0004ACC5 File Offset: 0x00048EC5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static TOut ValueAs<TIn, TOut>(TIn value)
	{
		return *Unsafe.As<TIn, TOut>(ref value);
	}

	// Token: 0x04002C03 RID: 11267
	public int index;

	// Token: 0x04002C04 RID: 11268
	public int nameID;

	// Token: 0x04002C05 RID: 11269
	public string name;

	// Token: 0x04002C06 RID: 11270
	public ShaderPropertyType type;

	// Token: 0x04002C07 RID: 11271
	public ShaderPropertyFlags flags;

	// Token: 0x04002C08 RID: 11272
	public Vector2 rangeLimits;

	// Token: 0x04002C09 RID: 11273
	public string[] attributes;

	// Token: 0x04002C0A RID: 11274
	public bool isKeywordToggle;

	// Token: 0x04002C0B RID: 11275
	public string keyword;
}
