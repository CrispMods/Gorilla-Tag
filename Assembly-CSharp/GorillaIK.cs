using System;
using UnityEngine;

// Token: 0x0200057D RID: 1405
public class GorillaIK : MonoBehaviour
{
	// Token: 0x060022BE RID: 8894 RVA: 0x000FA548 File Offset: 0x000F8748
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

	// Token: 0x060022BF RID: 8895 RVA: 0x000478A3 File Offset: 0x00045AA3
	private void OnEnable()
	{
		GorillaIKMgr.Instance.RegisterIK(this);
	}

	// Token: 0x060022C0 RID: 8896 RVA: 0x000478B0 File Offset: 0x00045AB0
	private void OnDisable()
	{
		GorillaIKMgr.Instance.DeregisterIK(this);
	}

	// Token: 0x060022C1 RID: 8897 RVA: 0x000FA61C File Offset: 0x000F881C
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

	// Token: 0x04002631 RID: 9777
	public Transform headBone;

	// Token: 0x04002632 RID: 9778
	public Transform leftUpperArm;

	// Token: 0x04002633 RID: 9779
	public Transform leftLowerArm;

	// Token: 0x04002634 RID: 9780
	public Transform leftHand;

	// Token: 0x04002635 RID: 9781
	public Transform rightUpperArm;

	// Token: 0x04002636 RID: 9782
	public Transform rightLowerArm;

	// Token: 0x04002637 RID: 9783
	public Transform rightHand;

	// Token: 0x04002638 RID: 9784
	public Transform targetLeft;

	// Token: 0x04002639 RID: 9785
	public Transform targetRight;

	// Token: 0x0400263A RID: 9786
	public Transform targetHead;

	// Token: 0x0400263B RID: 9787
	public Quaternion initialUpperLeft;

	// Token: 0x0400263C RID: 9788
	public Quaternion initialLowerLeft;

	// Token: 0x0400263D RID: 9789
	public Quaternion initialUpperRight;

	// Token: 0x0400263E RID: 9790
	public Quaternion initialLowerRight;

	// Token: 0x0400263F RID: 9791
	public Quaternion newRotationUpper;

	// Token: 0x04002640 RID: 9792
	public Quaternion newRotationLower;

	// Token: 0x04002641 RID: 9793
	public float dU;

	// Token: 0x04002642 RID: 9794
	public float dL;

	// Token: 0x04002643 RID: 9795
	public float dMax;

	// Token: 0x04002644 RID: 9796
	public bool testInEditor;

	// Token: 0x04002645 RID: 9797
	public bool reset;

	// Token: 0x04002646 RID: 9798
	public bool testDefineRot;

	// Token: 0x04002647 RID: 9799
	public bool moveOnce;

	// Token: 0x04002648 RID: 9800
	public float eps;

	// Token: 0x04002649 RID: 9801
	public float upperArmAngle;

	// Token: 0x0400264A RID: 9802
	public float elbowAngle;
}
