using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C51 RID: 3153
	public class RCHelicopter : RCVehicle
	{
		// Token: 0x06004ECA RID: 20170 RVA: 0x001B3434 File Offset: 0x001B1634
		protected override void AuthorityBeginDocked()
		{
			base.AuthorityBeginDocked();
			this.turnRate = 0f;
			this.verticalPropeller.localRotation = this.verticalPropellerBaseRotation;
			this.turnPropeller.localRotation = this.turnPropellerBaseRotation;
			if (this.connectedRemote == null)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004ECB RID: 20171 RVA: 0x001B3490 File Offset: 0x001B1690
		protected override void Awake()
		{
			base.Awake();
			this.verticalPropellerBaseRotation = this.verticalPropeller.localRotation;
			this.turnPropellerBaseRotation = this.turnPropeller.localRotation;
			this.ascendAccel = this.maxAscendSpeed / this.ascendAccelTime;
			this.turnAccel = this.maxTurnRate / this.turnAccelTime;
			this.horizontalAccel = this.maxHorizontalSpeed / this.horizontalAccelTime;
		}

		// Token: 0x06004ECC RID: 20172 RVA: 0x001B3500 File Offset: 0x001B1700
		protected override void SharedUpdate(float dt)
		{
			if (this.localState == RCVehicle.State.Mobilized)
			{
				float num = Mathf.Lerp(this.mainPropellerSpinRateRange.x, this.mainPropellerSpinRateRange.y, this.activeInput.trigger);
				this.verticalPropeller.Rotate(new Vector3(0f, num * dt, 0f), Space.Self);
				this.turnPropeller.Rotate(new Vector3(this.activeInput.joystick.x * this.backPropellerSpinRate * dt, 0f, 0f), Space.Self);
			}
		}

		// Token: 0x06004ECD RID: 20173 RVA: 0x001B3590 File Offset: 0x001B1790
		private void FixedUpdate()
		{
			if (!base.HasLocalAuthority || this.localState != RCVehicle.State.Mobilized)
			{
				return;
			}
			float fixedDeltaTime = Time.fixedDeltaTime;
			Vector3 velocity = this.rb.velocity;
			float magnitude = velocity.magnitude;
			float target = this.activeInput.joystick.x * this.maxTurnRate;
			this.turnRate = Mathf.MoveTowards(this.turnRate, target, this.turnAccel * fixedDeltaTime);
			float num = this.activeInput.joystick.y * this.maxHorizontalSpeed;
			float x = Mathf.Sign(this.activeInput.joystick.y) * Mathf.Lerp(0f, this.maxHorizontalTiltAngle, Mathf.Abs(this.activeInput.joystick.y));
			base.transform.rotation = Quaternion.Euler(new Vector3(x, this.turnAccel, 0f));
			float num2 = Mathf.Abs(num);
			Vector3 normalized = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up).normalized;
			float num3 = Vector3.Dot(normalized, velocity);
			if (num2 > 0.01f && ((num > 0f && num > num3) || (num < 0f && num < num3)))
			{
				this.rb.AddForce(normalized * Mathf.Sign(num) * this.horizontalAccel * fixedDeltaTime, ForceMode.Acceleration);
			}
			float num4 = this.activeInput.trigger * this.maxAscendSpeed;
			if (num4 > 0.01f && velocity.y < num4)
			{
				this.rb.AddForce(Vector3.up * this.ascendAccel, ForceMode.Acceleration);
			}
			if (this.rb.useGravity)
			{
				this.rb.AddForce(-Physics.gravity * this.gravityCompensation, ForceMode.Acceleration);
			}
		}

		// Token: 0x06004ECE RID: 20174 RVA: 0x000637CD File Offset: 0x000619CD
		private void OnTriggerEnter(Collider other)
		{
			if (!other.isTrigger && base.HasLocalAuthority && this.localState == RCVehicle.State.Mobilized)
			{
				this.AuthorityBeginCrash();
			}
		}

		// Token: 0x0400513C RID: 20796
		[SerializeField]
		private float maxAscendSpeed = 6f;

		// Token: 0x0400513D RID: 20797
		[SerializeField]
		private float ascendAccelTime = 3f;

		// Token: 0x0400513E RID: 20798
		[SerializeField]
		private float gravityCompensation = 0.5f;

		// Token: 0x0400513F RID: 20799
		[SerializeField]
		private float maxTurnRate = 90f;

		// Token: 0x04005140 RID: 20800
		[SerializeField]
		private float turnAccelTime = 0.75f;

		// Token: 0x04005141 RID: 20801
		[SerializeField]
		private float maxHorizontalSpeed = 6f;

		// Token: 0x04005142 RID: 20802
		[SerializeField]
		private float horizontalAccelTime = 2f;

		// Token: 0x04005143 RID: 20803
		[SerializeField]
		private float maxHorizontalTiltAngle = 45f;

		// Token: 0x04005144 RID: 20804
		[SerializeField]
		private Vector2 mainPropellerSpinRateRange = new Vector2(3f, 15f);

		// Token: 0x04005145 RID: 20805
		[SerializeField]
		private float backPropellerSpinRate = 5f;

		// Token: 0x04005146 RID: 20806
		[SerializeField]
		private Transform verticalPropeller;

		// Token: 0x04005147 RID: 20807
		[SerializeField]
		private Transform turnPropeller;

		// Token: 0x04005148 RID: 20808
		private Quaternion verticalPropellerBaseRotation;

		// Token: 0x04005149 RID: 20809
		private Quaternion turnPropellerBaseRotation;

		// Token: 0x0400514A RID: 20810
		private float turnRate;

		// Token: 0x0400514B RID: 20811
		private float ascendAccel;

		// Token: 0x0400514C RID: 20812
		private float turnAccel;

		// Token: 0x0400514D RID: 20813
		private float horizontalAccel;
	}
}
