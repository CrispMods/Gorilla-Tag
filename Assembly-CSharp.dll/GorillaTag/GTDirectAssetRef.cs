using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag
{
	// Token: 0x02000B79 RID: 2937
	[Serializable]
	public struct GTDirectAssetRef<T> : IEquatable<T> where T : UnityEngine.Object
	{
		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06004A62 RID: 19042 RVA: 0x00060198 File Offset: 0x0005E398
		// (set) Token: 0x06004A63 RID: 19043 RVA: 0x000601A0 File Offset: 0x0005E3A0
		public T obj
		{
			get
			{
				return this._obj;
			}
			set
			{
				this._obj = value;
				this.edAssetPath = null;
			}
		}

		// Token: 0x06004A64 RID: 19044 RVA: 0x000601A0 File Offset: 0x0005E3A0
		public GTDirectAssetRef(T theObj)
		{
			this._obj = theObj;
			this.edAssetPath = null;
		}

		// Token: 0x06004A65 RID: 19045 RVA: 0x000601B0 File Offset: 0x0005E3B0
		public static implicit operator T(GTDirectAssetRef<T> refObject)
		{
			return refObject.obj;
		}

		// Token: 0x06004A66 RID: 19046 RVA: 0x0019B560 File Offset: 0x00199760
		public static implicit operator GTDirectAssetRef<T>(T other)
		{
			return new GTDirectAssetRef<T>
			{
				obj = other
			};
		}

		// Token: 0x06004A67 RID: 19047 RVA: 0x000601B9 File Offset: 0x0005E3B9
		public bool Equals(T other)
		{
			return this.obj == other;
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x0019B580 File Offset: 0x00199780
		public override bool Equals(object other)
		{
			T t = other as T;
			return t != null && this.Equals(t);
		}

		// Token: 0x06004A69 RID: 19049 RVA: 0x000601D1 File Offset: 0x0005E3D1
		public override int GetHashCode()
		{
			if (!(this.obj != null))
			{
				return 0;
			}
			return this.obj.GetHashCode();
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x000601F8 File Offset: 0x0005E3F8
		public static bool operator ==(GTDirectAssetRef<T> left, T right)
		{
			return left.Equals(right);
		}

		// Token: 0x06004A6B RID: 19051 RVA: 0x00060202 File Offset: 0x0005E402
		public static bool operator !=(GTDirectAssetRef<T> left, T right)
		{
			return !(left == right);
		}

		// Token: 0x04004BF9 RID: 19449
		[SerializeField]
		[HideInInspector]
		internal T _obj;

		// Token: 0x04004BFA RID: 19450
		[FormerlySerializedAs("assetPath")]
		public string edAssetPath;
	}
}
