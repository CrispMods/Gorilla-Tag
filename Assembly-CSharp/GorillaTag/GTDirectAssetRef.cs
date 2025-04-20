using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag
{
	// Token: 0x02000BA3 RID: 2979
	[Serializable]
	public struct GTDirectAssetRef<T> : IEquatable<T> where T : UnityEngine.Object
	{
		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06004BA1 RID: 19361 RVA: 0x00061BD0 File Offset: 0x0005FDD0
		// (set) Token: 0x06004BA2 RID: 19362 RVA: 0x00061BD8 File Offset: 0x0005FDD8
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

		// Token: 0x06004BA3 RID: 19363 RVA: 0x00061BD8 File Offset: 0x0005FDD8
		public GTDirectAssetRef(T theObj)
		{
			this._obj = theObj;
			this.edAssetPath = null;
		}

		// Token: 0x06004BA4 RID: 19364 RVA: 0x00061BE8 File Offset: 0x0005FDE8
		public static implicit operator T(GTDirectAssetRef<T> refObject)
		{
			return refObject.obj;
		}

		// Token: 0x06004BA5 RID: 19365 RVA: 0x001A2578 File Offset: 0x001A0778
		public static implicit operator GTDirectAssetRef<T>(T other)
		{
			return new GTDirectAssetRef<T>
			{
				obj = other
			};
		}

		// Token: 0x06004BA6 RID: 19366 RVA: 0x00061BF1 File Offset: 0x0005FDF1
		public bool Equals(T other)
		{
			return this.obj == other;
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x001A2598 File Offset: 0x001A0798
		public override bool Equals(object other)
		{
			T t = other as T;
			return t != null && this.Equals(t);
		}

		// Token: 0x06004BA8 RID: 19368 RVA: 0x00061C09 File Offset: 0x0005FE09
		public override int GetHashCode()
		{
			if (!(this.obj != null))
			{
				return 0;
			}
			return this.obj.GetHashCode();
		}

		// Token: 0x06004BA9 RID: 19369 RVA: 0x00061C30 File Offset: 0x0005FE30
		public static bool operator ==(GTDirectAssetRef<T> left, T right)
		{
			return left.Equals(right);
		}

		// Token: 0x06004BAA RID: 19370 RVA: 0x00061C3A File Offset: 0x0005FE3A
		public static bool operator !=(GTDirectAssetRef<T> left, T right)
		{
			return !(left == right);
		}

		// Token: 0x04004CDD RID: 19677
		[SerializeField]
		[HideInInspector]
		internal T _obj;

		// Token: 0x04004CDE RID: 19678
		[FormerlySerializedAs("assetPath")]
		public string edAssetPath;
	}
}
