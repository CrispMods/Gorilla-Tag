using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag
{
	// Token: 0x02000B76 RID: 2934
	[Serializable]
	public struct GTDirectAssetRef<T> : IEquatable<T> where T : Object
	{
		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06004A56 RID: 19030 RVA: 0x001688D3 File Offset: 0x00166AD3
		// (set) Token: 0x06004A57 RID: 19031 RVA: 0x001688DB File Offset: 0x00166ADB
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

		// Token: 0x06004A58 RID: 19032 RVA: 0x001688DB File Offset: 0x00166ADB
		public GTDirectAssetRef(T theObj)
		{
			this._obj = theObj;
			this.edAssetPath = null;
		}

		// Token: 0x06004A59 RID: 19033 RVA: 0x001688EB File Offset: 0x00166AEB
		public static implicit operator T(GTDirectAssetRef<T> refObject)
		{
			return refObject.obj;
		}

		// Token: 0x06004A5A RID: 19034 RVA: 0x001688F4 File Offset: 0x00166AF4
		public static implicit operator GTDirectAssetRef<T>(T other)
		{
			return new GTDirectAssetRef<T>
			{
				obj = other
			};
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x00168912 File Offset: 0x00166B12
		public bool Equals(T other)
		{
			return this.obj == other;
		}

		// Token: 0x06004A5C RID: 19036 RVA: 0x0016892C File Offset: 0x00166B2C
		public override bool Equals(object other)
		{
			T t = other as T;
			return t != null && this.Equals(t);
		}

		// Token: 0x06004A5D RID: 19037 RVA: 0x00168956 File Offset: 0x00166B56
		public override int GetHashCode()
		{
			if (!(this.obj != null))
			{
				return 0;
			}
			return this.obj.GetHashCode();
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x0016897D File Offset: 0x00166B7D
		public static bool operator ==(GTDirectAssetRef<T> left, T right)
		{
			return left.Equals(right);
		}

		// Token: 0x06004A5F RID: 19039 RVA: 0x00168987 File Offset: 0x00166B87
		public static bool operator !=(GTDirectAssetRef<T> left, T right)
		{
			return !(left == right);
		}

		// Token: 0x04004BE7 RID: 19431
		[SerializeField]
		[HideInInspector]
		internal T _obj;

		// Token: 0x04004BE8 RID: 19432
		[FormerlySerializedAs("assetPath")]
		public string edAssetPath;
	}
}
