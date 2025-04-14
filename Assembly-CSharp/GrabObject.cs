using System;
using UnityEngine;

// Token: 0x02000310 RID: 784
public class GrabObject : MonoBehaviour
{
	// Token: 0x060012AB RID: 4779 RVA: 0x000598BB File Offset: 0x00057ABB
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

	// Token: 0x060012AC RID: 4780 RVA: 0x000598DF File Offset: 0x00057ADF
	public void Release()
	{
		GrabObject.ReleasedObject releasedObjectDelegate = this.ReleasedObjectDelegate;
		if (releasedObjectDelegate == null)
		{
			return;
		}
		releasedObjectDelegate();
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x000598F1 File Offset: 0x00057AF1
	public void CursorPos(Vector3 cursorPos)
	{
		GrabObject.SetCursorPosition cursorPositionDelegate = this.CursorPositionDelegate;
		if (cursorPositionDelegate == null)
		{
			return;
		}
		cursorPositionDelegate(cursorPos);
	}

	// Token: 0x0400149A RID: 5274
	[TextArea]
	public string ObjectName;

	// Token: 0x0400149B RID: 5275
	[TextArea]
	public string ObjectInstructions;

	// Token: 0x0400149C RID: 5276
	public GrabObject.ManipulationType objectManipulationType;

	// Token: 0x0400149D RID: 5277
	public bool showLaserWhileGrabbed;

	// Token: 0x0400149E RID: 5278
	[HideInInspector]
	public Quaternion grabbedRotation = Quaternion.identity;

	// Token: 0x0400149F RID: 5279
	public GrabObject.GrabbedObject GrabbedObjectDelegate;

	// Token: 0x040014A0 RID: 5280
	public GrabObject.ReleasedObject ReleasedObjectDelegate;

	// Token: 0x040014A1 RID: 5281
	public GrabObject.SetCursorPosition CursorPositionDelegate;

	// Token: 0x02000311 RID: 785
	public enum ManipulationType
	{
		// Token: 0x040014A3 RID: 5283
		Default,
		// Token: 0x040014A4 RID: 5284
		ForcedHand,
		// Token: 0x040014A5 RID: 5285
		DollyHand,
		// Token: 0x040014A6 RID: 5286
		DollyAttached,
		// Token: 0x040014A7 RID: 5287
		HorizontalScaled,
		// Token: 0x040014A8 RID: 5288
		VerticalScaled,
		// Token: 0x040014A9 RID: 5289
		Menu
	}

	// Token: 0x02000312 RID: 786
	// (Invoke) Token: 0x060012B0 RID: 4784
	public delegate void GrabbedObject(OVRInput.Controller grabHand);

	// Token: 0x02000313 RID: 787
	// (Invoke) Token: 0x060012B4 RID: 4788
	public delegate void ReleasedObject();

	// Token: 0x02000314 RID: 788
	// (Invoke) Token: 0x060012B8 RID: 4792
	public delegate void SetCursorPosition(Vector3 cursorPosition);
}
