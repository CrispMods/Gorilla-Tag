using System;
using UnityEngine;

// Token: 0x02000154 RID: 340
public class Crossbow : ProjectileWeapon
{
	// Token: 0x060008AB RID: 2219 RVA: 0x0008F2A0 File Offset: 0x0008D4A0
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

	// Token: 0x060008AC RID: 2220 RVA: 0x0008F2E8 File Offset: 0x0008D4E8
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

	// Token: 0x060008AD RID: 2221 RVA: 0x0008F380 File Offset: 0x0008D580
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

	// Token: 0x060008AE RID: 2222 RVA: 0x00036241 File Offset: 0x00034441
	protected override Vector3 GetLaunchPosition()
	{
		return this.launchPosition.position;
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x0003624E File Offset: 0x0003444E
	protected override Vector3 GetLaunchVelocity()
	{
		return this.launchPosition.forward * this.launchSpeed * base.myRig.scaleFactor;
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x0008F43C File Offset: 0x0008D63C
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

	// Token: 0x060008B1 RID: 2225 RVA: 0x0008F52C File Offset: 0x0008D72C
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

	// Token: 0x060008B2 RID: 2226 RVA: 0x00036276 File Offset: 0x00034476
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		if (this.reloadAudio.isPlaying && Time.time > this.playingCrankSoundUntilTimestamp)
		{
			this.reloadAudio.GTStop();
		}
	}

	// Token: 0x04000A43 RID: 2627
	[SerializeField]
	private Transform launchPosition;

	// Token: 0x04000A44 RID: 2628
	[SerializeField]
	private float launchSpeed;

	// Token: 0x04000A45 RID: 2629
	[SerializeField]
	private Animator animator;

	// Token: 0x04000A46 RID: 2630
	[SerializeField]
	private float crankTotalDegreesToReload;

	// Token: 0x04000A47 RID: 2631
	[SerializeField]
	private TransferrableObjectHoldablePart_Crank[] cranks;

	// Token: 0x04000A48 RID: 2632
	[SerializeField]
	private MeshRenderer dummyProjectile;

	// Token: 0x04000A49 RID: 2633
	[SerializeField]
	private AudioSource reloadAudio;

	// Token: 0x04000A4A RID: 2634
	[SerializeField]
	private AudioClip reloadComplete_audioClip;

	// Token: 0x04000A4B RID: 2635
	[SerializeField]
	private float crankSoundContinueDuration = 0.1f;

	// Token: 0x04000A4C RID: 2636
	[SerializeField]
	private float crankSoundDegreesThreshold = 0.1f;

	// Token: 0x04000A4D RID: 2637
	private AnimHashId FireHashID = "Fire";

	// Token: 0x04000A4E RID: 2638
	private AnimHashId ReloadFractionHashID = "ReloadFraction";

	// Token: 0x04000A4F RID: 2639
	private float totalCrankDegrees;

	// Token: 0x04000A50 RID: 2640
	private float loadFraction;

	// Token: 0x04000A51 RID: 2641
	private float playingCrankSoundUntilTimestamp;

	// Token: 0x04000A52 RID: 2642
	private float crankSoundDegrees;

	// Token: 0x04000A53 RID: 2643
	private bool wasPressingTrigger;
}
