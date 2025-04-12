using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000253 RID: 595
public class NativeSizeVolume : MonoBehaviour
{
	// Token: 0x06000DCA RID: 3530 RVA: 0x000A0904 File Offset: 0x0009EB04
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

	// Token: 0x06000DCB RID: 3531 RVA: 0x000A095C File Offset: 0x0009EB5C
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

	// Token: 0x040010D3 RID: 4307
	[SerializeField]
	private Collider triggerVolume;

	// Token: 0x040010D4 RID: 4308
	[SerializeField]
	private NativeSizeChangerSettings settings;

	// Token: 0x040010D5 RID: 4309
	[SerializeField]
	private NativeSizeVolume.NativeSizeVolumeAction OnEnterAction;

	// Token: 0x040010D6 RID: 4310
	[SerializeField]
	private NativeSizeVolume.NativeSizeVolumeAction OnExitAction;

	// Token: 0x02000254 RID: 596
	[Serializable]
	private enum NativeSizeVolumeAction
	{
		// Token: 0x040010D8 RID: 4312
		None,
		// Token: 0x040010D9 RID: 4313
		ApplySettings,
		// Token: 0x040010DA RID: 4314
		ResetSize
	}
}
