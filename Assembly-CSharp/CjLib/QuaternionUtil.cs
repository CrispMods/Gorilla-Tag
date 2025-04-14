using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CA2 RID: 3234
	public class QuaternionUtil
	{
		// Token: 0x0600519E RID: 20894 RVA: 0x001902D4 File Offset: 0x0018E4D4
		public static float Magnitude(Quaternion q)
		{
			return Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
		}

		// Token: 0x0600519F RID: 20895 RVA: 0x00190312 File Offset: 0x0018E512
		public static float MagnitudeSqr(Quaternion q)
		{
			return q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
		}

		// Token: 0x060051A0 RID: 20896 RVA: 0x0019034C File Offset: 0x0018E54C
		public static Quaternion Normalize(Quaternion q)
		{
			float num = 1f / QuaternionUtil.Magnitude(q);
			return new Quaternion(num * q.x, num * q.y, num * q.z, num * q.w);
		}

		// Token: 0x060051A1 RID: 20897 RVA: 0x0019038C File Offset: 0x0018E58C
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

		// Token: 0x060051A2 RID: 20898 RVA: 0x001903EC File Offset: 0x0018E5EC
		public static Quaternion AxisAngle(Vector3 axis, float angle)
		{
			float f = 0.5f * angle;
			float num = Mathf.Sin(f);
			float w = Mathf.Cos(f);
			return new Quaternion(num * axis.x, num * axis.y, num * axis.z, w);
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x0019042C File Offset: 0x0018E62C
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

		// Token: 0x060051A4 RID: 20900 RVA: 0x0019046F File Offset: 0x0018E66F
		public static float GetAngle(Quaternion q)
		{
			return 2f * Mathf.Acos(Mathf.Clamp(q.w, -1f, 1f));
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x00190494 File Offset: 0x0018E694
		public static Quaternion Pow(Quaternion q, float exp)
		{
			Vector3 axis = QuaternionUtil.GetAxis(q);
			float angle = QuaternionUtil.GetAngle(q);
			return QuaternionUtil.AxisAngle(axis, angle * exp);
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x001904B6 File Offset: 0x0018E6B6
		public static Quaternion Integrate(Quaternion q, Quaternion v, float dt)
		{
			return QuaternionUtil.Pow(v, dt) * q;
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x001904C5 File Offset: 0x0018E6C5
		public static Quaternion Integrate(Quaternion q, Vector3 omega, float dt)
		{
			dt *= 0.5f;
			return QuaternionUtil.Normalize(new Quaternion(omega.x * dt, omega.y * dt, omega.z * dt, 1f) * q);
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x001904FD File Offset: 0x0018E6FD
		public static Vector4 ToVector4(Quaternion q)
		{
			return new Vector4(q.x, q.y, q.z, q.w);
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x0019051C File Offset: 0x0018E71C
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

		// Token: 0x060051AA RID: 20906 RVA: 0x00190570 File Offset: 0x0018E770
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

		// Token: 0x060051AB RID: 20907 RVA: 0x0019064C File Offset: 0x0018E84C
		public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float t, QuaternionUtil.SterpMode mode = QuaternionUtil.SterpMode.Slerp)
		{
			Quaternion quaternion;
			Quaternion quaternion2;
			return QuaternionUtil.Sterp(a, b, twistAxis, t, out quaternion, out quaternion2, mode);
		}

		// Token: 0x060051AC RID: 20908 RVA: 0x00190668 File Offset: 0x0018E868
		public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float t, out Quaternion swing, out Quaternion twist, QuaternionUtil.SterpMode mode = QuaternionUtil.SterpMode.Slerp)
		{
			return QuaternionUtil.Sterp(a, b, twistAxis, t, t, out swing, out twist, mode);
		}

		// Token: 0x060051AD RID: 20909 RVA: 0x0019067C File Offset: 0x0018E87C
		public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float tSwing, float tTwist, QuaternionUtil.SterpMode mode = QuaternionUtil.SterpMode.Slerp)
		{
			Quaternion quaternion;
			Quaternion quaternion2;
			return QuaternionUtil.Sterp(a, b, twistAxis, tSwing, tTwist, out quaternion, out quaternion2, mode);
		}

		// Token: 0x060051AE RID: 20910 RVA: 0x0019069C File Offset: 0x0018E89C
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

		// Token: 0x02000CA3 RID: 3235
		public enum SterpMode
		{
			// Token: 0x040053D0 RID: 21456
			Nlerp,
			// Token: 0x040053D1 RID: 21457
			Slerp
		}
	}
}
