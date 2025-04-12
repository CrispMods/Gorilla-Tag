using System;
using UnityEngine;

// Token: 0x02000570 RID: 1392
public class GorillaIK : MonoBehaviour
{
	// Token: 0x06002268 RID: 8808 RVA: 0x000F77CC File Offset: 0x000F59CC
	private void Awake()
	{
		if (Application.isPlaying && !this.testInEditor)
		{
			this.dU = (this.leftUpperArm.position - this.leftLowerArm.position).magnitude;
			this.dL = (this.leftLowerArm.position - this.leftHand.position).magnitude;
			this.dMax = this.dU + this.dL - this.eps;
			this.initialUpperLeft = this.leftUpperArm.localRotation;
			this.initialLowerLeft = this.leftLowerArm.localRotation;
			this.initialUpperRight = this.rightUpperArm.localRotation;
			this.initialLowerRight = this.rightLowerArm.localRotation;
		}
	}

	// Token: 0x06002269 RID: 8809 RVA: 0x000464FE File Offset: 0x000446FE
	private void OnEnable()
	{
		GorillaIKMgr.Instance.RegisterIK(this);
	}

	// Token: 0x0600226A RID: 8810 RVA: 0x0004650B File Offset: 0x0004470B
	private void OnDisable()
	{
		GorillaIKMgr.Instance.DeregisterIK(this);
	}

	// Token: 0x0600226B RID: 8811 RVA: 0x000F78A0 File Offset: 0x000F5AA0
	private void ArmIK(ref Transform upperArm, ref Transform lowerArm, ref Transform hand, Quaternion initRotUpper, Quaternion initRotLower, Transform target)
	{
		upperArm.localRotation = initRotUpper;
		lowerArm.localRotation = initRotLower;
		float num = Mathf.Clamp((target.position - upperArm.position).magnitude, this.eps, this.dMax);
		float num2 = Mathf.Acos(Mathf.Clamp(Vector3.Dot((hand.position - upperArm.position).normalized, (lowerArm.position - upperArm.position).normalized), -1f, 1f));
		float num3 = Mathf.Acos(Mathf.Clamp(Vector3.Dot((upperArm.position - lowerArm.position).normalized, (hand.position - lowerArm.position).normalized), -1f, 1f));
		float num4 = Mathf.Acos(Mathf.Clamp(Vector3.Dot((hand.position - upperArm.position).normalized, (target.position - upperArm.position).normalized), -1f, 1f));
		float num5 = Mathf.Acos(Mathf.Clamp((this.dL * this.dL - this.dU * this.dU - num * num) / (-2f * this.dU * num), -1f, 1f));
		float num6 = Mathf.Acos(Mathf.Clamp((num * num - this.dU * this.dU - this.dL * this.dL) / (-2f * this.dU * this.dL), -1f, 1f));
		Vector3 normalized = Vector3.Cross(hand.position - upperArm.position, lowerArm.position - upperArm.position).normalized;
		Vector3 normalized2 = Vector3.Cross(hand.position - upperArm.position, target.position - upperArm.position).normalized;
		Quaternion rhs = Quaternion.AngleAxis((num5 - num2) * 57.29578f, Quaternion.Inverse(upperArm.rotation) * normalized);
		Quaternion rhs2 = Quaternion.AngleAxis((num6 - num3) * 57.29578f, Quaternion.Inverse(lowerArm.rotation) * normalized);
		Quaternion rhs3 = Quaternion.AngleAxis(num4 * 57.29578f, Quaternion.Inverse(upperArm.rotation) * normalized2);
		this.newRotationUpper = upperArm.localRotation * rhs3 * rhs;
		this.newRotationLower = lowerArm.localRotation * rhs2;
		upperArm.localRotation = this.newRotationUpper;
		lowerArm.localRotation = this.newRotationLower;
		hand.rotation = target.rotation;
	}

	// Token: 0x040025DF RID: 9695
	public Transform headBone;

	// Token: 0x040025E0 RID: 9696
	public Transform leftUpperArm;

	// Token: 0x040025E1 RID: 9697
	public Transform leftLowerArm;

	// Token: 0x040025E2 RID: 9698
	public Transform leftHand;

	// Token: 0x040025E3 RID: 9699
	public Transform rightUpperArm;

	// Token: 0x040025E4 RID: 9700
	public Transform rightLowerArm;

	// Token: 0x040025E5 RID: 9701
	public Transform rightHand;

	// Token: 0x040025E6 RID: 9702
	public Transform targetLeft;

	// Token: 0x040025E7 RID: 9703
	public Transform targetRight;

	// Token: 0x040025E8 RID: 9704
	public Transform targetHead;

	// Token: 0x040025E9 RID: 9705
	public Quaternion initialUpperLeft;

	// Token: 0x040025EA RID: 9706
	public Quaternion initialLowerLeft;

	// Token: 0x040025EB RID: 9707
	public Quaternion initialUpperRight;

	// Token: 0x040025EC RID: 9708
	public Quaternion initialLowerRight;

	// Token: 0x040025ED RID: 9709
	public Quaternion newRotationUpper;

	// Token: 0x040025EE RID: 9710
	public Quaternion newRotationLower;

	// Token: 0x040025EF RID: 9711
	public float dU;

	// Token: 0x040025F0 RID: 9712
	public float dL;

	// Token: 0x040025F1 RID: 9713
	public float dMax;

	// Token: 0x040025F2 RID: 9714
	public bool testInEditor;

	// Token: 0x040025F3 RID: 9715
	public bool reset;

	// Token: 0x040025F4 RID: 9716
	public bool testDefineRot;

	// Token: 0x040025F5 RID: 9717
	public bool moveOnce;

	// Token: 0x040025F6 RID: 9718
	public float eps;

	// Token: 0x040025F7 RID: 9719
	public float upperArmAngle;

	// Token: 0x040025F8 RID: 9720
	public float elbowAngle;
}
