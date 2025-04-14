using System;
using UnityEngine;

// Token: 0x02000325 RID: 805
public class SPPquad : MonoBehaviour
{
	// Token: 0x06001317 RID: 4887 RVA: 0x0005D164 File Offset: 0x0005B364
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

	// Token: 0x06001318 RID: 4888 RVA: 0x0005D1EF File Offset: 0x0005B3EF
	public void Grab(OVRInput.Controller grabHand)
	{
		this.passthroughLayer.RemoveSurfaceGeometry(this.projectionObject.gameObject);
		this.controllerHand = grabHand;
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x0005D20E File Offset: 0x0005B40E
	public void Release()
	{
		this.controllerHand = OVRInput.Controller.None;
		this.passthroughLayer.AddSurfaceGeometry(this.projectionObject.gameObject, false);
	}

	// Token: 0x04001518 RID: 5400
	private OVRPassthroughLayer passthroughLayer;

	// Token: 0x04001519 RID: 5401
	public MeshFilter projectionObject;

	// Token: 0x0400151A RID: 5402
	private OVRInput.Controller controllerHand;
}
