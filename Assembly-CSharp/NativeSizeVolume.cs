using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200025E RID: 606
public class NativeSizeVolume : MonoBehaviour
{
	// Token: 0x06000E13 RID: 3603 RVA: 0x000A3190 File Offset: 0x000A1390
	private void OnTriggerEnter(Collider other)
	{
		GTPlayer componentInParent = other.GetComponentInParent<GTPlayer>();
		if (componentInParent == null)
		{
			return;
		}
		NativeSizeVolume.NativeSizeVolumeAction onEnterAction = this.OnEnterAction;
		if (onEnterAction == NativeSizeVolume.NativeSizeVolumeAction.ApplySettings)
		{
			this.settings.WorldPosition = base.transform.position;
			componentInParent.SetNativeScale(this.settings);
			return;
		}
		if (onEnterAction != NativeSizeVolume.NativeSizeVolumeAction.ResetSize)
		{
			return;
		}
		componentInParent.SetNativeScale(null);
	}

	// Token: 0x06000E14 RID: 3604 RVA: 0x000A31E8 File Offset: 0x000A13E8
	private void OnTriggerExit(Collider other)
	{
		GTPlayer componentInParent = other.GetComponentInParent<GTPlayer>();
		if (componentInParent == null)
		{
			return;
		}
		NativeSizeVolume.NativeSizeVolumeAction onExitAction = this.OnExitAction;
		if (onExitAction == NativeSizeVolume.NativeSizeVolumeAction.ApplySettings)
		{
			this.settings.WorldPosition = base.transform.position;
			componentInParent.SetNativeScale(this.settings);
			return;
		}
		if (onExitAction != NativeSizeVolume.NativeSizeVolumeAction.ResetSize)
		{
			return;
		}
		componentInParent.SetNativeScale(null);
	}

	// Token: 0x04001118 RID: 4376
	[SerializeField]
	private Collider triggerVolume;

	// Token: 0x04001119 RID: 4377
	[SerializeField]
	private NativeSizeChangerSettings settings;

	// Token: 0x0400111A RID: 4378
	[SerializeField]
	private NativeSizeVolume.NativeSizeVolumeAction OnEnterAction;

	// Token: 0x0400111B RID: 4379
	[SerializeField]
	private NativeSizeVolume.NativeSizeVolumeAction OnExitAction;

	// Token: 0x0200025F RID: 607
	[Serializable]
	private enum NativeSizeVolumeAction
	{
		// Token: 0x0400111D RID: 4381
		None,
		// Token: 0x0400111E RID: 4382
		ApplySettings,
		// Token: 0x0400111F RID: 4383
		ResetSize
	}
}
