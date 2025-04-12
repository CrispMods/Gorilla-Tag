using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C55 RID: 3157
	[RequireComponent(typeof(TransferrableObject))]
	public class ProjectileShooterCosmetic : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06004EAA RID: 20138 RVA: 0x000629D4 File Offset: 0x00060BD4
		// (set) Token: 0x06004EAB RID: 20139 RVA: 0x000629DC File Offset: 0x00060BDC
		public bool TickRunning { get; set; }

		// Token: 0x06004EAC RID: 20140 RVA: 0x001B22F8 File Offset: 0x001B04F8
		private void Awake()
		{
			this.transferrableObject = base.GetComponent<TransferrableObject>();
			this.projectileHash = PoolUtils.GameObjHashCode(this.projectilePrefab);
			this.launchedTime = 0f;
			this.canShoot = true;
			this.pressCounter = 0f;
			this.chargeShader = base.GetComponent<ProjectileChargeShader>();
		}

		// Token: 0x06004EAD RID: 20141 RVA: 0x000629E5 File Offset: 0x00060BE5
		private void OnEnable()
		{
			TickSystem<object>.AddTickCallback(this);
			if (this.chargeShader)
			{
				this.chargeShader.UpdateChargeProgress(0f);
			}
		}

		// Token: 0x06004EAE RID: 20142 RVA: 0x001B234C File Offset: 0x001B054C
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

		// Token: 0x06004EAF RID: 20143 RVA: 0x001B2444 File Offset: 0x001B0644
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

		// Token: 0x06004EB0 RID: 20144 RVA: 0x001B24F4 File Offset: 0x001B06F4
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

		// Token: 0x06004EB1 RID: 20145 RVA: 0x001B258C File Offset: 0x001B078C
		private float GetLaunchSpeed()
		{
			if (!this.useButtonPressDurationAsVelocityModifier)
			{
				return 1f;
			}
			return Mathf.Lerp(this.launchMinSpeed, this.launchMaxSpeed, Mathf.InverseLerp(0f, this.maxButtonPressDuration, Mathf.Clamp(this.pressCounter, 0f, this.maxButtonPressDuration)));
		}

		// Token: 0x06004EB2 RID: 20146 RVA: 0x001B25E0 File Offset: 0x001B07E0
		private float GetChargeRate()
		{
			if (!this.useButtonPressDurationAsVelocityModifier)
			{
				return (float)this.chargeShader.shaderAnimSteps;
			}
			return Mathf.Lerp(0f, (float)this.chargeShader.shaderAnimSteps, Mathf.InverseLerp(0f, this.maxButtonPressDuration, Mathf.Clamp(this.pressCounter, 0f, this.maxButtonPressDuration)));
		}

		// Token: 0x06004EB3 RID: 20147 RVA: 0x00062A0A File Offset: 0x00060C0A
		private void TriggerShoot()
		{
			this.Shoot();
			this.pressCounter = 0f;
		}

		// Token: 0x06004EB4 RID: 20148 RVA: 0x00062A1D File Offset: 0x00060C1D
		public void OnButtonPressed()
		{
			this.pressStarted = true;
			if (this.launchActivatorType == ProjectileShooterCosmetic.LaunchActivator.ButtonPressed)
			{
				this.TriggerShoot();
			}
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x00062A35 File Offset: 0x00060C35
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

		// Token: 0x0400523C RID: 21052
		[SerializeField]
		private float cooldown;

		// Token: 0x0400523D RID: 21053
		[SerializeField]
		private GameObject projectilePrefab;

		// Token: 0x0400523E RID: 21054
		[SerializeField]
		private ParticleSystem launchParticles;

		// Token: 0x0400523F RID: 21055
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04005240 RID: 21056
		[SerializeField]
		private SoundBankPlayer shootSoundBank;

		// Token: 0x04005241 RID: 21057
		[SerializeField]
		private SoundBankPlayer readyToShootSoundBank;

		// Token: 0x04005242 RID: 21058
		[SerializeField]
		private SoundBankPlayer chargingSoundBank;

		// Token: 0x04005243 RID: 21059
		[SerializeField]
		private float launchMinSpeed;

		// Token: 0x04005244 RID: 21060
		[SerializeField]
		private float launchMaxSpeed;

		// Token: 0x04005245 RID: 21061
		[SerializeField]
		private Transform launchPosition;

		// Token: 0x04005246 RID: 21062
		[SerializeField]
		private ProjectileShooterCosmetic.LaunchActivator launchActivatorType;

		// Token: 0x04005247 RID: 21063
		[SerializeField]
		private bool useButtonPressDurationAsVelocityModifier;

		// Token: 0x04005248 RID: 21064
		[SerializeField]
		private float maxButtonPressDuration = 200f;

		// Token: 0x04005249 RID: 21065
		public UnityEvent<bool, float> onOwnerLaunchProjectile;

		// Token: 0x0400524A RID: 21066
		private int projectileHash;

		// Token: 0x0400524B RID: 21067
		private float launchedTime;

		// Token: 0x0400524C RID: 21068
		private bool canShoot;

		// Token: 0x0400524D RID: 21069
		private float pressStartedTime;

		// Token: 0x0400524E RID: 21070
		private bool pressStarted;

		// Token: 0x0400524F RID: 21071
		private float pressCounter;

		// Token: 0x04005250 RID: 21072
		private TransferrableObject transferrableObject;

		// Token: 0x04005251 RID: 21073
		private ProjectileChargeShader chargeShader;

		// Token: 0x02000C56 RID: 3158
		private enum LaunchActivator
		{
			// Token: 0x04005254 RID: 21076
			ButtonReleased,
			// Token: 0x04005255 RID: 21077
			ButtonPressed,
			// Token: 0x04005256 RID: 21078
			ButtonStayed
		}
	}
}
