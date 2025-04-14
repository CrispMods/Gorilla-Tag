using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CEE RID: 3310
	public class QuaternionUtil
	{
		// Token: 0x0600536C RID: 21356 RVA: 0x0019089C File Offset: 0x0018EA9C
		public static float Magnitude(Quaternion q)
		{
			return Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
		}

		// Token: 0x0600536D RID: 21357 RVA: 0x001908DA File Offset: 0x0018EADA
		public static float MagnitudeSqr(Quaternion q)
		{
			return q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
		}

		// Token: 0x0600536E RID: 21358 RVA: 0x0019B050 File Offset: 0x00199250
		public static Quaternion Normalize(Quaternion q)
		{
			float num = 1f / QuaternionUtil.Magnitude(q);
			return new Quaternion(num * q.x, num * q.y, num * q.z, num * q.w);
		}

		// Token: 0x0600536F RID: 21359 RVA: 0x0019B090 File Offset: 0x00199290
		public static Quaternion AxisAngle(Vector3 axis, float angle)
		{
			float f = 0.5f * angle;
			float num = Mathf.Sin(f);
			float w = Mathf.Cos(f);
			return new Quaternion(num * axis.x, num * axis.y, num * axis.z, w);
		}

		// Token: 0x06005370 RID: 21360 RVA: 0x0019B0D0 File Offset: 0x001992D0
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

		// Token: 0x06005371 RID: 21361 RVA: 0x00190A37 File Offset: 0x0018EC37
		public static float GetAngle(Quaternion q)
		{
			return 2f * Mathf.Acos(Mathf.Clamp(q.w, -1f, 1f));
		}

		// Token: 0x06005372 RID: 21362 RVA: 0x0019B114 File Offset: 0x00199314
		public static Quaternion FromAngularVector(Vector3 v)
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

		// Token: 0x06005373 RID: 21363 RVA: 0x0019B174 File Offset: 0x00199374
		public static Vector3 ToAngularVector(Quaternion q)
		{
			Vector3 axis = QuaternionUtil.GetAxis(q);
			return QuaternionUtil.GetAngle(q) * axis;
		}

		// Token: 0x06005374 RID: 21364 RVA: 0x0019B194 File Offset: 0x00199394
		public static Quaternion Pow(Quaternion q, float exp)
		{
			Vector3 axis = QuaternionUtil.GetAxis(q);
			float angle = QuaternionUtil.GetAngle(q) * exp;
			return QuaternionUtil.AxisAngle(axis, angle);
		}

		// Token: 0x06005375 RID: 21365 RVA: 0x0019B1B6 File Offset: 0x001993B6
		public static Quaternion Integrate(Quaternion q, Quaternion v, float dt)
		{
			return QuaternionUtil.Pow(v, dt) * q;
		}

		// Token: 0x06005376 RID: 21366 RVA: 0x0019B1C8 File Offset: 0x001993C8
		public static Quaternion Integrate(Quaternion q, Vector3 omega, float dt)
		{
			omega *= 0.5f;
			Quaternion quaternion = new Quaternion(omega.x, omega.y, omega.z, 0f) * q;
			return QuaternionUtil.Normalize(new Quaternion(q.x + quaternion.x * dt, q.y + quaternion.y * dt, q.z + quaternion.z * dt, q.w + quaternion.w * dt));
		}

		// Token: 0x06005377 RID: 21367 RVA: 0x00190AC5 File Offset: 0x0018ECC5
		public static Vector4 ToVector4(Quaternion q)
		{
			return new Vector4(q.x, q.y, q.z, q.w);
		}

		// Token: 0x06005378 RID: 21368 RVA: 0x0019B24C File Offset: 0x0019944C
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

		// Token: 0x06005379 RID: 21369 RVA: 0x0019B2A0 File Offset: 0x001994A0
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

		// Token: 0x0600537A RID: 21370 RVA: 0x0019B37C File Offset: 0x0019957C
		public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float t, QuaternionUtil.SterpMode mode = QuaternionUtil.SterpMode.Slerp)
		{
			Quaternion quaternion;
			Quaternion quaternion2;
			return QuaternionUtil.Sterp(a, b, twistAxis, t, out quaternion, out quaternion2, mode);
		}

		// Token: 0x0600537B RID: 21371 RVA: 0x0019B398 File Offset: 0x00199598
		public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float t, out Quaternion swing, out Quaternion twist, QuaternionUtil.SterpMode mode = QuaternionUtil.SterpMode.Slerp)
		{
			return QuaternionUtil.Sterp(a, b, twistAxis, t, t, out swing, out twist, mode);
		}

		// Token: 0x0600537C RID: 21372 RVA: 0x0019B3AC File Offset: 0x001995AC
		public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float tSwing, float tTwist, QuaternionUtil.SterpMode mode = QuaternionUtil.SterpMode.Slerp)
		{
			Quaternion quaternion;
			Quaternion quaternion2;
			return QuaternionUtil.Sterp(a, b, twistAxis, tSwing, tTwist, out quaternion, out quaternion2, mode);
		}

		// Token: 0x0600537D RID: 21373 RVA: 0x0019B3CC File Offset: 0x001995CC
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

		// Token: 0x02000CEF RID: 3311
		public enum SterpMode
		{
			// Token: 0x040055DA RID: 21978
			Nlerp,
			// Token: 0x040055DB RID: 21979
			Slerp
		}
	}
}
