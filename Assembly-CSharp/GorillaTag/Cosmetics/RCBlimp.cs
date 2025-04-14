using System;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C1F RID: 3103
	public class RCBlimp : RCVehicle
	{
		// Token: 0x06004D5D RID: 19805 RVA: 0x00179334 File Offset: 0x00177534
		protected override void AuthorityBeginDocked()
		{
			base.AuthorityBeginDocked();
			this.turnRate = 0f;
			this.turnAngle = Vector3.SignedAngle(Vector3.forward, Vector3.ProjectOnPlane(base.transform.forward, Vector3.up), Vector3.up);
			this.motorLevel = 0f;
			if (this.connectedRemote == null)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x001793A4 File Offset: 0x001775A4
		protected override void Awake()
		{
			base.Awake();
			this.ascendAccel = this.maxAscendSpeed / this.ascendAccelTime;
			this.turnAccel = this.maxTurnRate / this.turnAccelTime;
			this.horizontalAccel = this.maxHorizontalSpeed / this.horizontalAccelTime;
			this.tiltAccel = this.maxHorizontalTiltAngle / this.horizontalTiltTime;
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x00179403 File Offset: 0x00177603
		protected override void OnDisable()
		{
			base.OnDisable();
			this.audioSource.GTStop();
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x00179418 File Offset: 0x00177618
		protected override void AuthorityUpdate(float dt)
		{
			base.AuthorityUpdate(dt);
			this.motorLevel = 0f;
			if (this.localState == RCVehicle.State.Mobilized)
			{
				this.motorLevel = Mathf.Max(Mathf.Max(Mathf.Abs(this.activeInput.joystick.y), Mathf.Abs(this.activeInput.joystick.x)), this.activeInput.trigger);
			}
			if (this.networkSync != null)
			{
				this.networkSync.syncedState.dataA = (byte)Mathf.Clamp(Mathf.FloorToInt(this.motorLevel * 255f), 0, 255);
			}
		}

		// Token: 0x06004D61 RID: 19809 RVA: 0x001794C0 File Offset: 0x001776C0
		protected override void RemoteUpdate(float dt)
		{
			base.RemoteUpdate(dt);
			if (this.localState == RCVehicle.State.Mobilized && this.networkSync != null)
			{
				this.motorLevel = Mathf.Clamp01((float)this.networkSync.syncedState.dataA / 255f);
			}
		}

		// Token: 0x06004D62 RID: 19810 RVA: 0x00179510 File Offset: 0x00177710
		protected override void SharedUpdate(float dt)
		{
			base.SharedUpdate(dt);
			switch (this.localState)
			{
			case RCVehicle.State.Disabled:
				break;
			case RCVehicle.State.DockedLeft:
			case RCVehicle.State.DockedRight:
				if (this.localStatePrev != RCVehicle.State.DockedLeft && this.localStatePrev != RCVehicle.State.DockedRight)
				{
					this.audioSource.GTStop();
					this.blimpDeflateBlendWeight = 0f;
					this.blimpMesh.SetBlendShapeWeight(0, 0f);
					this.crashCollider.enabled = false;
				}
				this.leftPropellerSpinRate = Mathf.MoveTowards(this.leftPropellerSpinRate, 0.6f, 6.6666665f * dt);
				this.rightPropellerSpinRate = Mathf.MoveTowards(this.rightPropellerSpinRate, 0.6f, 6.6666665f * dt);
				this.leftPropellerAngle += this.leftPropellerSpinRate * 360f * dt;
				this.rightPropellerAngle += this.rightPropellerSpinRate * 360f * dt;
				this.leftPropeller.transform.localRotation = Quaternion.Euler(new Vector3(this.leftPropellerAngle, 0f, -90f));
				this.rightPropeller.transform.localRotation = Quaternion.Euler(new Vector3(this.rightPropellerAngle, 0f, 90f));
				return;
			case RCVehicle.State.Mobilized:
			{
				if (this.localStatePrev != RCVehicle.State.Mobilized)
				{
					this.audioSource.loop = true;
					this.audioSource.clip = this.motorSound;
					this.audioSource.volume = 0f;
					this.audioSource.GTPlay();
					this.blimpDeflateBlendWeight = 0f;
					this.blimpMesh.SetBlendShapeWeight(0, 0f);
					this.crashCollider.enabled = false;
				}
				float target = Mathf.Lerp(this.motorSoundVolumeMinMax.x, this.motorSoundVolumeMinMax.y, this.motorLevel);
				this.audioSource.volume = Mathf.MoveTowards(this.audioSource.volume, target, this.motorSoundVolumeMinMax.y / this.motorVolumeRampTime * dt);
				this.blimpDeflateBlendWeight = 0f;
				float num = this.activeInput.joystick.y * 5f;
				float num2 = this.activeInput.joystick.x * 5f;
				float target2 = Mathf.Clamp(num2 + num + 0.6f, -5f, 5f);
				float target3 = Mathf.Clamp(-num2 + num + 0.6f, -5f, 5f);
				this.leftPropellerSpinRate = Mathf.MoveTowards(this.leftPropellerSpinRate, target2, 6.6666665f * dt);
				this.rightPropellerSpinRate = Mathf.MoveTowards(this.rightPropellerSpinRate, target3, 6.6666665f * dt);
				this.leftPropellerAngle += this.leftPropellerSpinRate * 360f * dt;
				this.rightPropellerAngle += this.rightPropellerSpinRate * 360f * dt;
				this.leftPropeller.transform.localRotation = Quaternion.Euler(new Vector3(this.leftPropellerAngle, 0f, -90f));
				this.rightPropeller.transform.localRotation = Quaternion.Euler(new Vector3(this.rightPropellerAngle, 0f, 90f));
				break;
			}
			case RCVehicle.State.Crashed:
				if (this.localStatePrev != RCVehicle.State.Crashed)
				{
					this.audioSource.GTStop();
					this.audioSource.clip = null;
					this.audioSource.loop = false;
					this.audioSource.volume = this.deflateSoundVolume;
					if (this.deflateSound != null)
					{
						this.audioSource.GTPlayOneShot(this.deflateSound, 1f);
					}
					this.leftPropellerSpinRate = 0f;
					this.rightPropellerSpinRate = 0f;
					this.leftPropellerAngle = 0f;
					this.rightPropellerAngle = 0f;
					this.leftPropeller.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -90f));
					this.rightPropeller.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
					this.crashCollider.enabled = true;
				}
				this.blimpDeflateBlendWeight = Mathf.Lerp(1f, this.blimpDeflateBlendWeight, Mathf.Exp(-this.deflateRate * dt));
				this.blimpMesh.SetBlendShapeWeight(0, this.blimpDeflateBlendWeight * 100f);
				return;
			default:
				return;
			}
		}

		// Token: 0x06004D63 RID: 19811 RVA: 0x00179960 File Offset: 0x00177B60
		private void FixedUpdate()
		{
			if (!base.HasLocalAuthority)
			{
				return;
			}
			float fixedDeltaTime = Time.fixedDeltaTime;
			float x = base.transform.lossyScale.x;
			if (this.localState == RCVehicle.State.Mobilized)
			{
				float num = this.maxAscendSpeed * x;
				float num2 = this.maxHorizontalSpeed * x;
				float d = this.ascendAccel * x;
				Vector3 velocity = this.rb.velocity;
				Vector3 normalized = new Vector3(base.transform.forward.x, 0f, base.transform.forward.z).normalized;
				this.turnAngle = Vector3.SignedAngle(Vector3.forward, normalized, Vector3.up);
				this.tiltAngle = Vector3.SignedAngle(normalized, base.transform.forward, base.transform.right);
				float target = this.activeInput.joystick.x * this.maxTurnRate;
				this.turnRate = Mathf.MoveTowards(this.turnRate, target, this.turnAccel * fixedDeltaTime);
				this.turnAngle += this.turnRate * fixedDeltaTime;
				float value = Vector3.Dot(normalized, velocity);
				float t = Mathf.InverseLerp(-num2, num2, value);
				float target2 = Mathf.Lerp(-this.maxHorizontalTiltAngle, this.maxHorizontalTiltAngle, t);
				this.tiltAngle = Mathf.MoveTowards(this.tiltAngle, target2, this.tiltAccel * fixedDeltaTime);
				base.transform.rotation = Quaternion.Euler(new Vector3(this.tiltAngle, this.turnAngle, 0f));
				Vector3 b = new Vector3(velocity.x, 0f, velocity.z);
				Vector3 a = Vector3.Lerp(normalized * this.activeInput.joystick.y * num2, b, Mathf.Exp(-this.horizontalAccelTime * fixedDeltaTime));
				this.rb.AddForce(a - b, ForceMode.VelocityChange);
				float num3 = this.activeInput.trigger * num;
				if (num3 > 0.01f && velocity.y < num3)
				{
					this.rb.AddForce(Vector3.up * d, ForceMode.Acceleration);
				}
				if (this.rb.useGravity)
				{
					RCVehicle.AddScaledGravityCompensationForce(this.rb, x, this.gravityCompensation);
					return;
				}
			}
			else if (this.localState == RCVehicle.State.Crashed && this.rb.useGravity)
			{
				RCVehicle.AddScaledGravityCompensationForce(this.rb, x, this.crashedGravityCompensation);
			}
		}

		// Token: 0x06004D64 RID: 19812 RVA: 0x00179BD0 File Offset: 0x00177DD0
		private void OnTriggerEnter(Collider other)
		{
			bool flag = other.gameObject.IsOnLayer(UnityLayer.GorillaThrowable);
			bool flag2 = other.gameObject.IsOnLayer(UnityLayer.GorillaHand);
			if (!other.isTrigger && base.HasLocalAuthority && this.localState == RCVehicle.State.Mobilized)
			{
				this.AuthorityBeginCrash();
				return;
			}
			if ((flag || flag2) && this.localState == RCVehicle.State.Mobilized)
			{
				Vector3 vector = Vector3.zero;
				if (flag2)
				{
					GorillaHandClimber component = other.gameObject.GetComponent<GorillaHandClimber>();
					if (component != null)
					{
						vector = ((component.xrNode == XRNode.LeftHand) ? GTPlayer.Instance.leftHandCenterVelocityTracker : GTPlayer.Instance.rightHandCenterVelocityTracker).GetAverageVelocity(true, 0.15f, false);
					}
				}
				else if (other.attachedRigidbody != null)
				{
					vector = other.attachedRigidbody.velocity;
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

		// Token: 0x04004FEB RID: 20459
		[SerializeField]
		private float maxAscendSpeed = 6f;

		// Token: 0x04004FEC RID: 20460
		[SerializeField]
		private float ascendAccelTime = 3f;

		// Token: 0x04004FED RID: 20461
		[SerializeField]
		private float gravityCompensation = 0.9f;

		// Token: 0x04004FEE RID: 20462
		[SerializeField]
		private float crashedGravityCompensation = 0.5f;

		// Token: 0x04004FEF RID: 20463
		[SerializeField]
		private float maxTurnRate = 90f;

		// Token: 0x04004FF0 RID: 20464
		[SerializeField]
		private float turnAccelTime = 0.75f;

		// Token: 0x04004FF1 RID: 20465
		[SerializeField]
		private float maxHorizontalSpeed = 6f;

		// Token: 0x04004FF2 RID: 20466
		[SerializeField]
		private float horizontalAccelTime = 2f;

		// Token: 0x04004FF3 RID: 20467
		[SerializeField]
		private float maxHorizontalTiltAngle = 45f;

		// Token: 0x04004FF4 RID: 20468
		[SerializeField]
		private float horizontalTiltTime = 2f;

		// Token: 0x04004FF5 RID: 20469
		[SerializeField]
		private Vector2 motorSoundVolumeMinMax = new Vector2(0.1f, 0.8f);

		// Token: 0x04004FF6 RID: 20470
		[SerializeField]
		private float deflateSoundVolume = 0.1f;

		// Token: 0x04004FF7 RID: 20471
		[SerializeField]
		private Collider crashCollider;

		// Token: 0x04004FF8 RID: 20472
		[SerializeField]
		private Transform leftPropeller;

		// Token: 0x04004FF9 RID: 20473
		[SerializeField]
		private Transform rightPropeller;

		// Token: 0x04004FFA RID: 20474
		[SerializeField]
		private SkinnedMeshRenderer blimpMesh;

		// Token: 0x04004FFB RID: 20475
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04004FFC RID: 20476
		[SerializeField]
		private AudioClip motorSound;

		// Token: 0x04004FFD RID: 20477
		[SerializeField]
		private AudioClip deflateSound;

		// Token: 0x04004FFE RID: 20478
		private float turnRate;

		// Token: 0x04004FFF RID: 20479
		private float turnAngle;

		// Token: 0x04005000 RID: 20480
		private float tiltAngle;

		// Token: 0x04005001 RID: 20481
		private float ascendAccel;

		// Token: 0x04005002 RID: 20482
		private float turnAccel;

		// Token: 0x04005003 RID: 20483
		private float tiltAccel;

		// Token: 0x04005004 RID: 20484
		private float horizontalAccel;

		// Token: 0x04005005 RID: 20485
		private float leftPropellerAngle;

		// Token: 0x04005006 RID: 20486
		private float rightPropellerAngle;

		// Token: 0x04005007 RID: 20487
		private float leftPropellerSpinRate;

		// Token: 0x04005008 RID: 20488
		private float rightPropellerSpinRate;

		// Token: 0x04005009 RID: 20489
		private float blimpDeflateBlendWeight;

		// Token: 0x0400500A RID: 20490
		private float deflateRate = Mathf.Exp(1f);

		// Token: 0x0400500B RID: 20491
		private const float propellerIdleAcc = 1f;

		// Token: 0x0400500C RID: 20492
		private const float propellerIdleSpinRate = 0.6f;

		// Token: 0x0400500D RID: 20493
		private const float propellerMaxAcc = 6.6666665f;

		// Token: 0x0400500E RID: 20494
		private const float propellerMaxSpinRate = 5f;

		// Token: 0x0400500F RID: 20495
		private float motorVolumeRampTime = 1f;

		// Token: 0x04005010 RID: 20496
		private float motorLevel;
	}
}
