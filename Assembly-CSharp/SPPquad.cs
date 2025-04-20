using System;
using UnityEngine;

// Token: 0x02000330 RID: 816
public class SPPquad : MonoBehaviour
{
	// Token: 0x06001363 RID: 4963 RVA: 0x000B6FB8 File Offset: 0x000B51B8
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

	// Token: 0x06001364 RID: 4964 RVA: 0x0003D2CE File Offset: 0x0003B4CE
	public void Grab(OVRInput.Controller grabHand)
	{
		this.passthroughLayer.RemoveSurfaceGeometry(this.projectionObject.gameObject);
		this.controllerHand = grabHand;
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x0003D2ED File Offset: 0x0003B4ED
	public void Release()
	{
		this.controllerHand = OVRInput.Controller.None;
		this.passthroughLayer.AddSurfaceGeometry(this.projectionObject.gameObject, false);
	}

	// Token: 0x04001560 RID: 5472
	private OVRPassthroughLayer passthroughLayer;

	// Token: 0x04001561 RID: 5473
	public MeshFilter projectionObject;

	// Token: 0x04001562 RID: 5474
	private OVRInput.Controller controllerHand;
}
