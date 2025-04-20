using System;
using System.Collections.Generic;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C50 RID: 3152
	public class RCDragon : RCVehicle
	{
		// Token: 0x06004EBC RID: 20156 RVA: 0x001B29C0 File Offset: 0x001B0BC0
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

		// Token: 0x06004EBD RID: 20157 RVA: 0x001B2A30 File Offset: 0x001B0C30
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

		// Token: 0x06004EBE RID: 20158 RVA: 0x00063781 File Offset: 0x00061981
		protected override void OnDisable()
		{
			base.OnDisable();
			this.audioSource.GTStop();
		}

		// Token: 0x06004EBF RID: 20159 RVA: 0x001B2B24 File Offset: 0x001B0D24
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

		// Token: 0x06004EC0 RID: 20160 RVA: 0x001B2B8C File Offset: 0x001B0D8C
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

		// Token: 0x06004EC1 RID: 20161 RVA: 0x00063794 File Offset: 0x00061994
		public bool IsBreathingFire()
		{
			return this.fireBreathTimeRemaining >= 0f;
		}

		// Token: 0x06004EC2 RID: 20162 RVA: 0x000637A6 File Offset: 0x000619A6
		private void PlayRandomSound(List<AudioClip> clips, float volume)
		{
			if (clips == null || clips.Count == 0)
			{
				return;
			}
			this.PlaySound(clips[UnityEngine.Random.Range(0, clips.Count)], volume);
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x001B2BE4 File Offset: 0x001B0DE4
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

		// Token: 0x06004EC4 RID: 20164 RVA: 0x001B2C4C File Offset: 0x001B0E4C
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

		// Token: 0x06004EC5 RID: 20165 RVA: 0x001B2D48 File Offset: 0x001B0F48
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

		// Token: 0x06004EC6 RID: 20166 RVA: 0x001B2DD0 File Offset: 0x001B0FD0
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

		// Token: 0x06004EC7 RID: 20167 RVA: 0x001B306C File Offset: 0x001B126C
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

		// Token: 0x06004EC8 RID: 20168 RVA: 0x001B2478 File Offset: 0x001B0678
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

		// Token: 0x0400510F RID: 20751
		[SerializeField]
		private float maxAscendSpeed = 6f;

		// Token: 0x04005110 RID: 20752
		[SerializeField]
		private float ascendAccelTime = 3f;

		// Token: 0x04005111 RID: 20753
		[SerializeField]
		private float ascendWhileFlyingAccelBoost;

		// Token: 0x04005112 RID: 20754
		[SerializeField]
		private float gravityCompensation = 0.9f;

		// Token: 0x04005113 RID: 20755
		[SerializeField]
		private float crashedGravityCompensation = 0.5f;

		// Token: 0x04005114 RID: 20756
		[SerializeField]
		private float maxTurnRate = 90f;

		// Token: 0x04005115 RID: 20757
		[SerializeField]
		private float turnAccelTime = 0.75f;

		// Token: 0x04005116 RID: 20758
		[SerializeField]
		private float maxHorizontalSpeed = 6f;

		// Token: 0x04005117 RID: 20759
		[SerializeField]
		private float horizontalAccelTime = 2f;

		// Token: 0x04005118 RID: 20760
		[SerializeField]
		private float maxHorizontalTiltAngle = 45f;

		// Token: 0x04005119 RID: 20761
		[SerializeField]
		private float horizontalTiltTime = 2f;

		// Token: 0x0400511A RID: 20762
		[SerializeField]
		private Vector2 motorSoundVolumeMinMax = new Vector2(0.1f, 0.8f);

		// Token: 0x0400511B RID: 20763
		[SerializeField]
		private float crashSoundVolume = 0.1f;

		// Token: 0x0400511C RID: 20764
		[SerializeField]
		private float breathFireVolume = 0.5f;

		// Token: 0x0400511D RID: 20765
		[SerializeField]
		private float wingFlapVolume = 0.1f;

		// Token: 0x0400511E RID: 20766
		[SerializeField]
		private Animation animation;

		// Token: 0x0400511F RID: 20767
		[SerializeField]
		private string wingFlapAnimName;

		// Token: 0x04005120 RID: 20768
		[SerializeField]
		private float wingFlapAnimSpeed = 1f;

		// Token: 0x04005121 RID: 20769
		[SerializeField]
		private string dockedAnimName;

		// Token: 0x04005122 RID: 20770
		[SerializeField]
		private string idleAnimName;

		// Token: 0x04005123 RID: 20771
		[SerializeField]
		private string crashAnimName;

		// Token: 0x04005124 RID: 20772
		[SerializeField]
		private float crashAnimSpeed = 1f;

		// Token: 0x04005125 RID: 20773
		[SerializeField]
		private string mouthClosedAnimName;

		// Token: 0x04005126 RID: 20774
		[SerializeField]
		private string mouthBreathFireAnimName;

		// Token: 0x04005127 RID: 20775
		private bool shouldFlap;

		// Token: 0x04005128 RID: 20776
		private bool isFlapping;

		// Token: 0x04005129 RID: 20777
		private float nextFlapEventAnimTime;

		// Token: 0x0400512A RID: 20778
		[SerializeField]
		private float flapAnimEventTime = 0.25f;

		// Token: 0x0400512B RID: 20779
		[SerializeField]
		private GameObject fireBreath;

		// Token: 0x0400512C RID: 20780
		[SerializeField]
		private float fireBreathDuration;

		// Token: 0x0400512D RID: 20781
		private float fireBreathTimeRemaining;

		// Token: 0x0400512E RID: 20782
		[SerializeField]
		private Collider crashCollider;

		// Token: 0x0400512F RID: 20783
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04005130 RID: 20784
		[SerializeField]
		private List<AudioClip> breathFireSound;

		// Token: 0x04005131 RID: 20785
		[SerializeField]
		private List<AudioClip> wingFlapSound;

		// Token: 0x04005132 RID: 20786
		[SerializeField]
		private AudioClip crashSound;

		// Token: 0x04005133 RID: 20787
		private float turnRate;

		// Token: 0x04005134 RID: 20788
		private float turnAngle;

		// Token: 0x04005135 RID: 20789
		private float tiltAngle;

		// Token: 0x04005136 RID: 20790
		private float ascendAccel;

		// Token: 0x04005137 RID: 20791
		private float turnAccel;

		// Token: 0x04005138 RID: 20792
		private float tiltAccel;

		// Token: 0x04005139 RID: 20793
		private float horizontalAccel;

		// Token: 0x0400513A RID: 20794
		private float motorVolumeRampTime = 1f;

		// Token: 0x0400513B RID: 20795
		private float motorLevel;
	}
}
