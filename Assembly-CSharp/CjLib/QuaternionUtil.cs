using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CD3 RID: 3283
	public class QuaternionUtil
	{
		// Token: 0x06005300 RID: 21248 RVA: 0x00065C7A File Offset: 0x00063E7A
		public static float Magnitude(Quaternion q)
		{
			return Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
		}

		// Token: 0x06005301 RID: 21249 RVA: 0x00065CB8 File Offset: 0x00063EB8
		public static float MagnitudeSqr(Quaternion q)
		{
			return q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
		}

		// Token: 0x06005302 RID: 21250 RVA: 0x001C6B94 File Offset: 0x001C4D94
		public static Quaternion Normalize(Quaternion q)
		{
			float num = 1f / QuaternionUtil.Magnitude(q);
			return new Quaternion(num * q.x, num * q.y, num * q.z, num * q.w);
		}

		// Token: 0x06005303 RID: 21251 RVA: 0x001C6BD4 File Offset: 0x001C4DD4
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

		// Token: 0x06005304 RID: 21252 RVA: 0x001C6C34 File Offset: 0x001C4E34
		public static Quaternion AxisAngle(Vector3 axis, float angle)
		{
			float f = 0.5f * angle;
			float num = Mathf.Sin(f);
			float w = Mathf.Cos(f);
			return new Quaternion(num * axis.x, num * axis.y, num * axis.z, w);
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x001C6C74 File Offset: 0x001C4E74
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

		// Token: 0x06005306 RID: 21254 RVA: 0x00065CF1 File Offset: 0x00063EF1
		public static float GetAngle(Quaternion q)
		{
			return 2f * Mathf.Acos(Mathf.Clamp(q.w, -1f, 1f));
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x001C6CB8 File Offset: 0x001C4EB8
		public static Quaternion Pow(Quaternion q, float exp)
		{
			Vector3 axis = QuaternionUtil.GetAxis(q);
			float angle = QuaternionUtil.GetAngle(q);
			return QuaternionUtil.AxisAngle(axis, angle * exp);
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x00065D13 File Offset: 0x00063F13
		public static Quaternion Integrate(Quaternion q, Quaternion v, float dt)
		{
			return QuaternionUtil.Pow(v, dt) * q;
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x00065D22 File Offset: 0x00063F22
		public static Quaternion Integrate(Quaternion q, Vector3 omega, float dt)
		{
			dt *= 0.5f;
			return QuaternionUtil.Normalize(new Quaternion(omega.x * dt, omega.y * dt, omega.z * dt, 1f) * q);
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x00065D5A File Offset: 0x00063F5A
		public static Vector4 ToVector4(Quaternion q)
		{
			return new Vector4(q.x, q.y, q.z, q.w);
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x001C6CDC File Offset: 0x001C4EDC
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

		// Token: 0x0600530C RID: 21260 RVA: 0x001C6D30 File Offset: 0x001C4F30
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

		// Token: 0x0600530D RID: 21261 RVA: 0x001C6E0C File Offset: 0x001C500C
		public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float t, QuaternionUtil.SterpMode mode = QuaternionUtil.SterpMode.Slerp)
		{
			Quaternion quaternion;
			Quaternion quaternion2;
			return QuaternionUtil.Sterp(a, b, twistAxis, t, out quaternion, out quaternion2, mode);
		}

		// Token: 0x0600530E RID: 21262 RVA: 0x00065D79 File Offset: 0x00063F79
		public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float t, out Quaternion swing, out Quaternion twist, QuaternionUtil.SterpMode mode = QuaternionUtil.SterpMode.Slerp)
		{
			return QuaternionUtil.Sterp(a, b, twistAxis, t, t, out swing, out twist, mode);
		}

		// Token: 0x0600530F RID: 21263 RVA: 0x001C6E28 File Offset: 0x001C5028
		public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float tSwing, float tTwist, QuaternionUtil.SterpMode mode = QuaternionUtil.SterpMode.Slerp)
		{
			Quaternion quaternion;
			Quaternion quaternion2;
			return QuaternionUtil.Sterp(a, b, twistAxis, tSwing, tTwist, out quaternion, out quaternion2, mode);
		}

		// Token: 0x06005310 RID: 21264 RVA: 0x001C6E48 File Offset: 0x001C5048
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

		// Token: 0x02000CD4 RID: 3284
		public enum SterpMode
		{
			// Token: 0x040054DC RID: 21724
			Nlerp,
			// Token: 0x040054DD RID: 21725
			Slerp
		}
	}
}
