using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020003C2 RID: 962
[Serializable]
public class VRMapMiddle : VRMap
{
	// Token: 0x06001742 RID: 5954 RVA: 0x0003FCB0 File Offset: 0x0003DEB0
	public override void MapMyFinger(float lerpValue)
	{
		this.calcT = 0f;
		this.gripValue = ControllerInputPoller.GripFloat(this.vrTargetNode);
		this.calcT = 1f * this.gripValue;
		this.LerpFinger(lerpValue, false);
	}

	// Token: 0x06001743 RID: 5955 RVA: 0x000C79EC File Offset: 0x000C5BEC
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

	// Token: 0x040019C7 RID: 6599
	public InputFeatureUsage inputAxis;

	// Token: 0x040019C8 RID: 6600
	public float gripValue;

	// Token: 0x040019C9 RID: 6601
	public Transform fingerBone1;

	// Token: 0x040019CA RID: 6602
	public Transform fingerBone2;

	// Token: 0x040019CB RID: 6603
	public Transform fingerBone3;

	// Token: 0x040019CC RID: 6604
	public float closedAngles;

	// Token: 0x040019CD RID: 6605
	public Vector3 closedAngle1;

	// Token: 0x040019CE RID: 6606
	public Vector3 closedAngle2;

	// Token: 0x040019CF RID: 6607
	public Vector3 closedAngle3;

	// Token: 0x040019D0 RID: 6608
	public Vector3 startingAngle1;

	// Token: 0x040019D1 RID: 6609
	public Vector3 startingAngle2;

	// Token: 0x040019D2 RID: 6610
	public Vector3 startingAngle3;

	// Token: 0x040019D3 RID: 6611
	public Quaternion[] angle1Table;

	// Token: 0x040019D4 RID: 6612
	public Quaternion[] angle2Table;

	// Token: 0x040019D5 RID: 6613
	public Quaternion[] angle3Table;

	// Token: 0x040019D6 RID: 6614
	private int lastAngle1;

	// Token: 0x040019D7 RID: 6615
	private int lastAngle2;

	// Token: 0x040019D8 RID: 6616
	private int lastAngle3;

	// Token: 0x040019D9 RID: 6617
	private float currentAngle1;

	// Token: 0x040019DA RID: 6618
	private float currentAngle2;

	// Token: 0x040019DB RID: 6619
	private float currentAngle3;

	// Token: 0x040019DC RID: 6620
	private InputDevice tempDevice;

	// Token: 0x040019DD RID: 6621
	private int myTempInt;
}
