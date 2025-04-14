using System;
using UnityEngine;

// Token: 0x02000310 RID: 784
public class GrabObject : MonoBehaviour
{
	// Token: 0x060012AE RID: 4782 RVA: 0x00059C3F File Offset: 0x00057E3F
	public void Grab(OVRInput.Controller grabHand)
	{
		this.grabbedRotation = base.transform.rotation;
		GrabObject.GrabbedObject grabbedObjectDelegate = this.GrabbedObjectDelegate;
		if (grabbedObjectDelegate == null)
		{
			return;
		}
		grabbedObjectDelegate(grabHand);
	}

	// Token: 0x060012AF RID: 4783 RVA: 0x00059C63 File Offset: 0x00057E63
	public void Release()
	{
		GrabObject.ReleasedObject releasedObjectDelegate = this.ReleasedObjectDelegate;
		if (releasedObjectDelegate == null)
		{
			return;
		}
		releasedObjectDelegate();
	}

	// Token: 0x060012B0 RID: 4784 RVA: 0x00059C75 File Offset: 0x00057E75
	public void CursorPos(Vector3 cursorPos)
	{
		GrabObject.SetCursorPosition cursorPositionDelegate = this.CursorPositionDelegate;
		if (cursorPositionDelegate == null)
		{
			return;
		}
		cursorPositionDelegate(cursorPos);
	}

	// Token: 0x0400149B RID: 5275
	[TextArea]
	public string ObjectName;

	// Token: 0x0400149C RID: 5276
	[TextArea]
	public string ObjectInstructions;

	// Token: 0x0400149D RID: 5277
	public GrabObject.ManipulationType objectManipulationType;

	// Token: 0x0400149E RID: 5278
	public bool showLaserWhileGrabbed;

	// Token: 0x0400149F RID: 5279
	[HideInInspector]
	public Quaternion grabbedRotation = Quaternion.identity;

	// Token: 0x040014A0 RID: 5280
	public GrabObject.GrabbedObject GrabbedObjectDelegate;

	// Token: 0x040014A1 RID: 5281
	public GrabObject.ReleasedObject ReleasedObjectDelegate;

	// Token: 0x040014A2 RID: 5282
	public GrabObject.SetCursorPosition CursorPositionDelegate;

	// Token: 0x02000311 RID: 785
	public enum ManipulationType
	{
		// Token: 0x040014A4 RID: 5284
		Default,
		// Token: 0x040014A5 RID: 5285
		ForcedHand,
		// Token: 0x040014A6 RID: 5286
		DollyHand,
		// Token: 0x040014A7 RID: 5287
		DollyAttached,
		// Token: 0x040014A8 RID: 5288
		HorizontalScaled,
		// Token: 0x040014A9 RID: 5289
		VerticalScaled,
		// Token: 0x040014AA RID: 5290
		Menu
	}

	// Token: 0x02000312 RID: 786
	// (Invoke) Token: 0x060012B3 RID: 4787
	public delegate void GrabbedObject(OVRInput.Controller grabHand);

	// Token: 0x02000313 RID: 787
	// (Invoke) Token: 0x060012B7 RID: 4791
	public delegate void ReleasedObject();

	// Token: 0x02000314 RID: 788
	// (Invoke) Token: 0x060012BB RID: 4795
	public delegate void SetCursorPosition(Vector3 cursorPosition);
}
