using System;
using System.Collections.Generic;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C25 RID: 3109
	public class RCDragon : RCVehicle
	{
		// Token: 0x06004D77 RID: 19831 RVA: 0x0017A708 File Offset: 0x00178908
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

		// Token: 0x06004D78 RID: 19832 RVA: 0x0017A778 File Offset: 0x00178978
		protected override void Awake()
		{
			base.Awake();
			this.ascendAccel = this.maxAscendSpeed / this.ascendAccelTime;
			this.turnAccel = this.maxTurnRate / this.turnAccelTime;
			this.horizontalAccel = this.maxHorizontalSpeed / this.horizontalAccelTime;
			this.tiltAccel = this.maxHorizontalTiltAngle / this.horizontalTiltTime;
			this.shouldFlap = false;
			this.isFlapping = false;
			this.StopBreathFire();
			if (this.animation != null)
			{
				this.animation[this.wingFlapAnimName].speed = this.wingFlapAnimSpeed;
				this.animation[this.crashAnimName].speed = this.crashAnimSpeed;
				this.animation[this.mouthClosedAnimName].layer = 1;
				this.animation[this.mouthBreathFireAnimName].layer = 1;
			}
			this.nextFlapEventAnimTime = this.flapAnimEventTime;
		}

		// Token: 0x06004D79 RID: 19833 RVA: 0x0017A86B File Offset: 0x00178A6B
		protected override void OnDisable()
		{
			base.OnDisable();
			this.audioSource.GTStop();
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x0017A880 File Offset: 0x00178A80
		public void StartBreathFire()
		{
			if (!string.IsNullOrEmpty(this.mouthBreathFireAnimName))
			{
				this.animation.CrossFade(this.mouthBreathFireAnimName, 0.1f);
			}
			if (this.fireBreath != null)
			{
				this.fireBreath.SetActive(true);
			}
			this.PlayRandomSound(this.breathFireSound, this.breathFireVolume);
			this.fireBreathTimeRemaining = this.fireBreathDuration;
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x0017A8E8 File Offset: 0x00178AE8
		public void StopBreathFire()
		{
			if (!string.IsNullOrEmpty(this.mouthClosedAnimName))
			{
				this.animation.CrossFade(this.mouthClosedAnimName, 0.1f);
			}
			if (this.fireBreath != null)
			{
				this.fireBreath.SetActive(false);
			}
			this.fireBreathTimeRemaining = -1f;
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x0017A93D File Offset: 0x00178B3D
		public bool IsBreathingFire()
		{
			return this.fireBreathTimeRemaining >= 0f;
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x0017A94F File Offset: 0x00178B4F
		private void PlayRandomSound(List<AudioClip> clips, float volume)
		{
			if (clips == null || clips.Count == 0)
			{
				return;
			}
			this.PlaySound(clips[Random.Range(0, clips.Count)], volume);
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x0017A978 File Offset: 0x00178B78
		private void PlaySound(AudioClip clip, float volume)
		{
			if (this.audioSource == null || clip == null)
			{
				return;
			}
			this.audioSource.GTStop();
			this.audioSource.clip = null;
			this.audioSource.loop = false;
			this.audioSource.volume = volume;
			this.audioSource.GTPlayOneShot(clip, 1f);
		}

		// Token: 0x06004D7F RID: 19839 RVA: 0x0017A9E0 File Offset: 0x00178BE0
		protected override void AuthorityUpdate(float dt)
		{
			base.AuthorityUpdate(dt);
			this.motorLevel = 0f;
			if (this.localState == RCVehicle.State.Mobilized)
			{
				this.motorLevel = Mathf.Max(Mathf.Max(Mathf.Abs(this.activeInput.joystick.y), Mathf.Abs(this.activeInput.joystick.x)), this.activeInput.trigger);
				if (!this.IsBreathingFire() && this.activeInput.buttons > 0)
				{
					this.StartBreathFire();
				}
			}
			if (this.networkSync != null)
			{
				this.networkSync.syncedState.dataA = (byte)Mathf.Clamp(Mathf.FloorToInt(this.motorLevel * 255f), 0, 255);
				this.networkSync.syncedState.dataB = this.activeInput.buttons;
				this.networkSync.syncedState.dataC = (this.shouldFlap ? 1 : 0);
			}
		}

		// Token: 0x06004D80 RID: 19840 RVA: 0x0017AADC File Offset: 0x00178CDC
		protected override void RemoteUpdate(float dt)
		{
			base.RemoteUpdate(dt);
			if (this.localState == RCVehicle.State.Mobilized && this.networkSync != null)
			{
				this.motorLevel = Mathf.Clamp01((float)this.networkSync.syncedState.dataA / 255f);
				if (!this.IsBreathingFire() && this.networkSync.syncedState.dataB > 0)
				{
					this.StartBreathFire();
				}
				this.shouldFlap = (this.networkSync.syncedState.dataC > 0);
			}
		}

		// Token: 0x06004D81 RID: 19841 RVA: 0x0017AB64 File Offset: 0x00178D64
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
					if (this.crashCollider != null)
					{
						this.crashCollider.enabled = false;
					}
					if (this.animation != null)
					{
						this.animation.Play(this.dockedAnimName);
					}
					if (this.IsBreathingFire())
					{
						this.StopBreathFire();
						return;
					}
				}
				break;
			case RCVehicle.State.Mobilized:
			{
				if (this.localStatePrev != RCVehicle.State.Mobilized && this.crashCollider != null)
				{
					this.crashCollider.enabled = false;
				}
				if (this.animation != null)
				{
					if (!this.isFlapping && this.shouldFlap)
					{
						this.animation.CrossFade(this.wingFlapAnimName, 0.1f);
						this.nextFlapEventAnimTime = this.flapAnimEventTime;
					}
					else if (this.isFlapping && !this.shouldFlap)
					{
						this.animation.CrossFade(this.idleAnimName, 0.15f);
					}
					this.isFlapping = this.shouldFlap;
					if (this.isFlapping && !this.IsBreathingFire())
					{
						AnimationState animationState = this.animation[this.wingFlapAnimName];
						if (animationState.normalizedTime * animationState.length > this.nextFlapEventAnimTime)
						{
							this.PlayRandomSound(this.wingFlapSound, this.wingFlapVolume);
							this.nextFlapEventAnimTime = (Mathf.Floor(animationState.normalizedTime) + 1f) * animationState.length + this.flapAnimEventTime;
						}
					}
				}
				GTTime.TimeAsDouble();
				if (this.IsBreathingFire())
				{
					this.fireBreathTimeRemaining -= dt;
					if (this.fireBreathTimeRemaining <= 0f)
					{
						this.StopBreathFire();
					}
				}
				float target = Mathf.Lerp(this.motorSoundVolumeMinMax.x, this.motorSoundVolumeMinMax.y, this.motorLevel);
				this.audioSource.volume = Mathf.MoveTowards(this.audioSource.volume, target, this.motorSoundVolumeMinMax.y / this.motorVolumeRampTime * dt);
				break;
			}
			case RCVehicle.State.Crashed:
				if (this.localStatePrev != RCVehicle.State.Crashed)
				{
					this.PlaySound(this.crashSound, this.crashSoundVolume);
					if (this.crashCollider != null)
					{
						this.crashCollider.enabled = true;
					}
					if (this.animation != null)
					{
						this.animation.CrossFade(this.crashAnimName, 0.05f);
					}
					if (this.IsBreathingFire())
					{
						this.StopBreathFire();
						return;
					}
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x0017AE00 File Offset: 0x00179000
		private void FixedUpdate()
		{
			if (!base.HasLocalAuthority)
			{
				return;
			}
			float x = base.transform.lossyScale.x;
			float fixedDeltaTime = Time.fixedDeltaTime;
			this.shouldFlap = false;
			if (this.localState == RCVehicle.State.Mobilized)
			{
				float num = this.maxAscendSpeed * x;
				float num2 = this.maxHorizontalSpeed * x;
				float d = this.ascendAccel * x;
				float d2 = this.ascendWhileFlyingAccelBoost * x;
				float num3 = 0.5f * x;
				float num4 = 45f;
				Vector3 velocity = this.rb.velocity;
				Vector3 normalized = new Vector3(base.transform.forward.x, 0f, base.transform.forward.z).normalized;
				this.turnAngle = Vector3.SignedAngle(Vector3.forward, normalized, Vector3.up);
				this.tiltAngle = Vector3.SignedAngle(normalized, base.transform.forward, base.transform.right);
				float target = this.activeInput.joystick.x * this.maxTurnRate;
				this.turnRate = Mathf.MoveTowards(this.turnRate, target, this.turnAccel * fixedDeltaTime);
				this.turnAngle += this.turnRate * fixedDeltaTime;
				float num5 = Vector3.Dot(normalized, velocity);
				float t = Mathf.InverseLerp(-num2, num2, num5);
				float target2 = Mathf.Lerp(-this.maxHorizontalTiltAngle, this.maxHorizontalTiltAngle, t);
				this.tiltAngle = Mathf.MoveTowards(this.tiltAngle, target2, this.tiltAccel * fixedDeltaTime);
				base.transform.rotation = Quaternion.Euler(new Vector3(this.tiltAngle, this.turnAngle, 0f));
				Vector3 b = new Vector3(velocity.x, 0f, velocity.z);
				Vector3 a = Vector3.Lerp(normalized * this.activeInput.joystick.y * num2, b, Mathf.Exp(-this.horizontalAccelTime * fixedDeltaTime));
				this.rb.AddForce(a - b, ForceMode.VelocityChange);
				float num6 = this.activeInput.trigger * num;
				if (num6 > 0.01f && velocity.y < num6)
				{
					this.rb.AddForce(Vector3.up * d, ForceMode.Acceleration);
				}
				bool flag = Mathf.Abs(num5) > num3;
				bool flag2 = Mathf.Abs(this.turnRate) > num4;
				if (flag || flag2)
				{
					this.rb.AddForce(Vector3.up * d2, ForceMode.Acceleration);
				}
				this.shouldFlap = (num6 > 0.01f || flag || flag2);
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

		// Token: 0x06004D83 RID: 19843 RVA: 0x0017B0E4 File Offset: 0x001792E4
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

		// Token: 0x0400502B RID: 20523
		[SerializeField]
		private float maxAscendSpeed = 6f;

		// Token: 0x0400502C RID: 20524
		[SerializeField]
		private float ascendAccelTime = 3f;

		// Token: 0x0400502D RID: 20525
		[SerializeField]
		private float ascendWhileFlyingAccelBoost;

		// Token: 0x0400502E RID: 20526
		[SerializeField]
		private float gravityCompensation = 0.9f;

		// Token: 0x0400502F RID: 20527
		[SerializeField]
		private float crashedGravityCompensation = 0.5f;

		// Token: 0x04005030 RID: 20528
		[SerializeField]
		private float maxTurnRate = 90f;

		// Token: 0x04005031 RID: 20529
		[SerializeField]
		private float turnAccelTime = 0.75f;

		// Token: 0x04005032 RID: 20530
		[SerializeField]
		private float maxHorizontalSpeed = 6f;

		// Token: 0x04005033 RID: 20531
		[SerializeField]
		private float horizontalAccelTime = 2f;

		// Token: 0x04005034 RID: 20532
		[SerializeField]
		private float maxHorizontalTiltAngle = 45f;

		// Token: 0x04005035 RID: 20533
		[SerializeField]
		private float horizontalTiltTime = 2f;

		// Token: 0x04005036 RID: 20534
		[SerializeField]
		private Vector2 motorSoundVolumeMinMax = new Vector2(0.1f, 0.8f);

		// Token: 0x04005037 RID: 20535
		[SerializeField]
		private float crashSoundVolume = 0.1f;

		// Token: 0x04005038 RID: 20536
		[SerializeField]
		private float breathFireVolume = 0.5f;

		// Token: 0x04005039 RID: 20537
		[SerializeField]
		private float wingFlapVolume = 0.1f;

		// Token: 0x0400503A RID: 20538
		[SerializeField]
		private Animation animation;

		// Token: 0x0400503B RID: 20539
		[SerializeField]
		private string wingFlapAnimName;

		// Token: 0x0400503C RID: 20540
		[SerializeField]
		private float wingFlapAnimSpeed = 1f;

		// Token: 0x0400503D RID: 20541
		[SerializeField]
		private string dockedAnimName;

		// Token: 0x0400503E RID: 20542
		[SerializeField]
		private string idleAnimName;

		// Token: 0x0400503F RID: 20543
		[SerializeField]
		private string crashAnimName;

		// Token: 0x04005040 RID: 20544
		[SerializeField]
		private float crashAnimSpeed = 1f;

		// Token: 0x04005041 RID: 20545
		[SerializeField]
		private string mouthClosedAnimName;

		// Token: 0x04005042 RID: 20546
		[SerializeField]
		private string mouthBreathFireAnimName;

		// Token: 0x04005043 RID: 20547
		private bool shouldFlap;

		// Token: 0x04005044 RID: 20548
		private bool isFlapping;

		// Token: 0x04005045 RID: 20549
		private float nextFlapEventAnimTime;

		// Token: 0x04005046 RID: 20550
		[SerializeField]
		private float flapAnimEventTime = 0.25f;

		// Token: 0x04005047 RID: 20551
		[SerializeField]
		private GameObject fireBreath;

		// Token: 0x04005048 RID: 20552
		[SerializeField]
		private float fireBreathDuration;

		// Token: 0x04005049 RID: 20553
		private float fireBreathTimeRemaining;

		// Token: 0x0400504A RID: 20554
		[SerializeField]
		private Collider crashCollider;

		// Token: 0x0400504B RID: 20555
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x0400504C RID: 20556
		[SerializeField]
		private List<AudioClip> breathFireSound;

		// Token: 0x0400504D RID: 20557
		[SerializeField]
		private List<AudioClip> wingFlapSound;

		// Token: 0x0400504E RID: 20558
		[SerializeField]
		private AudioClip crashSound;

		// Token: 0x0400504F RID: 20559
		private float turnRate;

		// Token: 0x04005050 RID: 20560
		private float turnAngle;

		// Token: 0x04005051 RID: 20561
		private float tiltAngle;

		// Token: 0x04005052 RID: 20562
		private float ascendAccel;

		// Token: 0x04005053 RID: 20563
		private float turnAccel;

		// Token: 0x04005054 RID: 20564
		private float tiltAccel;

		// Token: 0x04005055 RID: 20565
		private float horizontalAccel;

		// Token: 0x04005056 RID: 20566
		private float motorVolumeRampTime = 1f;

		// Token: 0x04005057 RID: 20567
		private float motorLevel;
	}
}
