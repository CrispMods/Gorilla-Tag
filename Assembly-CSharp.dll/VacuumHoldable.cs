using System;
using UnityEngine;

// Token: 0x020000D8 RID: 216
public class VacuumHoldable : TransferrableObject
{
	// Token: 0x06000589 RID: 1417 RVA: 0x00033232 File Offset: 0x00031432
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x00033242 File Offset: 0x00031442
	internal override void OnEnable()
	{
		base.OnEnable();
		this.itemState = TransferrableObject.ItemStates.State0;
		this.hasAudioSource = (this.audioSource != null && this.audioSource.clip != null);
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x00080E30 File Offset: 0x0007F030
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

	// Token: 0x0600058C RID: 1420 RVA: 0x00080E84 File Offset: 0x0007F084
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

	// Token: 0x0600058D RID: 1421 RVA: 0x00033279 File Offset: 0x00031479
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		this.InitToDefault();
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x00080ED0 File Offset: 0x0007F0D0
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

	// Token: 0x0600058F RID: 1423 RVA: 0x00080FC4 File Offset: 0x0007F1C4
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

	// Token: 0x06000590 RID: 1424 RVA: 0x00033287 File Offset: 0x00031487
	public override void OnDeactivate()
	{
		base.OnDeactivate();
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x04000683 RID: 1667
	[Tooltip("Emission rate will be increase when the trigger button is pressed.")]
	public ParticleSystem particleFX;

	// Token: 0x04000684 RID: 1668
	[Tooltip("Sound will loop and fade in/out volume when trigger pressed.")]
	public AudioSource audioSource;

	// Token: 0x04000685 RID: 1669
	private float activationVibrationStartStrength = 0.8f;

	// Token: 0x04000686 RID: 1670
	private float activationVibrationStartDuration = 0.05f;

	// Token: 0x04000687 RID: 1671
	private float activationVibrationLoopStrength = 0.005f;

	// Token: 0x04000688 RID: 1672
	private float activationStartTime;

	// Token: 0x04000689 RID: 1673
	private bool hasAudioSource;

	// Token: 0x020000D9 RID: 217
	private enum VacuumState
	{
		// Token: 0x0400068B RID: 1675
		None = 1,
		// Token: 0x0400068C RID: 1676
		Active
	}
}
