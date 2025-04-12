using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000188 RID: 392
[Serializable]
public struct GTSturdyComponentRef<T> where T : Component
{
	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x060009C4 RID: 2500 RVA: 0x00035E2A File Offset: 0x0003402A
	// (set) Token: 0x060009C5 RID: 2501 RVA: 0x00035E32 File Offset: 0x00034032
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
	// (get) Token: 0x060009C6 RID: 2502 RVA: 0x000943AC File Offset: 0x000925AC
	// (set) Token: 0x060009C7 RID: 2503 RVA: 0x00035E3B File Offset: 0x0003403B
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

	// Token: 0x060009C8 RID: 2504 RVA: 0x00035E74 File Offset: 0x00034074
	public static implicit operator T(GTSturdyComponentRef<T> sturdyRef)
	{
		return sturdyRef.Value;
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x0009441C File Offset: 0x0009261C
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
