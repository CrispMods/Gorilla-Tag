using System;
using GorillaLocomotion.Gameplay;
using Photon.Pun;
using UnityEngine;

// Token: 0x020005E3 RID: 1507
public class SizeLayerChangerGrabable : MonoBehaviour, IGorillaGrabable
{
	// Token: 0x06002576 RID: 9590 RVA: 0x000B92BA File Offset: 0x000B74BA
	public bool MomentaryGrabOnly()
	{
		return this.momentaryGrabOnly;
	}

	// Token: 0x06002577 RID: 9591 RVA: 0x000444E2 File Offset: 0x000426E2
	bool IGorillaGrabable.CanBeGrabbed(GorillaGrabber grabber)
	{
		return true;
	}

	// Token: 0x06002578 RID: 9592 RVA: 0x000B92C4 File Offset: 0x000B74C4
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

	// Token: 0x06002579 RID: 9593 RVA: 0x000B932C File Offset: 0x000B752C
	void IGorillaGrabable.OnGrabReleased(GorillaGrabber g)
	{
		if (this.releaseChangesSizeLayer)
		{
			RigContainer rigContainer;
			VRRigCache.Instance.TryGetVrrig(PhotonNetwork.LocalPlayer, out rigContainer);
			rigContainer.Rig.sizeManager.currentSizeLayerMaskValue = this.releasedSizeLayerMask.Mask;
		}
	}

	// Token: 0x0600257B RID: 9595 RVA: 0x0001227B File Offset: 0x0001047B
	string IGorillaGrabable.get_name()
	{
		return base.name;
	}

	// Token: 0x040029AC RID: 10668
	[SerializeField]
	private bool grabChangesSizeLayer = true;

	// Token: 0x040029AD RID: 10669
	[SerializeField]
	private bool releaseChangesSizeLayer = true;

	// Token: 0x040029AE RID: 10670
	[SerializeField]
	private SizeLayerMask grabbedSizeLayerMask;

	// Token: 0x040029AF RID: 10671
	[SerializeField]
	private SizeLayerMask releasedSizeLayerMask;

	// Token: 0x040029B0 RID: 10672
	[SerializeField]
	private bool momentaryGrabOnly = true;
}
