using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag
{
	// Token: 0x02000B79 RID: 2937
	[Serializable]
	public struct GTDirectAssetRef<T> : IEquatable<T> where T : Object
	{
		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06004A62 RID: 19042 RVA: 0x00168E9B File Offset: 0x0016709B
		// (set) Token: 0x06004A63 RID: 19043 RVA: 0x00168EA3 File Offset: 0x001670A3
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

		// Token: 0x06004A64 RID: 19044 RVA: 0x00168EA3 File Offset: 0x001670A3
		public GTDirectAssetRef(T theObj)
		{
			this._obj = theObj;
			this.edAssetPath = null;
		}

		// Token: 0x06004A65 RID: 19045 RVA: 0x00168EB3 File Offset: 0x001670B3
		public static implicit operator T(GTDirectAssetRef<T> refObject)
		{
			return refObject.obj;
		}

		// Token: 0x06004A66 RID: 19046 RVA: 0x00168EBC File Offset: 0x001670BC
		public static implicit operator GTDirectAssetRef<T>(T other)
		{
			return new GTDirectAssetRef<T>
			{
				obj = other
			};
		}

		// Token: 0x06004A67 RID: 19047 RVA: 0x00168EDA File Offset: 0x001670DA
		public bool Equals(T other)
		{
			return this.obj == other;
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x00168EF4 File Offset: 0x001670F4
		public override bool Equals(object other)
		{
			T t = other as T;
			return t != null && this.Equals(t);
		}

		// Token: 0x06004A69 RID: 19049 RVA: 0x00168F1E File Offset: 0x0016711E
		public override int GetHashCode()
		{
			if (!(this.obj != null))
			{
				return 0;
			}
			return this.obj.GetHashCode();
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x00168F45 File Offset: 0x00167145
		public static bool operator ==(GTDirectAssetRef<T> left, T right)
		{
			return left.Equals(right);
		}

		// Token: 0x06004A6B RID: 19051 RVA: 0x00168F4F File Offset: 0x0016714F
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
