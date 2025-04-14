using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020003B7 RID: 951
[Serializable]
public class VRMapMiddle : VRMap
{
	// Token: 0x060016F8 RID: 5880 RVA: 0x0007096D File Offset: 0x0006EB6D
	public override void MapMyFinger(float lerpValue)
	{
		this.calcT = 0f;
		this.gripValue = ControllerInputPoller.GripFloat(this.vrTargetNode);
		this.calcT = 1f * this.gripValue;
		this.LerpFinger(lerpValue, false);
	}

	// Token: 0x060016F9 RID: 5881 RVA: 0x000709A8 File Offset: 0x0006EBA8
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

	// Token: 0x0400197F RID: 6527
	public InputFeatureUsage inputAxis;

	// Token: 0x04001980 RID: 6528
	public float gripValue;

	// Token: 0x04001981 RID: 6529
	public Transform fingerBone1;

	// Token: 0x04001982 RID: 6530
	public Transform fingerBone2;

	// Token: 0x04001983 RID: 6531
	public Transform fingerBone3;

	// Token: 0x04001984 RID: 6532
	public float closedAngles;

	// Token: 0x04001985 RID: 6533
	public Vector3 closedAngle1;

	// Token: 0x04001986 RID: 6534
	public Vector3 closedAngle2;

	// Token: 0x04001987 RID: 6535
	public Vector3 closedAngle3;

	// Token: 0x04001988 RID: 6536
	public Vector3 startingAngle1;

	// Token: 0x04001989 RID: 6537
	public Vector3 startingAngle2;

	// Token: 0x0400198A RID: 6538
	public Vector3 startingAngle3;

	// Token: 0x0400198B RID: 6539
	public Quaternion[] angle1Table;

	// Token: 0x0400198C RID: 6540
	public Quaternion[] angle2Table;

	// Token: 0x0400198D RID: 6541
	public Quaternion[] angle3Table;

	// Token: 0x0400198E RID: 6542
	private int lastAngle1;

	// Token: 0x0400198F RID: 6543
	private int lastAngle2;

	// Token: 0x04001990 RID: 6544
	private int lastAngle3;

	// Token: 0x04001991 RID: 6545
	private float currentAngle1;

	// Token: 0x04001992 RID: 6546
	private float currentAngle2;

	// Token: 0x04001993 RID: 6547
	private float currentAngle3;

	// Token: 0x04001994 RID: 6548
	private InputDevice tempDevice;

	// Token: 0x04001995 RID: 6549
	private int myTempInt;
}
