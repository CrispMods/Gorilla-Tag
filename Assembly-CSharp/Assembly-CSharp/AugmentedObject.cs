using System;
using UnityEngine;

// Token: 0x02000308 RID: 776
public class AugmentedObject : MonoBehaviour
{
	// Token: 0x06001280 RID: 4736 RVA: 0x000590B0 File Offset: 0x000572B0
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

	// Token: 0x06001281 RID: 4737 RVA: 0x00059118 File Offset: 0x00057318
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

	// Token: 0x06001282 RID: 4738 RVA: 0x000591A6 File Offset: 0x000573A6
	public void Grab(OVRInput.Controller grabHand)
	{
		this.controllerHand = grabHand;
	}

	// Token: 0x06001283 RID: 4739 RVA: 0x000591AF File Offset: 0x000573AF
	public void Release()
	{
		this.controllerHand = OVRInput.Controller.None;
	}

	// Token: 0x06001284 RID: 4740 RVA: 0x000591B8 File Offset: 0x000573B8
	private void ToggleShadowType()
	{
		this.groundShadow = !this.groundShadow;
	}

	// Token: 0x04001470 RID: 5232
	public OVRInput.Controller controllerHand;

	// Token: 0x04001471 RID: 5233
	public Transform shadow;

	// Token: 0x04001472 RID: 5234
	private bool groundShadow;
}
