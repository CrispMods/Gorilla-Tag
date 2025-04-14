using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CA5 RID: 3237
	public class QuaternionUtil
	{
		// Token: 0x060051AA RID: 20906 RVA: 0x0019089C File Offset: 0x0018EA9C
		public static float Magnitude(Quaternion q)
		{
			return Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x001908DA File Offset: 0x0018EADA
		public static float MagnitudeSqr(Quaternion q)
		{
			return q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
		}

		// Token: 0x060051AC RID: 20908 RVA: 0x00190914 File Offset: 0x0018EB14
		public static Quaternion Normalize(Quaternion q)
		{
			float num = 1f / QuaternionUtil.Magnitude(q);
			return new Quaternion(num * q.x, num * q.y, num * q.z, num * q.w);
		}

		// Token: 0x060051AD RID: 20909 RVA: 0x00190954 File Offset: 0x0018EB54
		public static Quaternion AngularVector(Vector3 v)
		{
			float magnitude = v.magnitude;
			if (magnitude < MathUtil.Epsilon)
			{
				return Quaternion.identity;
			}
			v /= magnitude;
			float f = 0.5f * magnitude;
			float num = Mathf.Sin(f);
			float w = Mathf.Cos(f);
			return new Quaternion(num * v.x, num * v.y, num * v.z, w);
		}

		// Token: 0x060051AE RID: 20910 RVA: 0x001909B4 File Offset: 0x0018EBB4
		public static Quaternion AxisAngle(Vector3 axis, float angle)
		{
			float f = 0.5f * angle;
			float num = Mathf.Sin(f);
			float w = Mathf.Cos(f);
			return new Quaternion(num * axis.x, num * axis.y, num * axis.z, w);
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x001909F4 File Offset: 0x0018EBF4
		public static Vector3 GetAxis(Quaternion q)
		{
			Vector3 a = new Vector3(q.x, q.y, q.z);
			float magnitude = a.magnitude;
			if (magnitude < MathUtil.Epsilon)
			{
				return Vector3.left;
			}
			return a / magnitude;
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x00190A37 File Offset: 0x0018EC37
		public static float GetAngle(Quaternion q)
		{
			return 2f * Mathf.Acos(Mathf.Clamp(q.w, -1f, 1f));
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x00190A5C File Offset: 0x0018EC5C
		public static Quaternion Pow(Quaternion q, float exp)
		{
			Vector3 axis = QuaternionUtil.GetAxis(q);
			float angle = QuaternionUtil.GetAngle(q);
			return QuaternionUtil.AxisAngle(axis, angle * exp);
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x00190A7E File Offset: 0x0018EC7E
		public static Quaternion Integrate(Quaternion q, Quaternion v, float dt)
		{
			return QuaternionUtil.Pow(v, dt) * q;
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x00190A8D File Offset: 0x0018EC8D
		public static Quaternion Integrate(Quaternion q, Vector3 omega, float dt)
		{
			dt *= 0.5f;
			return QuaternionUtil.Normalize(new Quaternion(omega.x * dt, omega.y * dt, omega.z * dt, 1f) * q);
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x00190AC5 File Offset: 0x0018ECC5
		public static Vector4 ToVector4(Quaternion q)
		{
			return new Vector4(q.x, q.y, q.z, q.w);
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x00190AE4 File Offset: 0x0018ECE4
		public static Quaternion FromVector4(Vector4 v, bool normalize = true)
		{
			if (normalize)
			{
				float sqrMagnitude = v.sqrMagnitude;
				if (sqrMagnitude < MathUtil.Epsilon)
				{
					return Quaternion.identity;
				}
				v /= Mathf.Sqrt(sqrMagnitude);
			}
			return new Quaternion(v.x, v.y, v.z, v.w);
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x00190B38 File Offset: 0x0018ED38
		public static void DecomposeSwingTwist(Quaternion q, Vector3 twistAxis, out Quaternion swing, out Quaternion twist)
		{
			Vector3 vector = new Vector3(q.x, q.y, q.z);
			if (vector.sqrMagnitude < MathUtil.Epsilon)
			{
				Vector3 vector2 = q * twistAxis;
				Vector3 axis = Vector3.Cross(twistAxis, vector2);
				if (axis.sqrMagnitude > MathUtil.Epsilon)
				{
					float angle = Vector3.Angle(twistAxis, vector2);
					swing = Quaternion.AngleAxis(angle, axis);
				}
				else
				{
					swing = Quaternion.identity;
				}
				twist = Quaternion.AngleAxis(180f, twistAxis);
				return;
			}
			Vector3 vector3 = Vector3.Project(vector, twistAxis);
			twist = new Quaternion(vector3.x, vector3.y, vector3.z, q.w);
			twist = QuaternionUtil.Normalize(twist);
			swing = q * Quaternion.Inverse(twist);
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x00190C14 File Offset: 0x0018EE14
		public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float t, QuaternionUtil.SterpMode mode = QuaternionUtil.SterpMode.Slerp)
		{
			Quaternion quaternion;
			Quaternion quaternion2;
			return QuaternionUtil.Sterp(a, b, twistAxis, t, out quaternion, out quaternion2, mode);
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x00190C30 File Offset: 0x0018EE30
		public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float t, out Quaternion swing, out Quaternion twist, QuaternionUtil.SterpMode mode = QuaternionUtil.SterpMode.Slerp)
		{
			return QuaternionUtil.Sterp(a, b, twistAxis, t, t, out swing, out twist, mode);
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x00190C44 File Offset: 0x0018EE44
		public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float tSwing, float tTwist, QuaternionUtil.SterpMode mode = QuaternionUtil.SterpMode.Slerp)
		{
			Quaternion quaternion;
			Quaternion quaternion2;
			return QuaternionUtil.Sterp(a, b, twistAxis, tSwing, tTwist, out quaternion, out quaternion2, mode);
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x00190C64 File Offset: 0x0018EE64
		public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float tSwing, float tTwist, out Quaternion swing, out Quaternion twist, QuaternionUtil.SterpMode mode)
		{
			Quaternion b2;
			Quaternion b3;
			QuaternionUtil.DecomposeSwingTwist(b * Quaternion.Inverse(a), twistAxis, out b2, out b3);
			if (mode == QuaternionUtil.SterpMode.Nlerp || mode != QuaternionUtil.SterpMode.Slerp)
			{
				swing = Quaternion.Lerp(Quaternion.identity, b2, tSwing);
				twist = Quaternion.Lerp(Quaternion.identity, b3, tTwist);
			}
			else
			{
				swing = Quaternion.Slerp(Quaternion.identity, b2, tSwing);
				twist = Quaternion.Slerp(Quaternion.identity, b3, tTwist);
			}
			return twist * swing;
		}

		// Token: 0x02000CA6 RID: 3238
		public enum SterpMode
		{
			// Token: 0x040053E2 RID: 21474
			Nlerp,
			// Token: 0x040053E3 RID: 21475
			Slerp
		}
	}
}
