using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag
{
	// Token: 0x02000B82 RID: 2946
	[Serializable]
	public struct XformOffset
	{
		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06004A94 RID: 19092 RVA: 0x0006036D File Offset: 0x0005E56D
		// (set) Token: 0x06004A95 RID: 19093 RVA: 0x00060375 File Offset: 0x0005E575
		[Tooltip("The rotation of the cosmetic relative to the parent bone.")]
		public Quaternion rot
		{
			get
			{
				return this._rotQuat;
			}
			set
			{
				this._rotQuat = value;
			}
		}

		// Token: 0x06004A96 RID: 19094 RVA: 0x0006037E File Offset: 0x0005E57E
		public XformOffset(int thisIsAnUnusedDummyValue)
		{
			this.pos = Vector3.zero;
			this._rotQuat = Quaternion.identity;
			this._rotEulerAngles = Vector3.zero;
			this.scale = Vector3.one;
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x000603AC File Offset: 0x0005E5AC
		public XformOffset(Vector3 pos, Quaternion rot, Vector3 scale)
		{
			this.pos = pos;
			this._rotQuat = rot;
			this._rotEulerAngles = rot.eulerAngles;
			this.scale = scale;
		}

		// Token: 0x06004A98 RID: 19096 RVA: 0x000603D0 File Offset: 0x0005E5D0
		public XformOffset(Vector3 pos, Vector3 rot, Vector3 scale)
		{
			this.pos = pos;
			this._rotQuat = Quaternion.Euler(rot);
			this._rotEulerAngles = rot;
			this.scale = scale;
		}

		// Token: 0x06004A99 RID: 19097 RVA: 0x000603F3 File Offset: 0x0005E5F3
		public XformOffset(Vector3 pos, Quaternion rot)
		{
			this.pos = pos;
			this._rotQuat = rot;
			this._rotEulerAngles = rot.eulerAngles;
			this.scale = Vector3.one;
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x0006041B File Offset: 0x0005E61B
		public XformOffset(Vector3 pos, Vector3 rot)
		{
			this.pos = pos;
			this._rotQuat = Quaternion.Euler(rot);
			this._rotEulerAngles = rot;
			this.scale = Vector3.one;
		}

		// Token: 0x06004A9B RID: 19099 RVA: 0x0019BE70 File Offset: 0x0019A070
		public XformOffset(Transform boneXform, Transform cosmeticXform)
		{
			this.pos = boneXform.InverseTransformPoint(cosmeticXform.position);
			this._rotQuat = Quaternion.Inverse(boneXform.rotation) * cosmeticXform.rotation;
			this._rotEulerAngles = this._rotQuat.eulerAngles;
			Vector3 lossyScale = boneXform.lossyScale;
			Vector3 lossyScale2 = cosmeticXform.lossyScale;
			this.scale = new Vector3(lossyScale2.x / lossyScale.x, lossyScale2.y / lossyScale.y, lossyScale2.z / lossyScale.z);
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x0019BEFC File Offset: 0x0019A0FC
		public XformOffset(Matrix4x4 matrix)
		{
			this.pos = matrix.GetPosition();
			this.scale = matrix.lossyScale;
			if (Vector3.Dot(Vector3.Cross(matrix.GetColumn(0), matrix.GetColumn(1)), matrix.GetColumn(2)) < 0f)
			{
				this.scale = -this.scale;
			}
			Matrix4x4 matrix4x = matrix;
			matrix4x.SetColumn(0, matrix4x.GetColumn(0) / this.scale.x);
			matrix4x.SetColumn(1, matrix4x.GetColumn(1) / this.scale.y);
			matrix4x.SetColumn(2, matrix4x.GetColumn(2) / this.scale.z);
			this._rotQuat = Quaternion.LookRotation(matrix4x.GetColumn(2), matrix4x.GetColumn(1));
			this._rotEulerAngles = this._rotQuat.eulerAngles;
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x0019C004 File Offset: 0x0019A204
		public bool Approx(XformOffset other)
		{
			return this.pos.Approx(other.pos, 1E-05f) && this._rotQuat.Approx(other._rotQuat, 1E-06f) && this.scale.Approx(other.scale, 1E-05f);
		}

		// Token: 0x04004C2F RID: 19503
		[Tooltip("The position of the cosmetic relative to the parent bone.")]
		public Vector3 pos;

		// Token: 0x04004C30 RID: 19504
		[FormerlySerializedAs("_edRotQuat")]
		[FormerlySerializedAs("rot")]
		[HideInInspector]
		[SerializeField]
		private Quaternion _rotQuat;

		// Token: 0x04004C31 RID: 19505
		[FormerlySerializedAs("_edRotEulerAngles")]
		[FormerlySerializedAs("_edRotEuler")]
		[HideInInspector]
		[SerializeField]
		private Vector3 _rotEulerAngles;

		// Token: 0x04004C32 RID: 19506
		[Tooltip("The scale of the cosmetic relative to the parent bone.")]
		public Vector3 scale;

		// Token: 0x04004C33 RID: 19507
		public static readonly XformOffset Identity = new XformOffset
		{
			_rotQuat = Quaternion.identity,
			scale = Vector3.one
		};
	}
}
