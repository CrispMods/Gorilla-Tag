using System;
using UnityEngine;

// Token: 0x0200014A RID: 330
public class Crossbow : ProjectileWeapon
{
	// Token: 0x06000867 RID: 2151 RVA: 0x0002E2C8 File Offset: 0x0002C4C8
	protected override void Awake()
	{
		base.Awake();
		TransferrableObjectHoldablePart_Crank[] array = this.cranks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetOnCrankedCallback(new Action<float>(this.OnCrank));
		}
		this.SetReloadFraction(0f);
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x0002E310 File Offset: 0x0002C510
	public void SetReloadFraction(float newFraction)
	{
		this.loadFraction = Mathf.Clamp01(newFraction);
		this.animator.SetFloat(this.ReloadFractionHashID, this.loadFraction);
		if (this.loadFraction == 1f && !this.dummyProjectile.enabled)
		{
			this.shootSfx.GTPlayOneShot(this.reloadComplete_audioClip, 1f);
			this.dummyProjectile.enabled = true;
			return;
		}
		if (this.loadFraction < 1f && this.dummyProjectile.enabled)
		{
			this.dummyProjectile.enabled = false;
		}
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x0002E3A8 File Offset: 0x0002C5A8
	private void OnCrank(float degrees)
	{
		if (this.loadFraction == 1f)
		{
			return;
		}
		this.totalCrankDegrees += degrees;
		this.crankSoundDegrees += degrees;
		if (Mathf.Abs(this.crankSoundDegrees) > this.crankSoundDegreesThreshold)
		{
			this.playingCrankSoundUntilTimestamp = Time.time + this.crankSoundContinueDuration;
			this.crankSoundDegrees = 0f;
		}
		if (!this.reloadAudio.isPlaying && Time.time < this.playingCrankSoundUntilTimestamp)
		{
			this.reloadAudio.GTPlay();
		}
		this.SetReloadFraction(Mathf.Abs(this.totalCrankDegrees / this.crankTotalDegreesToReload));
		if (this.loadFraction >= 1f)
		{
			this.totalCrankDegrees = 0f;
		}
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x0002E464 File Offset: 0x0002C664
	protected override Vector3 GetLaunchPosition()
	{
		return this.launchPosition.position;
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x0002E471 File Offset: 0x0002C671
	protected override Vector3 GetLaunchVelocity()
	{
		return this.launchPosition.forward * this.launchSpeed * base.myRig.scaleFactor;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x0002E49C File Offset: 0x0002C69C
	protected override void LateUpdateLocal()
	{
		base.LateUpdateLocal();
		if (!base.InHand())
		{
			this.wasPressingTrigger = false;
			return;
		}
		if ((base.InLeftHand() ? base.myRig.leftIndex.calcT : base.myRig.rightIndex.calcT) > 0.5f)
		{
			if (this.loadFraction == 1f && !this.wasPressingTrigger)
			{
				this.SetReloadFraction(0f);
				this.animator.SetTrigger(this.FireHashID);
				base.LaunchProjectile();
			}
			this.wasPressingTrigger = true;
		}
		else
		{
			this.wasPressingTrigger = false;
		}
		if (this.itemState.HasFlag(TransferrableObject.ItemStates.State0))
		{
			if (this.loadFraction < 1f)
			{
				this.itemState &= (TransferrableObject.ItemStates)(-2);
				return;
			}
		}
		else if (this.loadFraction == 1f)
		{
			this.itemState |= TransferrableObject.ItemStates.State0;
		}
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x0002E58C File Offset: 0x0002C78C
	protected override void LateUpdateReplicated()
	{
		base.LateUpdateReplicated();
		if (!base.InHand())
		{
			return;
		}
		if (this.itemState.HasFlag(TransferrableObject.ItemStates.State0))
		{
			this.SetReloadFraction(1f);
			return;
		}
		if (this.loadFraction == 1f)
		{
			this.SetReloadFraction(0f);
		}
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x0002E5E4 File Offset: 0x0002C7E4
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		if (this.reloadAudio.isPlaying && Time.time > this.playingCrankSoundUntilTimestamp)
		{
			this.reloadAudio.GTStop();
		}
	}

	// Token: 0x04000A00 RID: 2560
	[SerializeField]
	private Transform launchPosition;

	// Token: 0x04000A01 RID: 2561
	[SerializeField]
	private float launchSpeed;

	// Token: 0x04000A02 RID: 2562
	[SerializeField]
	private Animator animator;

	// Token: 0x04000A03 RID: 2563
	[SerializeField]
	private float crankTotalDegreesToReload;

	// Token: 0x04000A04 RID: 2564
	[SerializeField]
	private TransferrableObjectHoldablePart_Crank[] cranks;

	// Token: 0x04000A05 RID: 2565
	[SerializeField]
	private MeshRenderer dummyProjectile;

	// Token: 0x04000A06 RID: 2566
	[SerializeField]
	private AudioSource reloadAudio;

	// Token: 0x04000A07 RID: 2567
	[SerializeField]
	private AudioClip reloadComplete_audioClip;

	// Token: 0x04000A08 RID: 2568
	[SerializeField]
	private float crankSoundContinueDuration = 0.1f;

	// Token: 0x04000A09 RID: 2569
	[SerializeField]
	private float crankSoundDegreesThreshold = 0.1f;

	// Token: 0x04000A0A RID: 2570
	private AnimHashId FireHashID = "Fire";

	// Token: 0x04000A0B RID: 2571
	private AnimHashId ReloadFractionHashID = "ReloadFraction";

	// Token: 0x04000A0C RID: 2572
	private float totalCrankDegrees;

	// Token: 0x04000A0D RID: 2573
	private float loadFraction;

	// Token: 0x04000A0E RID: 2574
	private float playingCrankSoundUntilTimestamp;

	// Token: 0x04000A0F RID: 2575
	private float crankSoundDegrees;

	// Token: 0x04000A10 RID: 2576
	private bool wasPressingTrigger;
}
