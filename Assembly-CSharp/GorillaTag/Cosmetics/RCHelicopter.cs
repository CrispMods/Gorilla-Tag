using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C23 RID: 3107
	public class RCHelicopter : RCVehicle
	{
		// Token: 0x06004D79 RID: 19833 RVA: 0x0017AD24 File Offset: 0x00178F24
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

		// Token: 0x06004D7A RID: 19834 RVA: 0x0017AD80 File Offset: 0x00178F80
		protected override void Awake()
		{
			base.Awake();
			this.verticalPropellerBaseRotation = this.verticalPropeller.localRotation;
			this.turnPropellerBaseRotation = this.turnPropeller.localRotation;
			this.ascendAccel = this.maxAscendSpeed / this.ascendAccelTime;
			this.turnAccel = this.maxTurnRate / this.turnAccelTime;
			this.horizontalAccel = this.maxHorizontalSpeed / this.horizontalAccelTime;
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x0017ADF0 File Offset: 0x00178FF0
		protected override void SharedUpdate(float dt)
		{
			if (this.localState == RCVehicle.State.Mobilized)
			{
				float num = Mathf.Lerp(this.mainPropellerSpinRateRange.x, this.mainPropellerSpinRateRange.y, this.activeInput.trigger);
				this.verticalPropeller.Rotate(new Vector3(0f, num * dt, 0f), Space.Self);
				this.turnPropeller.Rotate(new Vector3(this.activeInput.joystick.x * this.backPropellerSpinRate * dt, 0f, 0f), Space.Self);
			}
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x0017AE80 File Offset: 0x00179080
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

		// Token: 0x06004D7D RID: 19837 RVA: 0x0017B04E File Offset: 0x0017924E
		private void OnTriggerEnter(Collider other)
		{
			if (!other.isTrigger && base.HasLocalAuthority && this.localState == RCVehicle.State.Mobilized)
			{
				this.AuthorityBeginCrash();
			}
		}

		// Token: 0x04005046 RID: 20550
		[SerializeField]
		private float maxAscendSpeed = 6f;

		// Token: 0x04005047 RID: 20551
		[SerializeField]
		private float ascendAccelTime = 3f;

		// Token: 0x04005048 RID: 20552
		[SerializeField]
		private float gravityCompensation = 0.5f;

		// Token: 0x04005049 RID: 20553
		[SerializeField]
		private float maxTurnRate = 90f;

		// Token: 0x0400504A RID: 20554
		[SerializeField]
		private float turnAccelTime = 0.75f;

		// Token: 0x0400504B RID: 20555
		[SerializeField]
		private float maxHorizontalSpeed = 6f;

		// Token: 0x0400504C RID: 20556
		[SerializeField]
		private float horizontalAccelTime = 2f;

		// Token: 0x0400504D RID: 20557
		[SerializeField]
		private float maxHorizontalTiltAngle = 45f;

		// Token: 0x0400504E RID: 20558
		[SerializeField]
		private Vector2 mainPropellerSpinRateRange = new Vector2(3f, 15f);

		// Token: 0x0400504F RID: 20559
		[SerializeField]
		private float backPropellerSpinRate = 5f;

		// Token: 0x04005050 RID: 20560
		[SerializeField]
		private Transform verticalPropeller;

		// Token: 0x04005051 RID: 20561
		[SerializeField]
		private Transform turnPropeller;

		// Token: 0x04005052 RID: 20562
		private Quaternion verticalPropellerBaseRotation;

		// Token: 0x04005053 RID: 20563
		private Quaternion turnPropellerBaseRotation;

		// Token: 0x04005054 RID: 20564
		private float turnRate;

		// Token: 0x04005055 RID: 20565
		private float ascendAccel;

		// Token: 0x04005056 RID: 20566
		private float turnAccel;

		// Token: 0x04005057 RID: 20567
		private float horizontalAccel;
	}
}
