using System;
using GorillaLocomotion.Gameplay;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005E4 RID: 1508
public class SizeLayerChangerGrabable : MonoBehaviour, IGorillaGrabable
{
	// Token: 0x0600257E RID: 9598 RVA: 0x000B973A File Offset: 0x000B793A
	public bool MomentaryGrabOnly()
	{
		return this.momentaryGrabOnly;
	}

	// Token: 0x0600257F RID: 9599 RVA: 0x00044826 File Offset: 0x00042A26
	bool IGorillaGrabable.CanBeGrabbed(GorillaGrabber grabber)
	{
		return true;
	}

	// Token: 0x06002580 RID: 9600 RVA: 0x000B9744 File Offset: 0x000B7944
	void IGorillaGrabable.OnGrabbed(GorillaGrabber g, out Transform grabbedObject, out Vector3 grabbedLocalPosiiton)
	{
		if (this.grabChangesSizeLayer)
		{
			RigContainer rigContainer;
			VRRigCache.Instance.TryGetVrrig(PhotonNetwork.LocalPlayer, out rigContainer);
			rigContainer.Rig.sizeManager.currentSizeLayerMaskValue = this.grabbedSizeLayerMask.Mask;
		}
		grabbedObject = base.transform;
		grabbedLocalPosiiton = base.transform.InverseTransformPoint(g.transform.position);
	}

	// Token: 0x06002581 RID: 9601 RVA: 0x000B97AC File Offset: 0x000B79AC
	void IGorillaGrabable.OnGrabReleased(GorillaGrabber g)
	{
		if (this.releaseChangesSizeLayer)
		{
			RigContainer rigContainer;
			VRRigCache.Instance.TryGetVrrig(PhotonNetwork.LocalPlayer, out rigContainer);
			rigContainer.Rig.sizeManager.currentSizeLayerMaskValue = this.releasedSizeLayerMask.Mask;
		}
	}

	// Token: 0x06002583 RID: 9603 RVA: 0x0001259F File Offset: 0x0001079F
	string IGorillaGrabable.get_name()
	{
		return base.name;
	}

	// Token: 0x040029B2 RID: 10674
	[SerializeField]
	private bool grabChangesSizeLayer = true;

	// Token: 0x040029B3 RID: 10675
	[SerializeField]
	private bool releaseChangesSizeLayer = true;

	// Token: 0x040029B4 RID: 10676
	[SerializeField]
	private SizeLayerMask grabbedSizeLayerMask;

	// Token: 0x040029B5 RID: 10677
	[SerializeField]
	private SizeLayerMask releasedSizeLayerMask;

	// Token: 0x040029B6 RID: 10678
	[SerializeField]
	private bool momentaryGrabOnly = true;
}
