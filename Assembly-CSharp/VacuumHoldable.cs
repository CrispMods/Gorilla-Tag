using System;
using UnityEngine;

// Token: 0x020000D8 RID: 216
public class VacuumHoldable : TransferrableObject
{
	// Token: 0x06000587 RID: 1415 RVA: 0x00020A30 File Offset: 0x0001EC30
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x00020A40 File Offset: 0x0001EC40
	internal override void OnEnable()
	{
		base.OnEnable();
		this.itemState = TransferrableObject.ItemStates.State0;
		this.hasAudioSource = (this.audioSource != null && this.audioSource.clip != null);
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x00020A78 File Offset: 0x0001EC78
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

	// Token: 0x0600058A RID: 1418 RVA: 0x00020ACC File Offset: 0x0001ECCC
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

	// Token: 0x0600058B RID: 1419 RVA: 0x00020B18 File Offset: 0x0001ED18
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		this.InitToDefault();
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x00020B28 File Offset: 0x0001ED28
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

	// Token: 0x0600058D RID: 1421 RVA: 0x00020C1C File Offset: 0x0001EE1C
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

	// Token: 0x0600058E RID: 1422 RVA: 0x00020C68 File Offset: 0x0001EE68
	public override void OnDeactivate()
	{
		base.OnDeactivate();
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x04000682 RID: 1666
	[Tooltip("Emission rate will be increase when the trigger button is pressed.")]
	public ParticleSystem particleFX;

	// Token: 0x04000683 RID: 1667
	[Tooltip("Sound will loop and fade in/out volume when trigger pressed.")]
	public AudioSource audioSource;

	// Token: 0x04000684 RID: 1668
	private float activationVibrationStartStrength = 0.8f;

	// Token: 0x04000685 RID: 1669
	private float activationVibrationStartDuration = 0.05f;

	// Token: 0x04000686 RID: 1670
	private float activationVibrationLoopStrength = 0.005f;

	// Token: 0x04000687 RID: 1671
	private float activationStartTime;

	// Token: 0x04000688 RID: 1672
	private bool hasAudioSource;

	// Token: 0x020000D9 RID: 217
	private enum VacuumState
	{
		// Token: 0x0400068A RID: 1674
		None = 1,
		// Token: 0x0400068B RID: 1675
		Active
	}
}
