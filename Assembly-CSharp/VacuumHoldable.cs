using System;
using UnityEngine;

// Token: 0x020000E2 RID: 226
public class VacuumHoldable : TransferrableObject
{
	// Token: 0x060005C8 RID: 1480 RVA: 0x00034496 File Offset: 0x00032696
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x000344A6 File Offset: 0x000326A6
	internal override void OnEnable()
	{
		base.OnEnable();
		this.itemState = TransferrableObject.ItemStates.State0;
		this.hasAudioSource = (this.audioSource != null && this.audioSource.clip != null);
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x00083738 File Offset: 0x00081938
	internal override void OnDisable()
	{
		base.OnDisable();
		this.itemState = TransferrableObject.ItemStates.State0;
		if (this.particleFX.isPlaying)
		{
			this.particleFX.Stop();
		}
		if (this.hasAudioSource && this.audioSource.isPlaying)
		{
			this.audioSource.GTStop();
		}
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x0008378C File Offset: 0x0008198C
	private void InitToDefault()
	{
		this.itemState = TransferrableObject.ItemStates.State0;
		if (this.particleFX.isPlaying)
		{
			this.particleFX.Stop();
		}
		if (this.hasAudioSource && this.audioSource.isPlaying)
		{
			this.audioSource.GTStop();
		}
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x000344DD File Offset: 0x000326DD
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		this.InitToDefault();
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x000837D8 File Offset: 0x000819D8
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		if (!this.IsMyItem() && base.myOnlineRig != null && base.myOnlineRig.muted)
		{
			this.itemState = TransferrableObject.ItemStates.State0;
		}
		if (this.itemState == TransferrableObject.ItemStates.State0)
		{
			if (this.particleFX.isPlaying)
			{
				this.particleFX.Stop();
			}
			if (this.hasAudioSource && this.audioSource.isPlaying)
			{
				this.audioSource.GTStop();
				return;
			}
		}
		else
		{
			if (!this.particleFX.isEmitting)
			{
				this.particleFX.Play();
			}
			if (this.hasAudioSource && !this.audioSource.isPlaying)
			{
				this.audioSource.GTPlay();
			}
			if (this.IsMyItem() && Time.time > this.activationStartTime + this.activationVibrationStartDuration)
			{
				GorillaTagger.Instance.StartVibration(this.currentState == TransferrableObject.PositionState.InLeftHand, this.activationVibrationLoopStrength, Time.deltaTime);
			}
		}
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x000838CC File Offset: 0x00081ACC
	public override void OnActivate()
	{
		base.OnActivate();
		this.itemState = TransferrableObject.ItemStates.State1;
		if (this.IsMyItem())
		{
			this.activationStartTime = Time.time;
			GorillaTagger.Instance.StartVibration(this.currentState == TransferrableObject.PositionState.InLeftHand, this.activationVibrationStartStrength, this.activationVibrationStartDuration);
		}
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x000344EB File Offset: 0x000326EB
	public override void OnDeactivate()
	{
		base.OnDeactivate();
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x040006C3 RID: 1731
	[Tooltip("Emission rate will be increase when the trigger button is pressed.")]
	public ParticleSystem particleFX;

	// Token: 0x040006C4 RID: 1732
	[Tooltip("Sound will loop and fade in/out volume when trigger pressed.")]
	public AudioSource audioSource;

	// Token: 0x040006C5 RID: 1733
	private float activationVibrationStartStrength = 0.8f;

	// Token: 0x040006C6 RID: 1734
	private float activationVibrationStartDuration = 0.05f;

	// Token: 0x040006C7 RID: 1735
	private float activationVibrationLoopStrength = 0.005f;

	// Token: 0x040006C8 RID: 1736
	private float activationStartTime;

	// Token: 0x040006C9 RID: 1737
	private bool hasAudioSource;

	// Token: 0x020000E3 RID: 227
	private enum VacuumState
	{
		// Token: 0x040006CB RID: 1739
		None = 1,
		// Token: 0x040006CC RID: 1740
		Active
	}
}
