using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000193 RID: 403
[Serializable]
public struct GTSturdyComponentRef<T> where T : Component
{
	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06000A0E RID: 2574 RVA: 0x000370EA File Offset: 0x000352EA
	// (set) Token: 0x06000A0F RID: 2575 RVA: 0x000370F2 File Offset: 0x000352F2
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

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000A10 RID: 2576 RVA: 0x00096CA0 File Offset: 0x00094EA0
	// (set) Token: 0x06000A11 RID: 2577 RVA: 0x000370FB File Offset: 0x000352FB
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

	// Token: 0x06000A12 RID: 2578 RVA: 0x00037134 File Offset: 0x00035334
	public static implicit operator T(GTSturdyComponentRef<T> sturdyRef)
	{
		return sturdyRef.Value;
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x00096D10 File Offset: 0x00094F10
	public static implicit operator GTSturdyComponentRef<T>(T component)
	{
		return new GTSturdyComponentRef<T>
		{
			Value = component
		};
	}

	// Token: 0x04000C1F RID: 3103
	[SerializeField]
	private T _value;

	// Token: 0x04000C20 RID: 3104
	[SerializeField]
	private string _relativePath;

	// Token: 0x04000C21 RID: 3105
	[SerializeField]
	private Transform _baseXform;
}
