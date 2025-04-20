using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020003C1 RID: 961
[Serializable]
public class VRMapIndex : VRMap
{
	// Token: 0x0600173F RID: 5951 RVA: 0x000C7788 File Offset: 0x000C5988
	public override void MapMyFinger(float lerpValue)
	{
		this.calcT = 0f;
		this.triggerValue = ControllerInputPoller.TriggerFloat(this.vrTargetNode);
		this.triggerTouch = ControllerInputPoller.TriggerTouch(this.vrTargetNode);
		this.calcT = 0.1f * this.triggerTouch;
		this.calcT += 0.9f * this.triggerValue;
		this.LerpFinger(lerpValue, false);
	}

	// Token: 0x06001740 RID: 5952 RVA: 0x000C77F8 File Offset: 0x000C59F8
	public override void LerpFinger(float lerpValue, bool isOther)
	{
		if (isOther)
		{
			this.currentAngle1 = Mathf.Lerp(this.currentAngle1, this.calcT, lerpValue);
			this.currentAngle2 = Mathf.Lerp(this.currentAngle2, this.calcT, lerpValue);
			this.currentAngle3 = Mathf.Lerp(this.currentAngle3, this.calcT, lerpValue);
			this.myTempInt = (int)(this.currentAngle1 * 10.1f);
			if (this.myTempInt != this.lastAngle1)
			{
				this.lastAngle1 = this.myTempInt;
				this.fingerBone1.localRotation = this.angle1Table[this.lastAngle1];
			}
			this.myTempInt = (int)(this.currentAngle2 * 10.1f);
			if (this.myTempInt != this.lastAngle2)
			{
				this.lastAngle2 = this.myTempInt;
				this.fingerBone2.localRotation = this.angle2Table[this.lastAngle2];
			}
			this.myTempInt = (int)(this.currentAngle3 * 10.1f);
			if (this.myTempInt != this.lastAngle3)
			{
				this.lastAngle3 = this.myTempInt;
				this.fingerBone3.localRotation = this.angle3Table[this.lastAngle3];
				return;
			}
		}
		else
		{
			this.fingerBone1.localRotation = Quaternion.Lerp(this.fingerBone1.localRotation, Quaternion.Lerp(Quaternion.Euler(this.startingAngle1), Quaternion.Euler(this.closedAngle1), this.calcT), lerpValue);
			this.fingerBone2.localRotation = Quaternion.Lerp(this.fingerBone2.localRotation, Quaternion.Lerp(Quaternion.Euler(this.startingAngle2), Quaternion.Euler(this.closedAngle2), this.calcT), lerpValue);
			this.fingerBone3.localRotation = Quaternion.Lerp(this.fingerBone3.localRotation, Quaternion.Lerp(Quaternion.Euler(this.startingAngle3), Quaternion.Euler(this.closedAngle3), this.calcT), lerpValue);
		}
	}

	// Token: 0x040019AF RID: 6575
	public InputFeatureUsage inputAxis;

	// Token: 0x040019B0 RID: 6576
	public float triggerTouch;

	// Token: 0x040019B1 RID: 6577
	public float triggerValue;

	// Token: 0x040019B2 RID: 6578
	public Transform fingerBone1;

	// Token: 0x040019B3 RID: 6579
	public Transform fingerBone2;

	// Token: 0x040019B4 RID: 6580
	public Transform fingerBone3;

	// Token: 0x040019B5 RID: 6581
	public float closedAngles;

	// Token: 0x040019B6 RID: 6582
	public Vector3 closedAngle1;

	// Token: 0x040019B7 RID: 6583
	public Vector3 closedAngle2;

	// Token: 0x040019B8 RID: 6584
	public Vector3 closedAngle3;

	// Token: 0x040019B9 RID: 6585
	public Vector3 startingAngle1;

	// Token: 0x040019BA RID: 6586
	public Vector3 startingAngle2;

	// Token: 0x040019BB RID: 6587
	public Vector3 startingAngle3;

	// Token: 0x040019BC RID: 6588
	private int lastAngle1;

	// Token: 0x040019BD RID: 6589
	private int lastAngle2;

	// Token: 0x040019BE RID: 6590
	private int lastAngle3;

	// Token: 0x040019BF RID: 6591
	private InputDevice myInputDevice;

	// Token: 0x040019C0 RID: 6592
	public Quaternion[] angle1Table;

	// Token: 0x040019C1 RID: 6593
	public Quaternion[] angle2Table;

	// Token: 0x040019C2 RID: 6594
	public Quaternion[] angle3Table;

	// Token: 0x040019C3 RID: 6595
	private float currentAngle1;

	// Token: 0x040019C4 RID: 6596
	private float currentAngle2;

	// Token: 0x040019C5 RID: 6597
	private float currentAngle3;

	// Token: 0x040019C6 RID: 6598
	private int myTempInt;
}
