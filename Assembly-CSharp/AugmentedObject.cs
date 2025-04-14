using System;
using UnityEngine;

// Token: 0x02000308 RID: 776
public class AugmentedObject : MonoBehaviour
{
	// Token: 0x0600127D RID: 4733 RVA: 0x00058D2C File Offset: 0x00056F2C
	private void Start()
	{
		if (base.GetComponent<GrabObject>())
		{
			GrabObject component = base.GetComponent<GrabObject>();
			component.GrabbedObjectDelegate = (GrabObject.GrabbedObject)Delegate.Combine(component.GrabbedObjectDelegate, new GrabObject.GrabbedObject(this.Grab));
			GrabObject component2 = base.GetComponent<GrabObject>();
			component2.ReleasedObjectDelegate = (GrabObject.ReleasedObject)Delegate.Combine(component2.ReleasedObjectDelegate, new GrabObject.ReleasedObject(this.Release));
		}
	}

	// Token: 0x0600127E RID: 4734 RVA: 0x00058D94 File Offset: 0x00056F94
	private void Update()
	{
		if (this.controllerHand != OVRInput.Controller.None && OVRInput.GetUp(OVRInput.Button.One, this.controllerHand))
		{
			this.ToggleShadowType();
		}
		if (this.shadow)
		{
			if (this.groundShadow)
			{
				this.shadow.transform.position = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
				return;
			}
			this.shadow.transform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x0600127F RID: 4735 RVA: 0x00058E22 File Offset: 0x00057022
	public void Grab(OVRInput.Controller grabHand)
	{
		this.controllerHand = grabHand;
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x00058E2B File Offset: 0x0005702B
	public void Release()
	{
		this.controllerHand = OVRInput.Controller.None;
	}

	// Token: 0x06001281 RID: 4737 RVA: 0x00058E34 File Offset: 0x00057034
	private void ToggleShadowType()
	{
		this.groundShadow = !this.groundShadow;
	}

	// Token: 0x0400146F RID: 5231
	public OVRInput.Controller controllerHand;

	// Token: 0x04001470 RID: 5232
	public Transform shadow;

	// Token: 0x04001471 RID: 5233
	private bool groundShadow;
}
