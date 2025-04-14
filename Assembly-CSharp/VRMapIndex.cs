using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020003B6 RID: 950
[Serializable]
public class VRMapIndex : VRMap
{
	// Token: 0x060016F2 RID: 5874 RVA: 0x00070380 File Offset: 0x0006E580
	public override void MapMyFinger(float lerpValue)
	{
		this.calcT = 0f;
		this.triggerValue = ControllerInputPoller.TriggerFloat(this.vrTargetNode);
		this.triggerTouch = ControllerInputPoller.TriggerTouch(this.vrTargetNode);
		this.calcT = 0.1f * this.triggerTouch;
		this.calcT += 0.9f * this.triggerValue;
		this.LerpFinger(lerpValue, false);
	}

	// Token: 0x060016F3 RID: 5875 RVA: 0x000703F0 File Offset: 0x0006E5F0
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

	// Token: 0x04001966 RID: 6502
	public InputFeatureUsage inputAxis;

	// Token: 0x04001967 RID: 6503
	public float triggerTouch;

	// Token: 0x04001968 RID: 6504
	public float triggerValue;

	// Token: 0x04001969 RID: 6505
	public Transform fingerBone1;

	// Token: 0x0400196A RID: 6506
	public Transform fingerBone2;

	// Token: 0x0400196B RID: 6507
	public Transform fingerBone3;

	// Token: 0x0400196C RID: 6508
	public float closedAngles;

	// Token: 0x0400196D RID: 6509
	public Vector3 closedAngle1;

	// Token: 0x0400196E RID: 6510
	public Vector3 closedAngle2;

	// Token: 0x0400196F RID: 6511
	public Vector3 closedAngle3;

	// Token: 0x04001970 RID: 6512
	public Vector3 startingAngle1;

	// Token: 0x04001971 RID: 6513
	public Vector3 startingAngle2;

	// Token: 0x04001972 RID: 6514
	public Vector3 startingAngle3;

	// Token: 0x04001973 RID: 6515
	private int lastAngle1;

	// Token: 0x04001974 RID: 6516
	private int lastAngle2;

	// Token: 0x04001975 RID: 6517
	private int lastAngle3;

	// Token: 0x04001976 RID: 6518
	private InputDevice myInputDevice;

	// Token: 0x04001977 RID: 6519
	public Quaternion[] angle1Table;

	// Token: 0x04001978 RID: 6520
	public Quaternion[] angle2Table;

	// Token: 0x04001979 RID: 6521
	public Quaternion[] angle3Table;

	// Token: 0x0400197A RID: 6522
	private float currentAngle1;

	// Token: 0x0400197B RID: 6523
	private float currentAngle2;

	// Token: 0x0400197C RID: 6524
	private float currentAngle3;

	// Token: 0x0400197D RID: 6525
	private int myTempInt;
}
