using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C83 RID: 3203
	[RequireComponent(typeof(TransferrableObject))]
	public class ProjectileShooterCosmetic : MonoBehaviour, ITickSystemTick
	{
		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06004FFE RID: 20478 RVA: 0x000643F9 File Offset: 0x000625F9
		// (set) Token: 0x06004FFF RID: 20479 RVA: 0x00064401 File Offset: 0x00062601
		public bool TickRunning { get; set; }

		// Token: 0x06005000 RID: 20480 RVA: 0x001BA3DC File Offset: 0x001B85DC
		private void Awake()
		{
			this.transferrableObject = base.GetComponent<TransferrableObject>();
			this.projectileHash = PoolUtils.GameObjHashCode(this.projectilePrefab);
			this.launchedTime = 0f;
			this.canShoot = true;
			this.pressCounter = 0f;
			this.chargeShader = base.GetComponent<ProjectileChargeShader>();
		}

		// Token: 0x06005001 RID: 20481 RVA: 0x0006440A File Offset: 0x0006260A
		private void OnEnable()
		{
			TickSystem<object>.AddTickCallback(this);
			if (this.chargeShader)
			{
				this.chargeShader.UpdateChargeProgress(0f);
			}
		}

		// Token: 0x06005002 RID: 20482 RVA: 0x001BA430 File Offset: 0x001B8630
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

		// Token: 0x06005003 RID: 20483 RVA: 0x001BA528 File Offset: 0x001B8728
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

		// Token: 0x06005004 RID: 20484 RVA: 0x001BA5D8 File Offset: 0x001B87D8
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

		// Token: 0x06005005 RID: 20485 RVA: 0x001BA670 File Offset: 0x001B8870
		private float GetLaunchSpeed()
		{
			if (!this.useButtonPressDurationAsVelocityModifier)
			{
				return 1f;
			}
			return Mathf.Lerp(this.launchMinSpeed, this.launchMaxSpeed, Mathf.InverseLerp(0f, this.maxButtonPressDuration, Mathf.Clamp(this.pressCounter, 0f, this.maxButtonPressDuration)));
		}

		// Token: 0x06005006 RID: 20486 RVA: 0x001BA6C4 File Offset: 0x001B88C4
		private float GetChargeRate()
		{
			if (!this.useButtonPressDurationAsVelocityModifier)
			{
				return (float)this.chargeShader.shaderAnimSteps;
			}
			return Mathf.Lerp(0f, (float)this.chargeShader.shaderAnimSteps, Mathf.InverseLerp(0f, this.maxButtonPressDuration, Mathf.Clamp(this.pressCounter, 0f, this.maxButtonPressDuration)));
		}

		// Token: 0x06005007 RID: 20487 RVA: 0x0006442F File Offset: 0x0006262F
		private void TriggerShoot()
		{
			this.Shoot();
			this.pressCounter = 0f;
		}

		// Token: 0x06005008 RID: 20488 RVA: 0x00064442 File Offset: 0x00062642
		public void OnButtonPressed()
		{
			this.pressStarted = true;
			if (this.launchActivatorType == ProjectileShooterCosmetic.LaunchActivator.ButtonPressed)
			{
				this.TriggerShoot();
			}
		}

		// Token: 0x06005009 RID: 20489 RVA: 0x0006445A File Offset: 0x0006265A
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

		// Token: 0x04005336 RID: 21302
		[SerializeField]
		private float cooldown;

		// Token: 0x04005337 RID: 21303
		[SerializeField]
		private GameObject projectilePrefab;

		// Token: 0x04005338 RID: 21304
		[SerializeField]
		private ParticleSystem launchParticles;

		// Token: 0x04005339 RID: 21305
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x0400533A RID: 21306
		[SerializeField]
		private SoundBankPlayer shootSoundBank;

		// Token: 0x0400533B RID: 21307
		[SerializeField]
		private SoundBankPlayer readyToShootSoundBank;

		// Token: 0x0400533C RID: 21308
		[SerializeField]
		private SoundBankPlayer chargingSoundBank;

		// Token: 0x0400533D RID: 21309
		[SerializeField]
		private float launchMinSpeed;

		// Token: 0x0400533E RID: 21310
		[SerializeField]
		private float launchMaxSpeed;

		// Token: 0x0400533F RID: 21311
		[SerializeField]
		private Transform launchPosition;

		// Token: 0x04005340 RID: 21312
		[SerializeField]
		private ProjectileShooterCosmetic.LaunchActivator launchActivatorType;

		// Token: 0x04005341 RID: 21313
		[SerializeField]
		private bool useButtonPressDurationAsVelocityModifier;

		// Token: 0x04005342 RID: 21314
		[SerializeField]
		private float maxButtonPressDuration = 200f;

		// Token: 0x04005343 RID: 21315
		public UnityEvent<bool, float> onOwnerLaunchProjectile;

		// Token: 0x04005344 RID: 21316
		private int projectileHash;

		// Token: 0x04005345 RID: 21317
		private float launchedTime;

		// Token: 0x04005346 RID: 21318
		private bool canShoot;

		// Token: 0x04005347 RID: 21319
		private float pressStartedTime;

		// Token: 0x04005348 RID: 21320
		private bool pressStarted;

		// Token: 0x04005349 RID: 21321
		private float pressCounter;

		// Token: 0x0400534A RID: 21322
		private TransferrableObject transferrableObject;

		// Token: 0x0400534B RID: 21323
		private ProjectileChargeShader chargeShader;

		// Token: 0x02000C84 RID: 3204
		private enum LaunchActivator
		{
			// Token: 0x0400534E RID: 21326
			ButtonReleased,
			// Token: 0x0400534F RID: 21327
			ButtonPressed,
			// Token: 0x04005350 RID: 21328
			ButtonStayed
		}
	}
}
