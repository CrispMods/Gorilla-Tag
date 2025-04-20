﻿using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CE5 RID: 3301
	public class BoingEffector : BoingBase
	{
		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x0600536E RID: 21358 RVA: 0x0006617F File Offset: 0x0006437F
		public Vector3 LinearVelocity
		{
			get
			{
				return this.m_linearVelocity;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x0600536F RID: 21359 RVA: 0x00066187 File Offset: 0x00064387
		public float LinearSpeed
		{
			get
			{
				return this.m_linearVelocity.magnitude;
			}
		}

		// Token: 0x06005370 RID: 21360 RVA: 0x00066194 File Offset: 0x00064394
		public void OnEnable()
		{
			this.m_currPosition = base.transform.position;
			this.m_prevPosition = base.transform.position;
			this.m_linearVelocity = Vector3.zero;
			BoingManager.Register(this);
		}

		// Token: 0x06005371 RID: 21361 RVA: 0x000661C9 File Offset: 0x000643C9
		public void OnDisable()
		{
			BoingManager.Unregister(this);
		}

		// Token: 0x06005372 RID: 21362 RVA: 0x001C96F4 File Offset: 0x001C78F4
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

		// Token: 0x06005373 RID: 21363 RVA: 0x001C9750 File Offset: 0x001C7950
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

		// Token: 0x04005594 RID: 21908
		[Header("Metrics")]
		[Range(0f, 20f)]
		[Tooltip("Maximum radius of influence.")]
		public float Radius = 3f;

		// Token: 0x04005595 RID: 21909
		[Range(0f, 1f)]
		[Tooltip("Fraction of Radius past which influence begins decaying gradually to zero exactly at Radius.\n\ne.g. With a Radius of 10.0 and FullEffectRadiusRatio of 0.5, reactors within distance of 5.0 will be fully influenced, reactors at distance of 7.5 will experience 50% influence, and reactors past distance of 10.0 will not be influenced at all.")]
		public float FullEffectRadiusRatio = 0.5f;

		// Token: 0x04005596 RID: 21910
		[Header("Dynamics")]
		[Range(0f, 100f)]
		[Tooltip("Speed of this effector at which impulse effects will be at maximum strength.\n\ne.g. With a MaxImpulseSpeed of 10.0 and an effector traveling at speed of 4.0, impulse effects will be at 40% maximum strength.")]
		public float MaxImpulseSpeed = 5f;

		// Token: 0x04005597 RID: 21911
		[Tooltip("This affects impulse-related effects.\n\nIf checked, continuous motion will be simulated between frames. This means even if an effector \"teleports\" by moving a huge distance between frames, the effector will still affect all reactors caught on the effector's path in between frames, not just the reactors around the effector's discrete positions at different frames.")]
		public bool ContinuousMotion;

		// Token: 0x04005598 RID: 21912
		[Header("Position Effect")]
		[Range(-10f, 10f)]
		[Tooltip("Distance to push away reactors at maximum influence.\n\ne.g. With a MoveDistance of 2.0, a Radius of 10.0, a FullEffectRadiusRatio of 0.5, and a reactor at distance of 7.5 away from effector, the reactor will be pushed away to 50% of maximum influence, i.e. 50% of MoveDistance, which is a distance of 1.0 away from the effector.")]
		public float MoveDistance = 0.5f;

		// Token: 0x04005599 RID: 21913
		[Range(-200f, 200f)]
		[Tooltip("Under maximum impulse influence (within distance of Radius * FullEffectRadiusRatio and with effector moving at speed faster or equal to MaxImpulaseSpeed), a reactor's movement speed will be maintained to be at least as fast as LinearImpulse (unit: distance per second) in the direction of effector's movement direction.\n\ne.g. With a LinearImpulse of 2.0, a Radius of 10.0, a FullEffectRadiusRatio of 0.5, and a reactor at distance of 7.5 away from effector, the reactor's movement speed in the direction of effector's movement direction will be maintained to be at least 50% of LinearImpulse, which is 1.0 per second.")]
		public float LinearImpulse = 5f;

		// Token: 0x0400559A RID: 21914
		[Header("Rotation Effect")]
		[Range(-180f, 180f)]
		[Tooltip("Angle (in degrees) to rotate reactors at maximum influence. The rotation will point reactors' up vectors (defined individually in the reactor component) away from the effector.\n\ne.g. With a RotationAngle of 20.0, a Radius of 10.0, a FullEffectRadiusRatio of 0.5, and a reactor at distance of 7.5 away from effector, the reactor will be rotated to 50% of maximum influence, i.e. 50% of RotationAngle, which is 10 degrees.")]
		public float RotationAngle = 20f;

		// Token: 0x0400559B RID: 21915
		[Range(-2000f, 2000f)]
		[Tooltip("Under maximum impulse influence (within distance of Radius * FullEffectRadiusRatio and with effector moving at speed faster or equal to MaxImpulaseSpeed), a reactor's rotation speed will be maintained to be at least as fast as AngularImpulse (unit: degrees per second) in the direction of effector's movement direction, i.e. the reactor's up vector will be pulled in the direction of effector's movement direction.\n\ne.g. With a AngularImpulse of 20.0, a Radius of 10.0, a FullEffectRadiusRatio of 0.5, and a reactor at distance of 7.5 away from effector, the reactor's rotation speed in the direction of effector's movement direction will be maintained to be at least 50% of AngularImpulse, which is 10.0 degrees per second.")]
		public float AngularImpulse = 400f;

		// Token: 0x0400559C RID: 21916
		[Header("Debug")]
		[Tooltip("If checked, gizmos of reactor fields affected by this effector will be drawn.")]
		public bool DrawAffectedReactorFieldGizmos;

		// Token: 0x0400559D RID: 21917
		private Vector3 m_currPosition;

		// Token: 0x0400559E RID: 21918
		private Vector3 m_prevPosition;

		// Token: 0x0400559F RID: 21919
		private Vector3 m_linearVelocity;

		// Token: 0x02000CE6 RID: 3302
		public struct Params
		{
			// Token: 0x06005375 RID: 21365 RVA: 0x001C9844 File Offset: 0x001C7A44
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

			// Token: 0x06005376 RID: 21366 RVA: 0x000661D1 File Offset: 0x000643D1
			public void Fill(BoingEffector effector)
			{
				this = new BoingEffector.Params(effector);
			}

			// Token: 0x06005377 RID: 21367 RVA: 0x001C9960 File Offset: 0x001C7B60
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

			// Token: 0x040055A0 RID: 21920
			public static readonly int Stride = 80;

			// Token: 0x040055A1 RID: 21921
			public Vector3 PrevPosition;

			// Token: 0x040055A2 RID: 21922
			private float m_padding0;

			// Token: 0x040055A3 RID: 21923
			public Vector3 CurrPosition;

			// Token: 0x040055A4 RID: 21924
			private float m_padding1;

			// Token: 0x040055A5 RID: 21925
			public Vector3 LinearVelocityDir;

			// Token: 0x040055A6 RID: 21926
			private float m_padding2;

			// Token: 0x040055A7 RID: 21927
			public float Radius;

			// Token: 0x040055A8 RID: 21928
			public float FullEffectRadius;

			// Token: 0x040055A9 RID: 21929
			public float MoveDistance;

			// Token: 0x040055AA RID: 21930
			public float LinearImpulse;

			// Token: 0x040055AB RID: 21931
			public float RotateAngle;

			// Token: 0x040055AC RID: 21932
			public float AngularImpulse;

			// Token: 0x040055AD RID: 21933
			public Bits32 Bits;

			// Token: 0x040055AE RID: 21934
			private int m_padding3;
		}
	}
}
