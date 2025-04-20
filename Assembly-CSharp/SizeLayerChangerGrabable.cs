using System;
using GorillaLocomotion.Gameplay;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005F1 RID: 1521
public class SizeLayerChangerGrabable : MonoBehaviour, IGorillaGrabable
{
	// Token: 0x060025D8 RID: 9688 RVA: 0x00049A9B File Offset: 0x00047C9B
	public bool MomentaryGrabOnly()
	{
		return this.momentaryGrabOnly;
	}

	// Token: 0x060025D9 RID: 9689 RVA: 0x00039846 File Offset: 0x00037A46
	bool IGorillaGrabable.CanBeGrabbed(GorillaGrabber grabber)
	{
		return true;
	}

	// Token: 0x060025DA RID: 9690 RVA: 0x00107080 File Offset: 0x00105280
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

	// Token: 0x060025DB RID: 9691 RVA: 0x001070E8 File Offset: 0x001052E8
	void IGorillaGrabable.OnGrabReleased(GorillaGrabber g)
	{
		if (this.releaseChangesSizeLayer)
		{
			RigContainer rigContainer;
			VRRigCache.Instance.TryGetVrrig(PhotonNetwork.LocalPlayer, out rigContainer);
			rigContainer.Rig.sizeManager.currentSizeLayerMaskValue = this.releasedSizeLayerMask.Mask;
		}
	}

	// Token: 0x060025DD RID: 9693 RVA: 0x0003261E File Offset: 0x0003081E
	string IGorillaGrabable.get_name()
	{
		return base.name;
	}

	// Token: 0x04002A0B RID: 10763
	[SerializeField]
	private bool grabChangesSizeLayer = true;

	// Token: 0x04002A0C RID: 10764
	[SerializeField]
	private bool releaseChangesSizeLayer = true;

	// Token: 0x04002A0D RID: 10765
	[SerializeField]
	private SizeLayerMask grabbedSizeLayerMask;

	// Token: 0x04002A0E RID: 10766
	[SerializeField]
	private SizeLayerMask releasedSizeLayerMask;

	// Token: 0x04002A0F RID: 10767
	[SerializeField]
	private bool momentaryGrabOnly = true;
}
