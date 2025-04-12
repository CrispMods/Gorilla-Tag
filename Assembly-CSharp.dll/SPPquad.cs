using System;
using UnityEngine;

// Token: 0x02000325 RID: 805
public class SPPquad : MonoBehaviour
{
	// Token: 0x0600131A RID: 4890 RVA: 0x000B4720 File Offset: 0x000B2920
	private void Start()
	{
		this.passthroughLayer = base.GetComponent<OVRPassthroughLayer>();
		this.passthroughLayer.AddSurfaceGeometry(this.projectionObject.gameObject, false);
		if (base.GetComponent<GrabObject>())
		{
			GrabObject component = base.GetComponent<GrabObject>();
			component.GrabbedObjectDelegate = (GrabObject.GrabbedObject)Delegate.Combine(component.GrabbedObjectDelegate, new GrabObject.GrabbedObject(this.Grab));
			GrabObject component2 = base.GetComponent<GrabObject>();
			component2.ReleasedObjectDelegate = (GrabObject.ReleasedObject)Delegate.Combine(component2.ReleasedObjectDelegate, new GrabObject.ReleasedObject(this.Release));
		}
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x0003C00E File Offset: 0x0003A20E
	public void Grab(OVRInput.Controller grabHand)
	{
		this.passthroughLayer.RemoveSurfaceGeometry(this.projectionObject.gameObject);
		this.controllerHand = grabHand;
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x0003C02D File Offset: 0x0003A22D
	public void Release()
	{
		this.controllerHand = OVRInput.Controller.None;
		this.passthroughLayer.AddSurfaceGeometry(this.projectionObject.gameObject, false);
	}

	// Token: 0x04001519 RID: 5401
	private OVRPassthroughLayer passthroughLayer;

	// Token: 0x0400151A RID: 5402
	public MeshFilter projectionObject;

	// Token: 0x0400151B RID: 5403
	private OVRInput.Controller controllerHand;
}
