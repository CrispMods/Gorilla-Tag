﻿using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CB7 RID: 3255
	public class BoingEffector : BoingBase
	{
		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06005218 RID: 21016 RVA: 0x00064709 File Offset: 0x00062909
		public Vector3 LinearVelocity
		{
			get
			{
				return this.m_linearVelocity;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06005219 RID: 21017 RVA: 0x00064711 File Offset: 0x00062911
		public float LinearSpeed
		{
			get
			{
				return this.m_linearVelocity.magnitude;
			}
		}

		// Token: 0x0600521A RID: 21018 RVA: 0x0006471E File Offset: 0x0006291E
		public void OnEnable()
		{
			this.m_currPosition = base.transform.position;
			this.m_prevPosition = base.transform.position;
			this.m_linearVelocity = Vector3.zero;
			BoingManager.Register(this);
		}

		// Token: 0x0600521B RID: 21019 RVA: 0x00064753 File Offset: 0x00062953
		public void OnDisable()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x0600521C RID: 21020 RVA: 0x001C1610 File Offset: 0x001BF810
		public void Update()
		{
			float deltaTime = Time.deltaTime;
			if (deltaTime < MathUtil.Epsilon)
			{
				return;
			}
			this.m_linearVelocity = (base.transform.position - this.m_prevPosition) / deltaTime;
			this.m_prevPosition = this.m_currPosition;
			this.m_currPosition = base.transform.position;
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x001C166C File Offset: 0x001BF86C
		public void OnDrawGizmosSelected()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.FullEffectRadiusRatio < 1f)
			{
				Gizmos.color = new Color(1f, 0.5f, 0.2f, 0.4f);
				Gizmos.DrawWireSphere(base.transform.position, this.Radius);
			}
			Gizmos.color = new Color(1f, 0.5f, 0.2f, 1f);
			Gizmos.DrawWireSphere(base.transform.position, this.Radius * this.FullEffectRadiusRatio);
		}

		// Token: 0x0400549A RID: 21658
		[Header("Metrics")]
		[Range(0f, 20f)]
		[Tooltip("Maximum radius of influence.")]
		public float Radius = 3f;

		// Token: 0x0400549B RID: 21659
		[Range(0f, 1f)]
		[Tooltip("Fraction of Radius past which influence begins decaying gradually to zero exactly at Radius.\n\ne.g. With a Radius of 10.0 and FullEffectRadiusRatio of 0.5, reactors within distance of 5.0 will be fully influenced, reactors at distance of 7.5 will experience 50% influence, and reactors past distance of 10.0 will not be influenced at all.")]
		public float FullEffectRadiusRatio = 0.5f;

		// Token: 0x0400549C RID: 21660
		[Header("Dynamics")]
		[Range(0f, 100f)]
		[Tooltip("Speed of this effector at which impulse effects will be at maximum strength.\n\ne.g. With a MaxImpulseSpeed of 10.0 and an effector traveling at speed of 4.0, impulse effects will be at 40% maximum strength.")]
		public float MaxImpulseSpeed = 5f;

		// Token: 0x0400549D RID: 21661
		[Tooltip("This affects impulse-related effects.\n\nIf checked, continuous motion will be simulated between frames. This means even if an effector \"teleports\" by moving a huge distance between frames, the effector will still affect all reactors caught on the effector's path in between frames, not just the reactors around the effector's discrete positions at different frames.")]
		public bool ContinuousMotion;

		// Token: 0x0400549E RID: 21662
		[Header("Position Effect")]
		[Range(-10f, 10f)]
		[Tooltip("Distance to push away reactors at maximum influence.\n\ne.g. With a MoveDistance of 2.0, a Radius of 10.0, a FullEffectRadiusRatio of 0.5, and a reactor at distance of 7.5 away from effector, the reactor will be pushed away to 50% of maximum influence, i.e. 50% of MoveDistance, which is a distance of 1.0 away from the effector.")]
		public float MoveDistance = 0.5f;

		// Token: 0x0400549F RID: 21663
		[Range(-200f, 200f)]
		[Tooltip("Under maximum impulse influence (within distance of Radius * FullEffectRadiusRatio and with effector moving at speed faster or equal to MaxImpulaseSpeed), a reactor's movement speed will be maintained to be at least as fast as LinearImpulse (unit: distance per second) in the direction of effector's movement direction.\n\ne.g. With a LinearImpulse of 2.0, a Radius of 10.0, a FullEffectRadiusRatio of 0.5, and a reactor at distance of 7.5 away from effector, the reactor's movement speed in the direction of effector's movement direction will be maintained to be at least 50% of LinearImpulse, which is 1.0 per second.")]
		public float LinearImpulse = 5f;

		// Token: 0x040054A0 RID: 21664
		[Header("Rotation Effect")]
		[Range(-180f, 180f)]
		[Tooltip("Angle (in degrees) to rotate reactors at maximum influence. The rotation will point reactors' up vectors (defined individually in the reactor component) away from the effector.\n\ne.g. With a RotationAngle of 20.0, a Radius of 10.0, a FullEffectRadiusRatio of 0.5, and a reactor at distance of 7.5 away from effector, the reactor will be rotated to 50% of maximum influence, i.e. 50% of RotationAngle, which is 10 degrees.")]
		public float RotationAngle = 20f;

		// Token: 0x040054A1 RID: 21665
		[Range(-2000f, 2000f)]
		[Tooltip("Under maximum impulse influence (within distance of Radius * FullEffectRadiusRatio and with effector moving at speed faster or equal to MaxImpulaseSpeed), a reactor's rotation speed will be maintained to be at least as fast as AngularImpulse (unit: degrees per second) in the direction of effector's movement direction, i.e. the reactor's up vector will be pulled in the direction of effector's movement direction.\n\ne.g. With a AngularImpulse of 20.0, a Radius of 10.0, a FullEffectRadiusRatio of 0.5, and a reactor at distance of 7.5 away from effector, the reactor's rotation speed in the direction of effector's movement direction will be maintained to be at least 50% of AngularImpulse, which is 10.0 degrees per second.")]
		public float AngularImpulse = 400f;

		// Token: 0x040054A2 RID: 21666
		[Header("Debug")]
		[Tooltip("If checked, gizmos of reactor fields affected by this effector will be drawn.")]
		public bool DrawAffectedReactorFieldGizmos;

		// Token: 0x040054A3 RID: 21667
		private Vector3 m_currPosition;

		// Token: 0x040054A4 RID: 21668
		private Vector3 m_prevPosition;

		// Token: 0x040054A5 RID: 21669
		private Vector3 m_linearVelocity;

		// Token: 0x02000CB8 RID: 3256
		public struct Params
		{
			// Token: 0x0600521F RID: 21023 RVA: 0x001C1760 File Offset: 0x001BF960
			public Params(BoingEffector effector)
			{
				this.Bits = default(Bits32);
				this.Bits.SetBit(0, effector.ContinuousMotion);
				float num = (effector.MaxImpulseSpeed > MathUtil.Epsilon) ? Mathf.Min(1f, effector.LinearSpeed / effector.MaxImpulseSpeed) : 1f;
				this.PrevPosition = effector.m_prevPosition;
				this.CurrPosition = effector.m_currPosition;
				this.LinearVelocityDir = VectorUtil.NormalizeSafe(effector.LinearVelocity, Vector3.zero);
				this.Radius = effector.Radius;
				this.FullEffectRadius = this.Radius * effector.FullEffectRadiusRatio;
				this.MoveDistance = effector.MoveDistance;
				this.LinearImpulse = num * effector.LinearImpulse;
				this.RotateAngle = effector.RotationAngle * MathUtil.Deg2Rad;
				this.AngularImpulse = num * effector.AngularImpulse * MathUtil.Deg2Rad;
				this.m_padding0 = 0f;
				this.m_padding1 = 0f;
				this.m_padding2 = 0f;
				this.m_padding3 = 0;
			}

			// Token: 0x06005220 RID: 21024 RVA: 0x0006475B File Offset: 0x0006295B
			public void Fill(BoingEffector effector)
			{
				this = new BoingEffector.Params(effector);
			}

			// Token: 0x06005221 RID: 21025 RVA: 0x001C187C File Offset: 0x001BFA7C
			private void SuppressWarnings()
			{
				this.m_padding0 = 0f;
				this.m_padding1 = 0f;
				this.m_padding2 = 0f;
				this.m_padding3 = 0;
				this.m_padding0 = this.m_padding1;
				this.m_padding1 = this.m_padding2;
				this.m_padding2 = (float)this.m_padding3;
				this.m_padding3 = (int)this.m_padding0;
			}

			// Token: 0x040054A6 RID: 21670
			public static readonly int Stride = 80;

			// Token: 0x040054A7 RID: 21671
			public Vector3 PrevPosition;

			// Token: 0x040054A8 RID: 21672
			private float m_padding0;

			// Token: 0x040054A9 RID: 21673
			public Vector3 CurrPosition;

			// Token: 0x040054AA RID: 21674
			private float m_padding1;

			// Token: 0x040054AB RID: 21675
			public Vector3 LinearVelocityDir;

			// Token: 0x040054AC RID: 21676
			private float m_padding2;

			// Token: 0x040054AD RID: 21677
			public float Radius;

			// Token: 0x040054AE RID: 21678
			public float FullEffectRadius;

			// Token: 0x040054AF RID: 21679
			public float MoveDistance;

			// Token: 0x040054B0 RID: 21680
			public float LinearImpulse;

			// Token: 0x040054B1 RID: 21681
			public float RotateAngle;

			// Token: 0x040054B2 RID: 21682
			public float AngularImpulse;

			// Token: 0x040054B3 RID: 21683
			public Bits32 Bits;

			// Token: 0x040054B4 RID: 21684
			private int m_padding3;
		}
	}
}
