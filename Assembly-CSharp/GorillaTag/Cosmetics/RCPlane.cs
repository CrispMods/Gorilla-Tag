using System;
using System.Collections.Generic;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C52 RID: 3154
	public class RCPlane : RCVehicle
	{
		// Token: 0x06004ED0 RID: 20176 RVA: 0x001B37EC File Offset: 0x001B19EC
		protected override void Awake()
		{
			base.Awake();
			this.pitchAccelMinMax.x = this.pitchVelocityTargetMinMax.x / this.pitchVelocityRampTimeMinMax.x;
			this.pitchAccelMinMax.y = this.pitchVelocityTargetMinMax.y / this.pitchVelocityRampTimeMinMax.y;
			this.rollAccel = this.rollVelocityTarget / this.rollVelocityRampTime;
			this.thrustAccel = this.thrustVelocityTarget / this.thrustAccelTime;
		}

		// Token: 0x06004ED1 RID: 20177 RVA: 0x001B386C File Offset: 0x001B1A6C
		protected override void AuthorityBeginMobilization()
		{
			base.AuthorityBeginMobilization();
			float x = base.transform.lossyScale.x;
			this.rb.velocity = base.transform.forward * this.initialSpeed * x;
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x001B38B8 File Offset: 0x001B1AB8
		protected override void AuthorityUpdate(float dt)
		{
			base.AuthorityUpdate(dt);
			this.motorLevel = 0f;
			if (this.localState == RCVehicle.State.Mobilized)
			{
				this.motorLevel = this.activeInput.trigger;
			}
			this.leftAileronLevel = 0f;
			this.rightAileronLevel = 0f;
			float magnitude = this.activeInput.joystick.magnitude;
			if (magnitude > 0.01f)
			{
				float num = Mathf.Abs(this.activeInput.joystick.x) / magnitude;
				float num2 = Mathf.Abs(this.activeInput.joystick.y) / magnitude;
				this.leftAileronLevel = Mathf.Clamp(num * this.activeInput.joystick.x + num2 * -this.activeInput.joystick.y, -1f, 1f);
				this.rightAileronLevel = Mathf.Clamp(num * this.activeInput.joystick.x + num2 * this.activeInput.joystick.y, -1f, 1f);
			}
			if (this.networkSync != null)
			{
				this.networkSync.syncedState.dataA = (byte)Mathf.Clamp(Mathf.FloorToInt(this.motorLevel * 255f), 0, 255);
				this.networkSync.syncedState.dataB = (byte)Mathf.Clamp(Mathf.FloorToInt(this.leftAileronLevel * 126f), -126, 126);
				this.networkSync.syncedState.dataC = (byte)Mathf.Clamp(Mathf.FloorToInt(this.rightAileronLevel * 126f), -126, 126);
			}
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x001B3A5C File Offset: 0x001B1C5C
		protected override void RemoteUpdate(float dt)
		{
			base.RemoteUpdate(dt);
			if (this.networkSync != null)
			{
				this.motorLevel = Mathf.Clamp01((float)this.networkSync.syncedState.dataA / 255f);
				this.leftAileronLevel = Mathf.Clamp((float)this.networkSync.syncedState.dataB / 126f, -1f, 1f);
				this.rightAileronLevel = Mathf.Clamp((float)this.networkSync.syncedState.dataC / 126f, -1f, 1f);
			}
		}

		// Token: 0x06004ED4 RID: 20180 RVA: 0x001B3AF8 File Offset: 0x001B1CF8
		protected override void SharedUpdate(float dt)
		{
			base.SharedUpdate(dt);
			switch (this.localState)
			{
			case RCVehicle.State.DockedLeft:
			case RCVehicle.State.DockedRight:
				this.propellerSpinRate = Mathf.MoveTowards(this.propellerSpinRate, 0.6f, 6.6666665f * dt);
				this.propellerAngle += this.propellerSpinRate * 360f * dt;
				this.propeller.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, this.propellerAngle));
				break;
			case RCVehicle.State.Mobilized:
			{
				if (this.localStatePrev != RCVehicle.State.Mobilized)
				{
					this.audioSource.loop = true;
					this.audioSource.clip = this.motorSound;
					this.audioSource.volume = 0f;
					this.audioSource.GTPlay();
				}
				float target = Mathf.Lerp(this.motorSoundVolumeMinMax.x, this.motorSoundVolumeMinMax.y, this.motorLevel);
				this.audioSource.volume = Mathf.MoveTowards(this.audioSource.volume, target, this.motorSoundVolumeMinMax.y / this.motorVolumeRampTime * dt);
				this.propellerSpinRate = Mathf.MoveTowards(this.propellerSpinRate, 5f, 6.6666665f * dt);
				this.propellerAngle += this.propellerSpinRate * 360f * dt;
				this.propeller.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, this.propellerAngle));
				break;
			}
			case RCVehicle.State.Crashed:
				if (this.localStatePrev != RCVehicle.State.Crashed)
				{
					this.audioSource.GTStop();
					this.audioSource.clip = null;
					this.audioSource.loop = false;
					this.audioSource.volume = this.crashSoundVolume;
					if (this.crashSound != null)
					{
						this.audioSource.GTPlayOneShot(this.crashSound, 1f);
					}
				}
				this.propellerSpinRate = Mathf.MoveTowards(this.propellerSpinRate, 0f, 13.333333f * dt);
				this.propellerAngle += this.propellerSpinRate * 360f * dt;
				this.propeller.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, this.propellerAngle));
				break;
			}
			float target2 = Mathf.Lerp(this.aileronAngularRange.x, this.aileronAngularRange.y, Mathf.InverseLerp(-1f, 1f, this.leftAileronLevel));
			float target3 = Mathf.Lerp(this.aileronAngularRange.x, this.aileronAngularRange.y, Mathf.InverseLerp(-1f, 1f, this.rightAileronLevel));
			this.leftAileronAngle = Mathf.MoveTowards(this.leftAileronAngle, target2, this.aileronAngularAcc * Time.deltaTime);
			this.rightAileronAngle = Mathf.MoveTowards(this.rightAileronAngle, target3, this.aileronAngularAcc * Time.deltaTime);
			Quaternion localRotation = Quaternion.Euler(0f, -90f, 90f + this.leftAileronAngle);
			Quaternion localRotation2 = Quaternion.Euler(0f, 90f, -90f + this.rightAileronAngle);
			this.leftAileronLower.localRotation = localRotation;
			this.leftAileronUpper.localRotation = localRotation;
			this.rightAileronLower.localRotation = localRotation2;
			this.rightAileronUpper.localRotation = localRotation2;
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x001B3E60 File Offset: 0x001B2060
		private void FixedUpdate()
		{
			if (!base.HasLocalAuthority || this.localState != RCVehicle.State.Mobilized)
			{
				return;
			}
			float x = base.transform.lossyScale.x;
			float num = this.thrustVelocityTarget * x;
			float num2 = this.thrustAccel * x;
			float fixedDeltaTime = Time.fixedDeltaTime;
			this.pitch = base.NormalizeAngle180(this.pitch);
			this.roll = base.NormalizeAngle180(this.roll);
			float num3 = this.pitch;
			float num4 = this.roll;
			if (this.activeInput.joystick.y >= 0f)
			{
				float target = this.activeInput.joystick.y * this.pitchVelocityTargetMinMax.y;
				this.pitchVel = Mathf.MoveTowards(this.pitchVel, target, this.pitchAccelMinMax.y * fixedDeltaTime);
				this.pitch += this.pitchVel * fixedDeltaTime;
			}
			else
			{
				float target2 = -this.activeInput.joystick.y * this.pitchVelocityTargetMinMax.x;
				this.pitchVel = Mathf.MoveTowards(this.pitchVel, target2, this.pitchAccelMinMax.x * fixedDeltaTime);
				this.pitch += this.pitchVel * fixedDeltaTime;
			}
			float target3 = -this.activeInput.joystick.x * this.rollVelocityTarget;
			this.rollVel = Mathf.MoveTowards(this.rollVel, target3, this.rollAccel * fixedDeltaTime);
			this.roll += this.rollVel * fixedDeltaTime;
			Quaternion rhs = Quaternion.Euler(new Vector3(this.pitch - num3, 0f, this.roll - num4));
			base.transform.rotation = base.transform.rotation * rhs;
			this.rb.angularVelocity = Vector3.zero;
			Vector3 velocity = this.rb.velocity;
			float magnitude = velocity.magnitude;
			float num5 = Mathf.Max(Vector3.Dot(base.transform.forward, velocity), 0f);
			float num6 = this.activeInput.trigger * num;
			float num7 = 0.1f * x;
			if (num6 > num7 && num6 > num5)
			{
				float num8 = Mathf.MoveTowards(num5, num6, num2 * fixedDeltaTime);
				this.rb.AddForce(base.transform.forward * (num8 - num5), ForceMode.VelocityChange);
			}
			float b = 0.01f * x;
			float time = Vector3.Dot(velocity / Mathf.Max(magnitude, b), base.transform.forward);
			float num9 = this.liftVsAttackCurve.Evaluate(time);
			float num10 = Mathf.Lerp(this.liftVsSpeedOutput.x, this.liftVsSpeedOutput.y, Mathf.InverseLerp(this.liftVsSpeedInput.x, this.liftVsSpeedInput.y, magnitude / x));
			float d = num9 * num10;
			Vector3 a = Vector3.RotateTowards(velocity, base.transform.forward * magnitude, this.pitchVelocityFollowRateAngle * 0.017453292f * fixedDeltaTime, this.pitchVelocityFollowRateMagnitude * fixedDeltaTime) - velocity;
			this.rb.AddForce(a * d, ForceMode.VelocityChange);
			float time2 = Vector3.Dot(velocity.normalized, base.transform.up);
			float d2 = this.dragVsAttackCurve.Evaluate(time2);
			this.rb.AddForce(-velocity * this.maxDrag * d2, ForceMode.Acceleration);
			if (this.rb.useGravity)
			{
				float gravityCompensation = Mathf.Lerp(this.gravityCompensationRange.x, this.gravityCompensationRange.y, Mathf.InverseLerp(0f, num, num5 / x));
				RCVehicle.AddScaledGravityCompensationForce(this.rb, x, gravityCompensation);
			}
		}

		// Token: 0x06004ED6 RID: 20182 RVA: 0x001B4218 File Offset: 0x001B2418
		private void OnCollisionEnter(Collision collision)
		{
			if (base.HasLocalAuthority && this.localState == RCVehicle.State.Mobilized)
			{
				for (int i = 0; i < collision.contactCount; i++)
				{
					ContactPoint contact = collision.GetContact(i);
					if (!this.nonCrashColliders.Contains(contact.thisCollider))
					{
						this.AuthorityBeginCrash();
					}
				}
				return;
			}
			bool flag = collision.collider.gameObject.IsOnLayer(UnityLayer.GorillaThrowable);
			bool flag2 = collision.collider.gameObject.IsOnLayer(UnityLayer.GorillaHand);
			if ((flag || flag2) && this.localState == RCVehicle.State.Mobilized)
			{
				Vector3 vector = Vector3.zero;
				if (flag2)
				{
					GorillaHandClimber component = collision.collider.gameObject.GetComponent<GorillaHandClimber>();
					if (component != null)
					{
						vector = ((component.xrNode == XRNode.LeftHand) ? GTPlayer.Instance.leftHandCenterVelocityTracker : GTPlayer.Instance.rightHandCenterVelocityTracker).GetAverageVelocity(true, 0.15f, false);
					}
				}
				else if (collision.rigidbody != null)
				{
					vector = collision.rigidbody.velocity;
				}
				if (flag || vector.sqrMagnitude > 0.01f)
				{
					if (base.HasLocalAuthority)
					{
						this.AuthorityApplyImpact(vector, flag);
						return;
					}
					if (this.networkSync != null)
					{
						this.networkSync.photonView.RPC("HitRCVehicleRPC", RpcTarget.Others, new object[]
						{
							vector,
							flag
						});
					}
				}
			}
		}

		// Token: 0x0400514E RID: 20814
		public Vector2 pitchVelocityTargetMinMax = new Vector2(-180f, 180f);

		// Token: 0x0400514F RID: 20815
		public Vector2 pitchVelocityRampTimeMinMax = new Vector2(-0.75f, 0.75f);

		// Token: 0x04005150 RID: 20816
		public float rollVelocityTarget = 180f;

		// Token: 0x04005151 RID: 20817
		public float rollVelocityRampTime = 0.75f;

		// Token: 0x04005152 RID: 20818
		public float thrustVelocityTarget = 15f;

		// Token: 0x04005153 RID: 20819
		public float thrustAccelTime = 2f;

		// Token: 0x04005154 RID: 20820
		[SerializeField]
		private float pitchVelocityFollowRateAngle = 60f;

		// Token: 0x04005155 RID: 20821
		[SerializeField]
		private float pitchVelocityFollowRateMagnitude = 5f;

		// Token: 0x04005156 RID: 20822
		[SerializeField]
		private float maxDrag = 0.1f;

		// Token: 0x04005157 RID: 20823
		[SerializeField]
		private Vector2 liftVsSpeedInput = new Vector2(0f, 4f);

		// Token: 0x04005158 RID: 20824
		[SerializeField]
		private Vector2 liftVsSpeedOutput = new Vector2(0.5f, 1f);

		// Token: 0x04005159 RID: 20825
		[SerializeField]
		private AnimationCurve liftVsAttackCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x0400515A RID: 20826
		[SerializeField]
		private AnimationCurve dragVsAttackCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x0400515B RID: 20827
		[SerializeField]
		private Vector2 gravityCompensationRange = new Vector2(0.5f, 1f);

		// Token: 0x0400515C RID: 20828
		[SerializeField]
		private List<Collider> nonCrashColliders = new List<Collider>();

		// Token: 0x0400515D RID: 20829
		[SerializeField]
		private Transform propeller;

		// Token: 0x0400515E RID: 20830
		[SerializeField]
		private Transform leftAileronUpper;

		// Token: 0x0400515F RID: 20831
		[SerializeField]
		private Transform leftAileronLower;

		// Token: 0x04005160 RID: 20832
		[SerializeField]
		private Transform rightAileronUpper;

		// Token: 0x04005161 RID: 20833
		[SerializeField]
		private Transform rightAileronLower;

		// Token: 0x04005162 RID: 20834
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04005163 RID: 20835
		[SerializeField]
		private AudioClip motorSound;

		// Token: 0x04005164 RID: 20836
		[SerializeField]
		private AudioClip crashSound;

		// Token: 0x04005165 RID: 20837
		[SerializeField]
		private Vector2 motorSoundVolumeMinMax = new Vector2(0.02f, 0.1f);

		// Token: 0x04005166 RID: 20838
		[SerializeField]
		private float crashSoundVolume = 0.12f;

		// Token: 0x04005167 RID: 20839
		private float motorVolumeRampTime = 1f;

		// Token: 0x04005168 RID: 20840
		private float propellerAngle;

		// Token: 0x04005169 RID: 20841
		private float propellerSpinRate;

		// Token: 0x0400516A RID: 20842
		private const float propellerIdleAcc = 1f;

		// Token: 0x0400516B RID: 20843
		private const float propellerIdleSpinRate = 0.6f;

		// Token: 0x0400516C RID: 20844
		private const float propellerMaxAcc = 6.6666665f;

		// Token: 0x0400516D RID: 20845
		private const float propellerMaxSpinRate = 5f;

		// Token: 0x0400516E RID: 20846
		public float initialSpeed = 3f;

		// Token: 0x0400516F RID: 20847
		private float pitch;

		// Token: 0x04005170 RID: 20848
		private float pitchVel;

		// Token: 0x04005171 RID: 20849
		private Vector2 pitchAccelMinMax;

		// Token: 0x04005172 RID: 20850
		private float roll;

		// Token: 0x04005173 RID: 20851
		private float rollVel;

		// Token: 0x04005174 RID: 20852
		private float rollAccel;

		// Token: 0x04005175 RID: 20853
		private float thrustAccel;

		// Token: 0x04005176 RID: 20854
		private float motorLevel;

		// Token: 0x04005177 RID: 20855
		private float leftAileronLevel;

		// Token: 0x04005178 RID: 20856
		private float rightAileronLevel;

		// Token: 0x04005179 RID: 20857
		private Vector2 aileronAngularRange = new Vector2(-30f, 45f);

		// Token: 0x0400517A RID: 20858
		private float aileronAngularAcc = 120f;

		// Token: 0x0400517B RID: 20859
		private float leftAileronAngle;

		// Token: 0x0400517C RID: 20860
		private float rightAileronAngle;
	}
}
