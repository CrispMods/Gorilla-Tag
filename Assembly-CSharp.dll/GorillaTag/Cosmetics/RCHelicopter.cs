using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C26 RID: 3110
	public class RCHelicopter : RCVehicle
	{
		// Token: 0x06004D85 RID: 19845 RVA: 0x001ABB48 File Offset: 0x001A9D48
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

		// Token: 0x06004D86 RID: 19846 RVA: 0x001ABBA4 File Offset: 0x001A9DA4
		protected override void Awake()
		{
			base.Awake();
			this.verticalPropellerBaseRotation = this.verticalPropeller.localRotation;
			this.turnPropellerBaseRotation = this.turnPropeller.localRotation;
			this.ascendAccel = this.maxAscendSpeed / this.ascendAccelTime;
			this.turnAccel = this.maxTurnRate / this.turnAccelTime;
			this.horizontalAccel = this.maxHorizontalSpeed / this.horizontalAccelTime;
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x001ABC14 File Offset: 0x001A9E14
		protected override void SharedUpdate(float dt)
		{
			if (this.localState == RCVehicle.State.Mobilized)
			{
				float num = Mathf.Lerp(this.mainPropellerSpinRateRange.x, this.mainPropellerSpinRateRange.y, this.activeInput.trigger);
				this.verticalPropeller.Rotate(new Vector3(0f, num * dt, 0f), Space.Self);
				this.turnPropeller.Rotate(new Vector3(this.activeInput.joystick.x * this.backPropellerSpinRate * dt, 0f, 0f), Space.Self);
			}
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x001ABCA4 File Offset: 0x001A9EA4
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

		// Token: 0x06004D89 RID: 19849 RVA: 0x00061E0C File Offset: 0x0006000C
		private void OnTriggerEnter(Collider other)
		{
			if (!other.isTrigger && base.HasLocalAuthority && this.localState == RCVehicle.State.Mobilized)
			{
				this.AuthorityBeginCrash();
			}
		}

		// Token: 0x04005058 RID: 20568
		[SerializeField]
		private float maxAscendSpeed = 6f;

		// Token: 0x04005059 RID: 20569
		[SerializeField]
		private float ascendAccelTime = 3f;

		// Token: 0x0400505A RID: 20570
		[SerializeField]
		private float gravityCompensation = 0.5f;

		// Token: 0x0400505B RID: 20571
		[SerializeField]
		private float maxTurnRate = 90f;

		// Token: 0x0400505C RID: 20572
		[SerializeField]
		private float turnAccelTime = 0.75f;

		// Token: 0x0400505D RID: 20573
		[SerializeField]
		private float maxHorizontalSpeed = 6f;

		// Token: 0x0400505E RID: 20574
		[SerializeField]
		private float horizontalAccelTime = 2f;

		// Token: 0x0400505F RID: 20575
		[SerializeField]
		private float maxHorizontalTiltAngle = 45f;

		// Token: 0x04005060 RID: 20576
		[SerializeField]
		private Vector2 mainPropellerSpinRateRange = new Vector2(3f, 15f);

		// Token: 0x04005061 RID: 20577
		[SerializeField]
		private float backPropellerSpinRate = 5f;

		// Token: 0x04005062 RID: 20578
		[SerializeField]
		private Transform verticalPropeller;

		// Token: 0x04005063 RID: 20579
		[SerializeField]
		private Transform turnPropeller;

		// Token: 0x04005064 RID: 20580
		private Quaternion verticalPropellerBaseRotation;

		// Token: 0x04005065 RID: 20581
		private Quaternion turnPropellerBaseRotation;

		// Token: 0x04005066 RID: 20582
		private float turnRate;

		// Token: 0x04005067 RID: 20583
		private float ascendAccel;

		// Token: 0x04005068 RID: 20584
		private float turnAccel;

		// Token: 0x04005069 RID: 20585
		private float horizontalAccel;
	}
}
