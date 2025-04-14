using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000188 RID: 392
[Serializable]
public struct GTSturdyComponentRef<T> where T : Component
{
	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x060009C4 RID: 2500 RVA: 0x00036EE1 File Offset: 0x000350E1
	// (set) Token: 0x060009C5 RID: 2501 RVA: 0x00036EE9 File Offset: 0x000350E9
	public Transform BaseXform
	{
		get
		{
			return this._baseXform;
		}
		set
		{
			this._baseXform = value;
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x060009C6 RID: 2502 RVA: 0x00036EF4 File Offset: 0x000350F4
	// (set) Token: 0x060009C7 RID: 2503 RVA: 0x00036F63 File Offset: 0x00035163
	public T Value
	{
		get
		{
			if (!this._value)
			{
				return this._value;
			}
			if (string.IsNullOrEmpty(this._relativePath))
			{
				return default(T);
			}
			Transform transform;
			if (!this._baseXform.TryFindByPath(this._relativePath, out transform, false))
			{
				return default(T);
			}
			this._value = transform.GetComponent<T>();
			return this._value;
		}
		set
		{
			this._value = value;
			this._relativePath = ((!value) ? this._baseXform.GetRelativePath(value.transform) : string.Empty);
		}
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x00036F9C File Offset: 0x0003519C
	public static implicit operator T(GTSturdyComponentRef<T> sturdyRef)
	{
		return sturdyRef.Value;
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00036FA8 File Offset: 0x000351A8
	public static implicit operator GTSturdyComponentRef<T>(T component)
	{
		return new GTSturdyComponentRef<T>
		{
			Value = component
		};
	}

	// Token: 0x04000BDA RID: 3034
	[SerializeField]
	private T _value;

	// Token: 0x04000BDB RID: 3035
	[SerializeField]
	private string _relativePath;

	// Token: 0x04000BDC RID: 3036
	[SerializeField]
	private Transform _baseXform;
}
