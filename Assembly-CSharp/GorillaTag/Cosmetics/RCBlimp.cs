using System;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C4D RID: 3149
	public class RCBlimp : RCVehicle
	{
		// Token: 0x06004EAE RID: 20142 RVA: 0x001B1BF0 File Offset: 0x001AFDF0
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

		// Token: 0x06004EAF RID: 20143 RVA: 0x001B1C60 File Offset: 0x001AFE60
		protected override void Awake()
		{
			base.Awake();
			this.ascendAccel = this.maxAscendSpeed / this.ascendAccelTime;
			this.turnAccel = this.maxTurnRate / this.turnAccelTime;
			this.horizontalAccel = this.maxHorizontalSpeed / this.horizontalAccelTime;
			this.tiltAccel = this.maxHorizontalTiltAngle / this.horizontalTiltTime;
		}

		// Token: 0x06004EB0 RID: 20144 RVA: 0x00063747 File Offset: 0x00061947
		protected override void OnDisable()
		{
			base.OnDisable();
			this.audioSource.GTStop();
		}

		// Token: 0x06004EB1 RID: 20145 RVA: 0x001B1CC0 File Offset: 0x001AFEC0
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

		// Token: 0x06004EB2 RID: 20146 RVA: 0x001B1D68 File Offset: 0x001AFF68
		protected override void RemoteUpdate(float dt)
		{
			base.RemoteUpdate(dt);
			if (this.localState == RCVehicle.State.Mobilized && this.networkSync != null)
			{
				this.motorLevel = Mathf.Clamp01((float)this.networkSync.syncedState.dataA / 255f);
			}
		}

		// Token: 0x06004EB3 RID: 20147 RVA: 0x001B1DB8 File Offset: 0x001AFFB8
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

		// Token: 0x06004EB4 RID: 20148 RVA: 0x001B2208 File Offset: 0x001B0408
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

		// Token: 0x06004EB5 RID: 20149 RVA: 0x001B2478 File Offset: 0x001B0678
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

		// Token: 0x040050E1 RID: 20705
		[SerializeField]
		private float maxAscendSpeed = 6f;

		// Token: 0x040050E2 RID: 20706
		[SerializeField]
		private float ascendAccelTime = 3f;

		// Token: 0x040050E3 RID: 20707
		[SerializeField]
		private float gravityCompensation = 0.9f;

		// Token: 0x040050E4 RID: 20708
		[SerializeField]
		private float crashedGravityCompensation = 0.5f;

		// Token: 0x040050E5 RID: 20709
		[SerializeField]
		private float maxTurnRate = 90f;

		// Token: 0x040050E6 RID: 20710
		[SerializeField]
		private float turnAccelTime = 0.75f;

		// Token: 0x040050E7 RID: 20711
		[SerializeField]
		private float maxHorizontalSpeed = 6f;

		// Token: 0x040050E8 RID: 20712
		[SerializeField]
		private float horizontalAccelTime = 2f;

		// Token: 0x040050E9 RID: 20713
		[SerializeField]
		private float maxHorizontalTiltAngle = 45f;

		// Token: 0x040050EA RID: 20714
		[SerializeField]
		private float horizontalTiltTime = 2f;

		// Token: 0x040050EB RID: 20715
		[SerializeField]
		private Vector2 motorSoundVolumeMinMax = new Vector2(0.1f, 0.8f);

		// Token: 0x040050EC RID: 20716
		[SerializeField]
		private float deflateSoundVolume = 0.1f;

		// Token: 0x040050ED RID: 20717
		[SerializeField]
		private Collider crashCollider;

		// Token: 0x040050EE RID: 20718
		[SerializeField]
		private Transform leftPropeller;

		// Token: 0x040050EF RID: 20719
		[SerializeField]
		private Transform rightPropeller;

		// Token: 0x040050F0 RID: 20720
		[SerializeField]
		private SkinnedMeshRenderer blimpMesh;

		// Token: 0x040050F1 RID: 20721
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x040050F2 RID: 20722
		[SerializeField]
		private AudioClip motorSound;

		// Token: 0x040050F3 RID: 20723
		[SerializeField]
		private AudioClip deflateSound;

		// Token: 0x040050F4 RID: 20724
		private float turnRate;

		// Token: 0x040050F5 RID: 20725
		private float turnAngle;

		// Token: 0x040050F6 RID: 20726
		private float tiltAngle;

		// Token: 0x040050F7 RID: 20727
		private float ascendAccel;

		// Token: 0x040050F8 RID: 20728
		private float turnAccel;

		// Token: 0x040050F9 RID: 20729
		private float tiltAccel;

		// Token: 0x040050FA RID: 20730
		private float horizontalAccel;

		// Token: 0x040050FB RID: 20731
		private float leftPropellerAngle;

		// Token: 0x040050FC RID: 20732
		private float rightPropellerAngle;

		// Token: 0x040050FD RID: 20733
		private float leftPropellerSpinRate;

		// Token: 0x040050FE RID: 20734
		private float rightPropellerSpinRate;

		// Token: 0x040050FF RID: 20735
		private float blimpDeflateBlendWeight;

		// Token: 0x04005100 RID: 20736
		private float deflateRate = Mathf.Exp(1f);

		// Token: 0x04005101 RID: 20737
		private const float propellerIdleAcc = 1f;

		// Token: 0x04005102 RID: 20738
		private const float propellerIdleSpinRate = 0.6f;

		// Token: 0x04005103 RID: 20739
		private const float propellerMaxAcc = 6.6666665f;

		// Token: 0x04005104 RID: 20740
		private const float propellerMaxSpinRate = 5f;

		// Token: 0x04005105 RID: 20741
		private float motorVolumeRampTime = 1f;

		// Token: 0x04005106 RID: 20742
		private float motorLevel;
	}
}
