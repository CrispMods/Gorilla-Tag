using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020003C3 RID: 963
[Serializable]
public class VRMapThumb : VRMap
{
	// Token: 0x06001745 RID: 5957 RVA: 0x000C7BE0 File Offset: 0x000C5DE0
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

	// Token: 0x06001746 RID: 5958 RVA: 0x000C7CD4 File Offset: 0x000C5ED4
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

	// Token: 0x040019DE RID: 6622
	public InputFeatureUsage inputAxis;

	// Token: 0x040019DF RID: 6623
	public bool primaryButtonTouch;

	// Token: 0x040019E0 RID: 6624
	public bool primaryButtonPress;

	// Token: 0x040019E1 RID: 6625
	public bool secondaryButtonTouch;

	// Token: 0x040019E2 RID: 6626
	public bool secondaryButtonPress;

	// Token: 0x040019E3 RID: 6627
	public Transform fingerBone1;

	// Token: 0x040019E4 RID: 6628
	public Transform fingerBone2;

	// Token: 0x040019E5 RID: 6629
	public Vector3 closedAngle1;

	// Token: 0x040019E6 RID: 6630
	public Vector3 closedAngle2;

	// Token: 0x040019E7 RID: 6631
	public Vector3 startingAngle1;

	// Token: 0x040019E8 RID: 6632
	public Vector3 startingAngle2;

	// Token: 0x040019E9 RID: 6633
	public Quaternion[] angle1Table;

	// Token: 0x040019EA RID: 6634
	public Quaternion[] angle2Table;

	// Token: 0x040019EB RID: 6635
	private float currentAngle1;

	// Token: 0x040019EC RID: 6636
	private float currentAngle2;

	// Token: 0x040019ED RID: 6637
	private int lastAngle1;

	// Token: 0x040019EE RID: 6638
	private int lastAngle2;

	// Token: 0x040019EF RID: 6639
	private InputDevice tempDevice;

	// Token: 0x040019F0 RID: 6640
	private int myTempInt;
}
