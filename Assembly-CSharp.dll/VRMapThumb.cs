using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020003B8 RID: 952
[Serializable]
public class VRMapThumb : VRMap
{
	// Token: 0x060016FB RID: 5883 RVA: 0x000C53B8 File Offset: 0x000C35B8
	public override void MapMyFinger(float lerpValue)
	{
		this.calcT = 0f;
		if (this.vrTargetNode == XRNode.LeftHand)
		{
			this.primaryButtonPress = ControllerInputPoller.instance.leftControllerPrimaryButton;
			this.primaryButtonTouch = ControllerInputPoller.instance.leftControllerPrimaryButtonTouch;
			this.secondaryButtonPress = ControllerInputPoller.instance.leftControllerSecondaryButton;
			this.secondaryButtonTouch = ControllerInputPoller.instance.leftControllerSecondaryButtonTouch;
		}
		else
		{
			this.primaryButtonPress = ControllerInputPoller.instance.rightControllerPrimaryButton;
			this.primaryButtonTouch = ControllerInputPoller.instance.rightControllerPrimaryButtonTouch;
			this.secondaryButtonPress = ControllerInputPoller.instance.rightControllerSecondaryButton;
			this.secondaryButtonTouch = ControllerInputPoller.instance.rightControllerSecondaryButtonTouch;
		}
		if (this.primaryButtonPress || this.secondaryButtonPress)
		{
			this.calcT = 1f;
		}
		else if (this.primaryButtonTouch || this.secondaryButtonTouch)
		{
			this.calcT = 0.1f;
		}
		this.LerpFinger(lerpValue, false);
	}

	// Token: 0x060016FC RID: 5884 RVA: 0x000C54AC File Offset: 0x000C36AC
	public override void LerpFinger(float lerpValue, bool isOther)
	{
		if (isOther)
		{
			this.currentAngle1 = Mathf.Lerp(this.currentAngle1, this.calcT, lerpValue);
			this.currentAngle2 = Mathf.Lerp(this.currentAngle2, this.calcT, lerpValue);
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
				return;
			}
		}
		else
		{
			this.fingerBone1.localRotation = Quaternion.Lerp(this.fingerBone1.localRotation, Quaternion.Lerp(Quaternion.Euler(this.startingAngle1), Quaternion.Euler(this.closedAngle1), this.calcT), lerpValue);
			this.fingerBone2.localRotation = Quaternion.Lerp(this.fingerBone2.localRotation, Quaternion.Lerp(Quaternion.Euler(this.startingAngle2), Quaternion.Euler(this.closedAngle2), this.calcT), lerpValue);
		}
	}

	// Token: 0x04001996 RID: 6550
	public InputFeatureUsage inputAxis;

	// Token: 0x04001997 RID: 6551
	public bool primaryButtonTouch;

	// Token: 0x04001998 RID: 6552
	public bool primaryButtonPress;

	// Token: 0x04001999 RID: 6553
	public bool secondaryButtonTouch;

	// Token: 0x0400199A RID: 6554
	public bool secondaryButtonPress;

	// Token: 0x0400199B RID: 6555
	public Transform fingerBone1;

	// Token: 0x0400199C RID: 6556
	public Transform fingerBone2;

	// Token: 0x0400199D RID: 6557
	public Vector3 closedAngle1;

	// Token: 0x0400199E RID: 6558
	public Vector3 closedAngle2;

	// Token: 0x0400199F RID: 6559
	public Vector3 startingAngle1;

	// Token: 0x040019A0 RID: 6560
	public Vector3 startingAngle2;

	// Token: 0x040019A1 RID: 6561
	public Quaternion[] angle1Table;

	// Token: 0x040019A2 RID: 6562
	public Quaternion[] angle2Table;

	// Token: 0x040019A3 RID: 6563
	private float currentAngle1;

	// Token: 0x040019A4 RID: 6564
	private float currentAngle2;

	// Token: 0x040019A5 RID: 6565
	private int lastAngle1;

	// Token: 0x040019A6 RID: 6566
	private int lastAngle2;

	// Token: 0x040019A7 RID: 6567
	private InputDevice tempDevice;

	// Token: 0x040019A8 RID: 6568
	private int myTempInt;
}
