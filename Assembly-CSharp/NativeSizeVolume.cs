using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000253 RID: 595
public class NativeSizeVolume : MonoBehaviour
{
	// Token: 0x06000DC8 RID: 3528 RVA: 0x00046158 File Offset: 0x00044358
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

	// Token: 0x06000DC9 RID: 3529 RVA: 0x000461B0 File Offset: 0x000443B0
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

	// Token: 0x040010D2 RID: 4306
	[SerializeField]
	private Collider triggerVolume;

	// Token: 0x040010D3 RID: 4307
	[SerializeField]
	private NativeSizeChangerSettings settings;

	// Token: 0x040010D4 RID: 4308
	[SerializeField]
	private NativeSizeVolume.NativeSizeVolumeAction OnEnterAction;

	// Token: 0x040010D5 RID: 4309
	[SerializeField]
	private NativeSizeVolume.NativeSizeVolumeAction OnExitAction;

	// Token: 0x02000254 RID: 596
	[Serializable]
	private enum NativeSizeVolumeAction
	{
		// Token: 0x040010D7 RID: 4311
		None,
		// Token: 0x040010D8 RID: 4312
		ApplySettings,
		// Token: 0x040010D9 RID: 4313
		ResetSize
	}
}
