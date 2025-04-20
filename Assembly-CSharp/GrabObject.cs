using System;
using UnityEngine;

// Token: 0x0200031B RID: 795
public class GrabObject : MonoBehaviour
{
	// Token: 0x060012F7 RID: 4855 RVA: 0x0003CFEA File Offset: 0x0003B1EA
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

	// Token: 0x060012F8 RID: 4856 RVA: 0x0003D00E File Offset: 0x0003B20E
	public void Release()
	{
		GrabObject.ReleasedObject releasedObjectDelegate = this.ReleasedObjectDelegate;
		if (releasedObjectDelegate == null)
		{
			return;
		}
		releasedObjectDelegate();
	}

	// Token: 0x060012F9 RID: 4857 RVA: 0x0003D020 File Offset: 0x0003B220
	public void CursorPos(Vector3 cursorPos)
	{
		GrabObject.SetCursorPosition cursorPositionDelegate = this.CursorPositionDelegate;
		if (cursorPositionDelegate == null)
		{
			return;
		}
		cursorPositionDelegate(cursorPos);
	}

	// Token: 0x040014E2 RID: 5346
	[TextArea]
	public string ObjectName;

	// Token: 0x040014E3 RID: 5347
	[TextArea]
	public string ObjectInstructions;

	// Token: 0x040014E4 RID: 5348
	public GrabObject.ManipulationType objectManipulationType;

	// Token: 0x040014E5 RID: 5349
	public bool showLaserWhileGrabbed;

	// Token: 0x040014E6 RID: 5350
	[HideInInspector]
	public Quaternion grabbedRotation = Quaternion.identity;

	// Token: 0x040014E7 RID: 5351
	public GrabObject.GrabbedObject GrabbedObjectDelegate;

	// Token: 0x040014E8 RID: 5352
	public GrabObject.ReleasedObject ReleasedObjectDelegate;

	// Token: 0x040014E9 RID: 5353
	public GrabObject.SetCursorPosition CursorPositionDelegate;

	// Token: 0x0200031C RID: 796
	public enum ManipulationType
	{
		// Token: 0x040014EB RID: 5355
		Default,
		// Token: 0x040014EC RID: 5356
		ForcedHand,
		// Token: 0x040014ED RID: 5357
		DollyHand,
		// Token: 0x040014EE RID: 5358
		DollyAttached,
		// Token: 0x040014EF RID: 5359
		HorizontalScaled,
		// Token: 0x040014F0 RID: 5360
		VerticalScaled,
		// Token: 0x040014F1 RID: 5361
		Menu
	}

	// Token: 0x0200031D RID: 797
	// (Invoke) Token: 0x060012FC RID: 4860
	public delegate void GrabbedObject(OVRInput.Controller grabHand);

	// Token: 0x0200031E RID: 798
	// (Invoke) Token: 0x06001300 RID: 4864
	public delegate void ReleasedObject();

	// Token: 0x0200031F RID: 799
	// (Invoke) Token: 0x06001304 RID: 4868
	public delegate void SetCursorPosition(Vector3 cursorPosition);
}
