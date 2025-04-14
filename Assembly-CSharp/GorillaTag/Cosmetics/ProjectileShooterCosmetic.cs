using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C52 RID: 3154
	[RequireComponent(typeof(TransferrableObject))]
	public class ProjectileShooterCosmetic : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06004E9E RID: 20126 RVA: 0x001820A1 File Offset: 0x001802A1
		// (set) Token: 0x06004E9F RID: 20127 RVA: 0x001820A9 File Offset: 0x001802A9
		public bool TickRunning { get; set; }

		// Token: 0x06004EA0 RID: 20128 RVA: 0x001820B4 File Offset: 0x001802B4
		private void Awake()
		{
			this.transferrableObject = base.GetComponent<TransferrableObject>();
			this.projectileHash = PoolUtils.GameObjHashCode(this.projectilePrefab);
			this.launchedTime = 0f;
			this.canShoot = true;
			this.pressCounter = 0f;
			this.chargeShader = base.GetComponent<ProjectileChargeShader>();
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x00182107 File Offset: 0x00180307
		private void OnEnable()
		{
			TickSystem<object>.AddTickCallback(this);
			if (this.chargeShader)
			{
				this.chargeShader.UpdateChargeProgress(0f);
			}
		}

		// Token: 0x06004EA2 RID: 20130 RVA: 0x0018212C File Offset: 0x0018032C
		public void Tick()
		{
			if (!this.canShoot && Time.time - this.launchedTime >= this.cooldown)
			{
				this.canShoot = true;
				if (this.audioSource && this.readyToShootSoundBank != null)
				{
					this.readyToShootSoundBank.Play();
				}
				this.pressCounter = 0f;
			}
			if (this.pressStarted && this.canShoot)
			{
				float num = this.pressCounter + 1f;
				this.pressCounter = num;
				if (num <= this.maxButtonPressDuration)
				{
					this.pressCounter += 1f;
					if (this.audioSource && this.chargingSoundBank != null && !this.chargingSoundBank.isPlaying)
					{
						this.chargingSoundBank.Play();
					}
				}
				if (this.chargeShader)
				{
					this.chargeShader.UpdateChargeProgress(this.GetChargeRate());
				}
			}
		}

		// Token: 0x06004EA3 RID: 20131 RVA: 0x00182224 File Offset: 0x00180424
		private void Shoot()
		{
			if (!this.canShoot)
			{
				return;
			}
			Vector3 velocity = this.launchPosition.forward * this.GetLaunchSpeed() * this.transferrableObject.ownerRig.scaleFactor;
			this.LaunchProjectileLocal(this.launchPosition.position, this.launchPosition.rotation, velocity, this.transferrableObject.ownerRig.scaleFactor);
			this.launchedTime = Time.time;
			this.canShoot = false;
			if (this.transferrableObject.IsMyItem())
			{
				UnityEvent<bool, float> unityEvent = this.onOwnerLaunchProjectile;
				if (unityEvent == null)
				{
					return;
				}
				unityEvent.Invoke(this.transferrableObject.InLeftHand(), this.GetLaunchSpeed());
			}
		}

		// Token: 0x06004EA4 RID: 20132 RVA: 0x001822D4 File Offset: 0x001804D4
		private void LaunchProjectileLocal(Vector3 startPos, Quaternion rotation, Vector3 velocity, float playerScale)
		{
			GameObject gameObject = ObjectPools.instance.Instantiate(this.projectileHash);
			gameObject.transform.localScale = Vector3.one * playerScale;
			IProjectile component = gameObject.GetComponent<IProjectile>();
			if (component != null)
			{
				component.Launch(startPos, rotation, velocity, playerScale);
			}
			if (this.audioSource && this.shootSoundBank != null)
			{
				this.shootSoundBank.audioSource.Stop();
				this.shootSoundBank.Play();
			}
			if (this.launchParticles)
			{
				this.launchParticles.Play();
			}
		}

		// Token: 0x06004EA5 RID: 20133 RVA: 0x0018236C File Offset: 0x0018056C
		private float GetLaunchSpeed()
		{
			if (!this.useButtonPressDurationAsVelocityModifier)
			{
				return 1f;
			}
			return Mathf.Lerp(this.launchMinSpeed, this.launchMaxSpeed, Mathf.InverseLerp(0f, this.maxButtonPressDuration, Mathf.Clamp(this.pressCounter, 0f, this.maxButtonPressDuration)));
		}

		// Token: 0x06004EA6 RID: 20134 RVA: 0x001823C0 File Offset: 0x001805C0
		private float GetChargeRate()
		{
			if (!this.useButtonPressDurationAsVelocityModifier)
			{
				return (float)this.chargeShader.shaderAnimSteps;
			}
			return Mathf.Lerp(0f, (float)this.chargeShader.shaderAnimSteps, Mathf.InverseLerp(0f, this.maxButtonPressDuration, Mathf.Clamp(this.pressCounter, 0f, this.maxButtonPressDuration)));
		}

		// Token: 0x06004EA7 RID: 20135 RVA: 0x0018241E File Offset: 0x0018061E
		private void TriggerShoot()
		{
			this.Shoot();
			this.pressCounter = 0f;
		}

		// Token: 0x06004EA8 RID: 20136 RVA: 0x00182431 File Offset: 0x00180631
		public void OnButtonPressed()
		{
			this.pressStarted = true;
			if (this.launchActivatorType == ProjectileShooterCosmetic.LaunchActivator.ButtonPressed)
			{
				this.TriggerShoot();
			}
		}

		// Token: 0x06004EA9 RID: 20137 RVA: 0x00182449 File Offset: 0x00180649
		public void OnButtonReleased()
		{
			this.pressStarted = false;
			if (this.chargeShader)
			{
				this.chargeShader.UpdateChargeProgress(0f);
			}
			if (this.launchActivatorType == ProjectileShooterCosmetic.LaunchActivator.ButtonReleased)
			{
				this.TriggerShoot();
			}
		}

		// Token: 0x0400522A RID: 21034
		[SerializeField]
		private float cooldown;

		// Token: 0x0400522B RID: 21035
		[SerializeField]
		private GameObject projectilePrefab;

		// Token: 0x0400522C RID: 21036
		[SerializeField]
		private ParticleSystem launchParticles;

		// Token: 0x0400522D RID: 21037
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x0400522E RID: 21038
		[SerializeField]
		private SoundBankPlayer shootSoundBank;

		// Token: 0x0400522F RID: 21039
		[SerializeField]
		private SoundBankPlayer readyToShootSoundBank;

		// Token: 0x04005230 RID: 21040
		[SerializeField]
		private SoundBankPlayer chargingSoundBank;

		// Token: 0x04005231 RID: 21041
		[SerializeField]
		private float launchMinSpeed;

		// Token: 0x04005232 RID: 21042
		[SerializeField]
		private float launchMaxSpeed;

		// Token: 0x04005233 RID: 21043
		[SerializeField]
		private Transform launchPosition;

		// Token: 0x04005234 RID: 21044
		[SerializeField]
		private ProjectileShooterCosmetic.LaunchActivator launchActivatorType;

		// Token: 0x04005235 RID: 21045
		[SerializeField]
		private bool useButtonPressDurationAsVelocityModifier;

		// Token: 0x04005236 RID: 21046
		[SerializeField]
		private float maxButtonPressDuration = 200f;

		// Token: 0x04005237 RID: 21047
		public UnityEvent<bool, float> onOwnerLaunchProjectile;

		// Token: 0x04005238 RID: 21048
		private int projectileHash;

		// Token: 0x04005239 RID: 21049
		private float launchedTime;

		// Token: 0x0400523A RID: 21050
		private bool canShoot;

		// Token: 0x0400523B RID: 21051
		private float pressStartedTime;

		// Token: 0x0400523C RID: 21052
		private bool pressStarted;

		// Token: 0x0400523D RID: 21053
		private float pressCounter;

		// Token: 0x0400523E RID: 21054
		private TransferrableObject transferrableObject;

		// Token: 0x0400523F RID: 21055
		private ProjectileChargeShader chargeShader;

		// Token: 0x02000C53 RID: 3155
		private enum LaunchActivator
		{
			// Token: 0x04005242 RID: 21058
			ButtonReleased,
			// Token: 0x04005243 RID: 21059
			ButtonPressed,
			// Token: 0x04005244 RID: 21060
			ButtonStayed
		}
	}
}
