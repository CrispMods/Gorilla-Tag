using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag
{
	// Token: 0x02000B7F RID: 2943
	[Serializable]
	public struct XformOffset
	{
		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06004A88 RID: 19080 RVA: 0x001693B9 File Offset: 0x001675B9
		// (set) Token: 0x06004A89 RID: 19081 RVA: 0x001693C1 File Offset: 0x001675C1
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

		// Token: 0x06004A8A RID: 19082 RVA: 0x001693CA File Offset: 0x001675CA
		public XformOffset(int thisIsAnUnusedDummyValue)
		{
			this.pos = Vector3.zero;
			this._rotQuat = Quaternion.identity;
			this._rotEulerAngles = Vector3.zero;
			this.scale = Vector3.one;
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x001693F8 File Offset: 0x001675F8
		public XformOffset(Vector3 pos, Quaternion rot, Vector3 scale)
		{
			this.pos = pos;
			this._rotQuat = rot;
			this._rotEulerAngles = rot.eulerAngles;
			this.scale = scale;
		}

		// Token: 0x06004A8C RID: 19084 RVA: 0x0016941C File Offset: 0x0016761C
		public XformOffset(Vector3 pos, Vector3 rot, Vector3 scale)
		{
			this.pos = pos;
			this._rotQuat = Quaternion.Euler(rot);
			this._rotEulerAngles = rot;
			this.scale = scale;
		}

		// Token: 0x06004A8D RID: 19085 RVA: 0x0016943F File Offset: 0x0016763F
		public XformOffset(Vector3 pos, Quaternion rot)
		{
			this.pos = pos;
			this._rotQuat = rot;
			this._rotEulerAngles = rot.eulerAngles;
			this.scale = Vector3.one;
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x00169467 File Offset: 0x00167667
		public XformOffset(Vector3 pos, Vector3 rot)
		{
			this.pos = pos;
			this._rotQuat = Quaternion.Euler(rot);
			this._rotEulerAngles = rot;
			this.scale = Vector3.one;
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x00169490 File Offset: 0x00167690
		public XformOffset(Transform boneXform, Transform cosmeticXform)
		{
			this.pos = boneXform.InverseTransformPoint(cosmeticXform.position);
			this._rotQuat = Quaternion.Inverse(boneXform.rotation) * cosmeticXform.rotation;
			this._rotEulerAngles = this._rotQuat.eulerAngles;
			Vector3 lossyScale = boneXform.lossyScale;
			Vector3 lossyScale2 = cosmeticXform.lossyScale;
			this.scale = new Vector3(lossyScale2.x / lossyScale.x, lossyScale2.y / lossyScale.y, lossyScale2.z / lossyScale.z);
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x0016951C File Offset: 0x0016771C
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

		// Token: 0x06004A91 RID: 19089 RVA: 0x00169624 File Offset: 0x00167824
		public bool Approx(XformOffset other)
		{
			return this.pos.Approx(other.pos, 1E-05f) && this._rotQuat.Approx(other._rotQuat, 1E-06f) && this.scale.Approx(other.scale, 1E-05f);
		}

		// Token: 0x04004C1D RID: 19485
		[Tooltip("The position of the cosmetic relative to the parent bone.")]
		public Vector3 pos;

		// Token: 0x04004C1E RID: 19486
		[FormerlySerializedAs("_edRotQuat")]
		[FormerlySerializedAs("rot")]
		[HideInInspector]
		[SerializeField]
		private Quaternion _rotQuat;

		// Token: 0x04004C1F RID: 19487
		[FormerlySerializedAs("_edRotEulerAngles")]
		[FormerlySerializedAs("_edRotEuler")]
		[HideInInspector]
		[SerializeField]
		private Vector3 _rotEulerAngles;

		// Token: 0x04004C20 RID: 19488
		[Tooltip("The scale of the cosmetic relative to the parent bone.")]
		public Vector3 scale;

		// Token: 0x04004C21 RID: 19489
		public static readonly XformOffset Identity = new XformOffset
		{
			_rotQuat = Quaternion.identity,
			scale = Vector3.one
		};
	}
}
