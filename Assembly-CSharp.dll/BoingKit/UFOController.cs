using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CAD RID: 3245
	public class UFOController : MonoBehaviour
	{
		// Token: 0x060051DF RID: 20959 RVA: 0x001BF474 File Offset: 0x001BD674
		private void Start()
		{
			this.m_linearVelocity = Vector3.zero;
			this.m_angularVelocity = 0f;
			this.m_yawAngle = base.transform.rotation.eulerAngles.y * MathUtil.Deg2Rad;
			this.m_hoverCenter = base.transform.position;
			this.m_hoverPhase = 0f;
			this.m_motorAngle = 0f;
			if (this.Eyes != null)
			{
				this.m_eyeInitScale = this.Eyes.localScale;
				this.m_eyeInitPositionLs = this.Eyes.localPosition;
				this.m_blinkTimer = this.BlinkInterval + UnityEngine.Random.Range(1f, 2f);
				this.m_lastBlinkWasDouble = false;
				this.m_eyeScaleSpring.Reset(this.m_eyeInitScale);
				this.m_eyePositionLsSpring.Reset(this.m_eyeInitPositionLs);
			}
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x000644A0 File Offset: 0x000626A0
		private void OnEnable()
		{
			this.Start();
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x001BF558 File Offset: 0x001BD758
		private void FixedUpdate()
		{
			float fixedDeltaTime = Time.fixedDeltaTime;
			Vector3 a = Vector3.zero;
			if (Input.GetKey(KeyCode.W))
			{
				a += Vector3.forward;
			}
			if (Input.GetKey(KeyCode.S))
			{
				a += Vector3.back;
			}
			if (Input.GetKey(KeyCode.A))
			{
				a += Vector3.left;
			}
			if (Input.GetKey(KeyCode.D))
			{
				a += Vector3.right;
			}
			if (Input.GetKey(KeyCode.R))
			{
				a += Vector3.up;
			}
			if (Input.GetKey(KeyCode.F))
			{
				a += Vector3.down;
			}
			if (a.sqrMagnitude > MathUtil.Epsilon)
			{
				a = a.normalized * this.LinearThrust;
				this.m_linearVelocity += a * fixedDeltaTime;
				this.m_linearVelocity = VectorUtil.ClampLength(this.m_linearVelocity, 0f, this.MaxLinearSpeed);
			}
			else
			{
				this.m_linearVelocity = VectorUtil.ClampLength(this.m_linearVelocity, 0f, Mathf.Max(0f, this.m_linearVelocity.magnitude - this.LinearDrag * fixedDeltaTime));
			}
			float magnitude = this.m_linearVelocity.magnitude;
			float t = magnitude * MathUtil.InvSafe(this.MaxLinearSpeed);
			Quaternion lhs = Quaternion.identity;
			float num = 0f;
			if (magnitude > MathUtil.Epsilon)
			{
				Vector3 linearVelocity = this.m_linearVelocity;
				linearVelocity.y = 0f;
				float num2 = (this.m_linearVelocity.magnitude > 0.01f) ? (1f - Mathf.Clamp01(Mathf.Abs(this.m_linearVelocity.y) / this.m_linearVelocity.magnitude)) : 0f;
				num = Mathf.Min(1f, magnitude / Mathf.Max(MathUtil.Epsilon, this.MaxLinearSpeed)) * num2;
				Vector3 normalized = Vector3.Cross(Vector3.up, linearVelocity).normalized;
				float angle = this.Tilt * MathUtil.Deg2Rad * num;
				lhs = QuaternionUtil.AxisAngle(normalized, angle);
			}
			float num3 = 0f;
			if (Input.GetKey(KeyCode.Q))
			{
				num3 -= 1f;
			}
			if (Input.GetKey(KeyCode.E))
			{
				num3 += 1f;
			}
			bool key = Input.GetKey(KeyCode.LeftControl);
			if (Mathf.Abs(num3) > MathUtil.Epsilon)
			{
				float num4 = this.MaxAngularSpeed * (key ? 2.5f : 1f);
				num3 *= this.AngularThrust * MathUtil.Deg2Rad;
				this.m_angularVelocity += num3 * fixedDeltaTime;
				this.m_angularVelocity = Mathf.Clamp(this.m_angularVelocity, -num4 * MathUtil.Deg2Rad, num4 * MathUtil.Deg2Rad);
			}
			else
			{
				this.m_angularVelocity -= Mathf.Sign(this.m_angularVelocity) * Mathf.Min(Mathf.Abs(this.m_angularVelocity), this.AngularDrag * MathUtil.Deg2Rad * fixedDeltaTime);
			}
			this.m_yawAngle += this.m_angularVelocity * fixedDeltaTime;
			Quaternion rhs = QuaternionUtil.AxisAngle(Vector3.up, this.m_yawAngle);
			this.m_hoverCenter += this.m_linearVelocity * fixedDeltaTime;
			this.m_hoverPhase += Time.deltaTime;
			Vector3 vector = 0.05f * Mathf.Sin(1.37f * this.m_hoverPhase) * Vector3.right + 0.05f * Mathf.Sin(1.93f * this.m_hoverPhase + 1.234f) * Vector3.forward + 0.04f * Mathf.Sin(0.97f * this.m_hoverPhase + 4.321f) * Vector3.up;
			vector *= this.Hover;
			Quaternion rhs2 = Quaternion.FromToRotation(Vector3.up, vector + Vector3.up);
			base.transform.position = this.m_hoverCenter + vector;
			base.transform.rotation = lhs * rhs * rhs2;
			if (this.Motor != null)
			{
				float num5 = Mathf.Lerp(this.MotorBaseAngularSpeed, this.MotorMaxAngularSpeed, num);
				this.m_motorAngle += num5 * MathUtil.Deg2Rad * fixedDeltaTime;
				this.Motor.localRotation = QuaternionUtil.AxisAngle(Vector3.up, this.m_motorAngle - this.m_yawAngle);
			}
			if (this.BubbleEmitter != null)
			{
				this.BubbleEmitter.emission.rateOverTime = Mathf.Lerp(this.BubbleBaseEmissionRate, this.BubbleMaxEmissionRate, t);
			}
			if (this.Eyes != null)
			{
				this.m_blinkTimer -= fixedDeltaTime;
				if (this.m_blinkTimer <= 0f)
				{
					bool flag = !this.m_lastBlinkWasDouble && UnityEngine.Random.Range(0f, 1f) > 0.75f;
					this.m_blinkTimer = (flag ? 0.2f : (this.BlinkInterval + UnityEngine.Random.Range(1f, 2f)));
					this.m_lastBlinkWasDouble = flag;
					this.m_eyeScaleSpring.Value.y = 0f;
					this.m_eyePositionLsSpring.Value.y = this.m_eyePositionLsSpring.Value.y - 0.025f;
				}
				this.Eyes.localScale = this.m_eyeScaleSpring.TrackDampingRatio(this.m_eyeInitScale, 30f, 0.8f, fixedDeltaTime);
				this.Eyes.localPosition = this.m_eyePositionLsSpring.TrackDampingRatio(this.m_eyeInitPositionLs, 30f, 0.8f, fixedDeltaTime);
			}
		}

		// Token: 0x04005404 RID: 21508
		public float LinearThrust = 3f;

		// Token: 0x04005405 RID: 21509
		public float MaxLinearSpeed = 2.5f;

		// Token: 0x04005406 RID: 21510
		public float LinearDrag = 4f;

		// Token: 0x04005407 RID: 21511
		public float Tilt = 15f;

		// Token: 0x04005408 RID: 21512
		public float AngularThrust = 30f;

		// Token: 0x04005409 RID: 21513
		public float MaxAngularSpeed = 30f;

		// Token: 0x0400540A RID: 21514
		public float AngularDrag = 30f;

		// Token: 0x0400540B RID: 21515
		[Range(0f, 1f)]
		public float Hover = 1f;

		// Token: 0x0400540C RID: 21516
		public Transform Eyes;

		// Token: 0x0400540D RID: 21517
		public float BlinkInterval = 5f;

		// Token: 0x0400540E RID: 21518
		private float m_blinkTimer;

		// Token: 0x0400540F RID: 21519
		private bool m_lastBlinkWasDouble;

		// Token: 0x04005410 RID: 21520
		private Vector3 m_eyeInitScale;

		// Token: 0x04005411 RID: 21521
		private Vector3 m_eyeInitPositionLs;

		// Token: 0x04005412 RID: 21522
		private Vector3Spring m_eyeScaleSpring;

		// Token: 0x04005413 RID: 21523
		private Vector3Spring m_eyePositionLsSpring;

		// Token: 0x04005414 RID: 21524
		public Transform Motor;

		// Token: 0x04005415 RID: 21525
		public float MotorBaseAngularSpeed = 10f;

		// Token: 0x04005416 RID: 21526
		public float MotorMaxAngularSpeed = 10f;

		// Token: 0x04005417 RID: 21527
		public ParticleSystem BubbleEmitter;

		// Token: 0x04005418 RID: 21528
		public float BubbleBaseEmissionRate = 10f;

		// Token: 0x04005419 RID: 21529
		public float BubbleMaxEmissionRate = 10f;

		// Token: 0x0400541A RID: 21530
		private Vector3 m_linearVelocity;

		// Token: 0x0400541B RID: 21531
		private float m_angularVelocity;

		// Token: 0x0400541C RID: 21532
		private float m_yawAngle;

		// Token: 0x0400541D RID: 21533
		private Vector3 m_hoverCenter;

		// Token: 0x0400541E RID: 21534
		private float m_hoverPhase;

		// Token: 0x0400541F RID: 21535
		private float m_motorAngle;
	}
}
