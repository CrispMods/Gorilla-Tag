using System;
using UnityEngine;

// Token: 0x02000313 RID: 787
public class AugmentedObject : MonoBehaviour
{
	// Token: 0x060012C9 RID: 4809 RVA: 0x000B2FE8 File Offset: 0x000B11E8
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

	// Token: 0x060012CA RID: 4810 RVA: 0x000B3050 File Offset: 0x000B1250
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

	// Token: 0x060012CB RID: 4811 RVA: 0x0003CE73 File Offset: 0x0003B073
	public void Grab(OVRInput.Controller grabHand)
	{
		this.controllerHand = grabHand;
	}

	// Token: 0x060012CC RID: 4812 RVA: 0x0003CE7C File Offset: 0x0003B07C
	public void Release()
	{
		this.controllerHand = OVRInput.Controller.None;
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x0003CE85 File Offset: 0x0003B085
	private void ToggleShadowType()
	{
		this.groundShadow = !this.groundShadow;
	}

	// Token: 0x040014B7 RID: 5303
	public OVRInput.Controller controllerHand;

	// Token: 0x040014B8 RID: 5304
	public Transform shadow;

	// Token: 0x040014B9 RID: 5305
	private bool groundShadow;
}
