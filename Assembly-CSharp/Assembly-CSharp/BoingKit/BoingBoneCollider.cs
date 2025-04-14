using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CB0 RID: 3248
	public class BoingBoneCollider : MonoBehaviour
	{
		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x060051FD RID: 20989 RVA: 0x0019228C File Offset: 0x0019048C
		public Bounds Bounds
		{
			get
			{
				switch (this.Shape)
				{
				case BoingBoneCollider.Type.Sphere:
				{
					float num = VectorUtil.MinComponent(base.transform.localScale);
					return new Bounds(base.transform.position, 2f * num * this.Radius * Vector3.one);
				}
				case BoingBoneCollider.Type.Capsule:
				{
					float num2 = VectorUtil.MinComponent(base.transform.localScale);
					return new Bounds(base.transform.position, 2f * num2 * this.Radius * Vector3.one + this.Height * VectorUtil.ComponentWiseAbs(base.transform.rotation * Vector3.up));
				}
				case BoingBoneCollider.Type.Box:
					return new Bounds(base.transform.position, VectorUtil.ComponentWiseMult(base.transform.localScale, VectorUtil.ComponentWiseAbs(base.transform.rotation * this.Dimensions)));
				default:
					return default(Bounds);
				}
			}
		}

		// Token: 0x060051FE RID: 20990 RVA: 0x0019239C File Offset: 0x0019059C
		public bool Collide(Vector3 boneCenter, float boneRadius, out Vector3 push)
		{
			switch (this.Shape)
			{
			case BoingBoneCollider.Type.Sphere:
			{
				float num = VectorUtil.MinComponent(base.transform.localScale);
				return Collision.SphereSphere(boneCenter, boneRadius, base.transform.position, num * this.Radius, out push);
			}
			case BoingBoneCollider.Type.Capsule:
			{
				float num2 = VectorUtil.MinComponent(base.transform.localScale);
				Vector3 headB = base.transform.TransformPoint(0.5f * this.Height * Vector3.up);
				Vector3 tailB = base.transform.TransformPoint(0.5f * this.Height * Vector3.down);
				return Collision.SphereCapsule(boneCenter, boneRadius, headB, tailB, num2 * this.Radius, out push);
			}
			case BoingBoneCollider.Type.Box:
			{
				Vector3 centerOffsetA = base.transform.InverseTransformPoint(boneCenter);
				Vector3 halfExtentB = 0.5f * VectorUtil.ComponentWiseMult(base.transform.localScale, this.Dimensions);
				if (!Collision.SphereBox(centerOffsetA, boneRadius, halfExtentB, out push))
				{
					return false;
				}
				push = base.transform.TransformVector(push);
				return true;
			}
			default:
				push = Vector3.zero;
				return false;
			}
		}

		// Token: 0x060051FF RID: 20991 RVA: 0x001924C0 File Offset: 0x001906C0
		public void OnValidate()
		{
			this.Radius = Mathf.Max(0f, this.Radius);
			this.Dimensions.x = Mathf.Max(0f, this.Dimensions.x);
			this.Dimensions.y = Mathf.Max(0f, this.Dimensions.y);
			this.Dimensions.z = Mathf.Max(0f, this.Dimensions.z);
		}

		// Token: 0x06005200 RID: 20992 RVA: 0x00192543 File Offset: 0x00190743
		public void OnDrawGizmos()
		{
			this.DrawGizmos();
		}

		// Token: 0x06005201 RID: 20993 RVA: 0x0019254C File Offset: 0x0019074C
		public void DrawGizmos()
		{
			switch (this.Shape)
			{
			case BoingBoneCollider.Type.Sphere:
			{
				float radius = VectorUtil.MinComponent(base.transform.localScale) * this.Radius;
				Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one);
				if (this.Shape == BoingBoneCollider.Type.Sphere)
				{
					Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
					Gizmos.DrawSphere(Vector3.zero, radius);
				}
				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere(Vector3.zero, radius);
				Gizmos.matrix = Matrix4x4.identity;
				return;
			}
			case BoingBoneCollider.Type.Capsule:
			{
				float num = VectorUtil.MinComponent(base.transform.localScale);
				float num2 = num * this.Radius;
				float d = 0.5f * num * this.Height;
				Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one);
				if (this.Shape == BoingBoneCollider.Type.Capsule)
				{
					Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
					Gizmos.DrawSphere(d * Vector3.up, num2);
					Gizmos.DrawSphere(d * Vector3.down, num2);
				}
				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere(d * Vector3.up, num2);
				Gizmos.DrawWireSphere(d * Vector3.down, num2);
				for (int i = 0; i < 4; i++)
				{
					float f = (float)i * MathUtil.HalfPi;
					Vector3 a = new Vector3(num2 * Mathf.Cos(f), 0f, num2 * Mathf.Sin(f));
					Gizmos.DrawLine(a + d * Vector3.up, a + d * Vector3.down);
				}
				Gizmos.matrix = Matrix4x4.identity;
				return;
			}
			case BoingBoneCollider.Type.Box:
			{
				Vector3 size = VectorUtil.ComponentWiseMult(base.transform.localScale, this.Dimensions);
				Gizmos.matrix = base.transform.localToWorldMatrix;
				if (this.Shape == BoingBoneCollider.Type.Box)
				{
					Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
					Gizmos.DrawCube(Vector3.zero, size);
				}
				Gizmos.color = Color.white;
				Gizmos.DrawWireCube(Vector3.zero, size);
				Gizmos.matrix = Matrix4x4.identity;
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x0400543E RID: 21566
		public BoingBoneCollider.Type Shape;

		// Token: 0x0400543F RID: 21567
		public float Radius = 0.1f;

		// Token: 0x04005440 RID: 21568
		public float Height = 0.25f;

		// Token: 0x04005441 RID: 21569
		public Vector3 Dimensions = new Vector3(0.1f, 0.1f, 0.1f);

		// Token: 0x02000CB1 RID: 3249
		public enum Type
		{
			// Token: 0x04005443 RID: 21571
			Sphere,
			// Token: 0x04005444 RID: 21572
			Capsule,
			// Token: 0x04005445 RID: 21573
			Box
		}
	}
}
