using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag
{
	// Token: 0x02000BAC RID: 2988
	[Serializable]
	public struct XformOffset
	{
		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06004BD3 RID: 19411 RVA: 0x00061DA5 File Offset: 0x0005FFA5
		// (set) Token: 0x06004BD4 RID: 19412 RVA: 0x00061DAD File Offset: 0x0005FFAD
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

		// Token: 0x06004BD5 RID: 19413 RVA: 0x00061DB6 File Offset: 0x0005FFB6
		public XformOffset(int thisIsAnUnusedDummyValue)
		{
			this.pos = Vector3.zero;
			this._rotQuat = Quaternion.identity;
			this._rotEulerAngles = Vector3.zero;
			this.scale = Vector3.one;
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x00061DE4 File Offset: 0x0005FFE4
		public XformOffset(Vector3 pos, Quaternion rot, Vector3 scale)
		{
			this.pos = pos;
			this._rotQuat = rot;
			this._rotEulerAngles = rot.eulerAngles;
			this.scale = scale;
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x00061E08 File Offset: 0x00060008
		public XformOffset(Vector3 pos, Vector3 rot, Vector3 scale)
		{
			this.pos = pos;
			this._rotQuat = Quaternion.Euler(rot);
			this._rotEulerAngles = rot;
			this.scale = scale;
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x00061E2B File Offset: 0x0006002B
		public XformOffset(Vector3 pos, Quaternion rot)
		{
			this.pos = pos;
			this._rotQuat = rot;
			this._rotEulerAngles = rot.eulerAngles;
			this.scale = Vector3.one;
		}

		// Token: 0x06004BD9 RID: 19417 RVA: 0x00061E53 File Offset: 0x00060053
		public XformOffset(Vector3 pos, Vector3 rot)
		{
			this.pos = pos;
			this._rotQuat = Quaternion.Euler(rot);
			this._rotEulerAngles = rot;
			this.scale = Vector3.one;
		}

		// Token: 0x06004BDA RID: 19418 RVA: 0x001A2E88 File Offset: 0x001A1088
		public XformOffset(Transform boneXform, Transform cosmeticXform)
		{
			this.pos = boneXform.InverseTransformPoint(cosmeticXform.position);
			this._rotQuat = Quaternion.Inverse(boneXform.rotation) * cosmeticXform.rotation;
			this._rotEulerAngles = this._rotQuat.eulerAngles;
			Vector3 lossyScale = boneXform.lossyScale;
			Vector3 lossyScale2 = cosmeticXform.lossyScale;
			this.scale = new Vector3(lossyScale2.x / lossyScale.x, lossyScale2.y / lossyScale.y, lossyScale2.z / lossyScale.z);
		}

		// Token: 0x06004BDB RID: 19419 RVA: 0x001A2F14 File Offset: 0x001A1114
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

		// Token: 0x06004BDC RID: 19420 RVA: 0x001A301C File Offset: 0x001A121C
		public bool Approx(XformOffset other)
		{
			return this.pos.Approx(other.pos, 1E-05f) && this._rotQuat.Approx(other._rotQuat, 1E-06f) && this.scale.Approx(other.scale, 1E-05f);
		}

		// Token: 0x04004D13 RID: 19731
		[Tooltip("The position of the cosmetic relative to the parent bone.")]
		public Vector3 pos;

		// Token: 0x04004D14 RID: 19732
		[FormerlySerializedAs("_edRotQuat")]
		[FormerlySerializedAs("rot")]
		[HideInInspector]
		[SerializeField]
		private Quaternion _rotQuat;

		// Token: 0x04004D15 RID: 19733
		[FormerlySerializedAs("_edRotEulerAngles")]
		[FormerlySerializedAs("_edRotEuler")]
		[HideInInspector]
		[SerializeField]
		private Vector3 _rotEulerAngles;

		// Token: 0x04004D16 RID: 19734
		[Tooltip("The scale of the cosmetic relative to the parent bone.")]
		public Vector3 scale;

		// Token: 0x04004D17 RID: 19735
		public static readonly XformOffset Identity = new XformOffset
		{
			_rotQuat = Quaternion.identity,
			scale = Vector3.one
		};
	}
}
