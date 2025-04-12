using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

// Token: 0x02000571 RID: 1393
public class GorillaIKMgr : MonoBehaviour
{
	// Token: 0x1700037F RID: 895
	// (get) Token: 0x0600226D RID: 8813 RVA: 0x00046518 File Offset: 0x00044718
	public static GorillaIKMgr Instance
	{
		get
		{
			return GorillaIKMgr._instance;
		}
	}

	// Token: 0x0600226E RID: 8814 RVA: 0x000F7B94 File Offset: 0x000F5D94
	private void Awake()
	{
		GorillaIKMgr._instance = this;
		this.firstFrame = true;
		this.tAA = new TransformAccessArray(0, -1);
		this.transformList = new List<Transform>();
		this.job = new GorillaIKMgr.IKJob
		{
			constantInput = new NativeArray<GorillaIKMgr.IKConstantInput>(40, Allocator.Persistent, NativeArrayOptions.ClearMemory),
			input = new NativeArray<GorillaIKMgr.IKInput>(40, Allocator.Persistent, NativeArrayOptions.ClearMemory),
			output = new NativeArray<GorillaIKMgr.IKOutput>(40, Allocator.Persistent, NativeArrayOptions.ClearMemory)
		};
		this.jobXform = new GorillaIKMgr.IKTransformJob
		{
			transformRotations = new NativeArray<Quaternion>(140, Allocator.Persistent, NativeArrayOptions.ClearMemory)
		};
	}

	// Token: 0x0600226F RID: 8815 RVA: 0x000F7C28 File Offset: 0x000F5E28
	private void OnDestroy()
	{
		this.jobHandle.Complete();
		this.jobXformHandle.Complete();
		this.jobXform.transformRotations.Dispose();
		this.tAA.Dispose();
		this.job.input.Dispose();
		this.job.constantInput.Dispose();
		this.job.output.Dispose();
	}

	// Token: 0x06002270 RID: 8816 RVA: 0x000F7C98 File Offset: 0x000F5E98
	public void RegisterIK(GorillaIK ik)
	{
		this.ikList.Add(ik);
		this.actualListSz += 2;
		this.updatedSinceLastRun = true;
		if (this.job.constantInput.IsCreated)
		{
			this.SetConstantData(ik, this.actualListSz - 2);
		}
	}

	// Token: 0x06002271 RID: 8817 RVA: 0x000F7CE8 File Offset: 0x000F5EE8
	public void DeregisterIK(GorillaIK ik)
	{
		int num = this.ikList.FindIndex((GorillaIK curr) => curr == ik);
		this.updatedSinceLastRun = true;
		this.ikList.RemoveAt(num);
		this.actualListSz -= 2;
		if (this.job.constantInput.IsCreated)
		{
			for (int i = num; i < this.actualListSz; i++)
			{
				this.job.constantInput[i] = this.job.constantInput[i + 2];
			}
		}
	}

	// Token: 0x06002272 RID: 8818 RVA: 0x000F7D84 File Offset: 0x000F5F84
	private void SetConstantData(GorillaIK ik, int index)
	{
		this.job.constantInput[index] = new GorillaIKMgr.IKConstantInput
		{
			initRotLower = ik.initialLowerLeft,
			initRotUpper = ik.initialUpperLeft
		};
		this.job.constantInput[index + 1] = new GorillaIKMgr.IKConstantInput
		{
			initRotLower = ik.initialLowerRight,
			initRotUpper = ik.initialUpperRight
		};
	}

	// Token: 0x06002273 RID: 8819 RVA: 0x000F7DFC File Offset: 0x000F5FFC
	private void CopyInput()
	{
		int num = 0;
		int i = 0;
		while (i < this.actualListSz)
		{
			GorillaIK gorillaIK = this.ikList[i / 2];
			this.job.input[i] = new GorillaIKMgr.IKInput
			{
				targetPos = gorillaIK.leftUpperArm.parent.InverseTransformPoint(gorillaIK.targetLeft.position)
			};
			this.job.input[i + 1] = new GorillaIKMgr.IKInput
			{
				targetPos = gorillaIK.rightUpperArm.parent.InverseTransformPoint(gorillaIK.targetRight.position)
			};
			i += 2;
			num++;
		}
	}

	// Token: 0x06002274 RID: 8820 RVA: 0x000F7EB0 File Offset: 0x000F60B0
	private void CopyOutput()
	{
		bool flag = false;
		if (this.updatedSinceLastRun || this.tAA.length != this.ikList.Count * 7)
		{
			flag = true;
			this.tAA.Dispose();
			this.transformList.Clear();
		}
		for (int i = 0; i < this.ikList.Count; i++)
		{
			GorillaIK gorillaIK = this.ikList[i];
			if (flag || this.updatedSinceLastRun)
			{
				this.transformList.Add(gorillaIK.leftUpperArm);
				this.transformList.Add(gorillaIK.leftLowerArm);
				this.transformList.Add(gorillaIK.rightUpperArm);
				this.transformList.Add(gorillaIK.rightLowerArm);
				this.transformList.Add(gorillaIK.headBone);
				this.transformList.Add(gorillaIK.leftHand);
				this.transformList.Add(gorillaIK.rightHand);
			}
			this.jobXform.transformRotations[this.tFormCount * i] = this.job.output[i * 2].upperArmLocalRot;
			this.jobXform.transformRotations[this.tFormCount * i + 1] = this.job.output[i * 2].lowerArmLocalRot;
			this.jobXform.transformRotations[this.tFormCount * i + 2] = this.job.output[i * 2 + 1].upperArmLocalRot;
			this.jobXform.transformRotations[this.tFormCount * i + 3] = this.job.output[i * 2 + 1].lowerArmLocalRot;
			this.jobXform.transformRotations[this.tFormCount * i + 4] = gorillaIK.targetHead.rotation;
			this.jobXform.transformRotations[this.tFormCount * i + 5] = gorillaIK.targetLeft.rotation;
			this.jobXform.transformRotations[this.tFormCount * i + 6] = gorillaIK.targetRight.rotation;
		}
		if (flag)
		{
			this.tAA = new TransformAccessArray(this.transformList.ToArray(), -1);
		}
		this.updatedSinceLastRun = false;
	}

	// Token: 0x06002275 RID: 8821 RVA: 0x000F8100 File Offset: 0x000F6300
	public void LateUpdate()
	{
		if (!this.firstFrame)
		{
			this.jobXformHandle.Complete();
		}
		this.CopyInput();
		this.jobHandle = this.job.Schedule(this.actualListSz, 20, default(JobHandle));
		this.jobHandle.Complete();
		this.CopyOutput();
		this.jobXformHandle = this.jobXform.Schedule(this.tAA, default(JobHandle));
		this.firstFrame = false;
	}

	// Token: 0x040025F9 RID: 9721
	[OnEnterPlay_SetNull]
	private static GorillaIKMgr _instance;

	// Token: 0x040025FA RID: 9722
	private const int MaxSize = 20;

	// Token: 0x040025FB RID: 9723
	private List<GorillaIK> ikList = new List<GorillaIK>(20);

	// Token: 0x040025FC RID: 9724
	private int actualListSz;

	// Token: 0x040025FD RID: 9725
	private JobHandle jobHandle;

	// Token: 0x040025FE RID: 9726
	private JobHandle jobXformHandle;

	// Token: 0x040025FF RID: 9727
	private bool firstFrame = true;

	// Token: 0x04002600 RID: 9728
	private TransformAccessArray tAA;

	// Token: 0x04002601 RID: 9729
	private List<Transform> transformList;

	// Token: 0x04002602 RID: 9730
	private bool updatedSinceLastRun;

	// Token: 0x04002603 RID: 9731
	private int tFormCount = 7;

	// Token: 0x04002604 RID: 9732
	private GorillaIKMgr.IKJob job;

	// Token: 0x04002605 RID: 9733
	private GorillaIKMgr.IKTransformJob jobXform;

	// Token: 0x02000572 RID: 1394
	private struct IKConstantInput
	{
		// Token: 0x04002606 RID: 9734
		public Quaternion initRotLower;

		// Token: 0x04002607 RID: 9735
		public Quaternion initRotUpper;
	}

	// Token: 0x02000573 RID: 1395
	private struct IKInput
	{
		// Token: 0x04002608 RID: 9736
		public Vector3 targetPos;
	}

	// Token: 0x02000574 RID: 1396
	private struct IKOutput
	{
		// Token: 0x06002277 RID: 8823 RVA: 0x00046542 File Offset: 0x00044742
		public IKOutput(Quaternion upperArmLocalRot_, Quaternion lowerArmLocalRot_)
		{
			this.upperArmLocalRot = upperArmLocalRot_;
			this.lowerArmLocalRot = lowerArmLocalRot_;
		}

		// Token: 0x04002609 RID: 9737
		public Quaternion upperArmLocalRot;

		// Token: 0x0400260A RID: 9738
		public Quaternion lowerArmLocalRot;
	}

	// Token: 0x02000575 RID: 1397
	[BurstCompile]
	private struct IKJob : IJobParallelFor
	{
		// Token: 0x06002278 RID: 8824 RVA: 0x000F8180 File Offset: 0x000F6380
		public void Execute(int i)
		{
			Quaternion initRotUpper = this.constantInput[i].initRotUpper;
			Vector3 vector = GorillaIKMgr.IKJob.upperArmLocalPos;
			Quaternion rotation = initRotUpper * this.constantInput[i].initRotLower;
			Vector3 vector2 = vector + initRotUpper * GorillaIKMgr.IKJob.forearmLocalPos;
			Vector3 vector3 = vector2 + rotation * GorillaIKMgr.IKJob.handLocalPos;
			float num = 0f;
			float magnitude = (vector - vector2).magnitude;
			float magnitude2 = (vector2 - vector3).magnitude;
			float max = magnitude + magnitude2 - num;
			Vector3 normalized = (vector3 - vector).normalized;
			Vector3 normalized2 = (vector2 - vector).normalized;
			Vector3 normalized3 = (vector3 - vector2).normalized;
			Vector3 normalized4 = (this.input[i].targetPos - vector).normalized;
			float num2 = Mathf.Clamp((this.input[i].targetPos - vector).magnitude, num, max);
			float num3 = Mathf.Acos(Mathf.Clamp(Vector3.Dot(normalized, normalized2), -1f, 1f));
			float num4 = Mathf.Acos(Mathf.Clamp(Vector3.Dot(-normalized2, normalized3), -1f, 1f));
			float num5 = Mathf.Acos(Mathf.Clamp(Vector3.Dot(normalized, normalized4), -1f, 1f));
			float num6 = Mathf.Acos(Mathf.Clamp((magnitude2 * magnitude2 - magnitude * magnitude - num2 * num2) / (-2f * magnitude * num2), -1f, 1f));
			float num7 = Mathf.Acos(Mathf.Clamp((num2 * num2 - magnitude * magnitude - magnitude2 * magnitude2) / (-2f * magnitude * magnitude2), -1f, 1f));
			Vector3 normalized5 = Vector3.Cross(normalized, normalized2).normalized;
			Vector3 normalized6 = Vector3.Cross(normalized, normalized4).normalized;
			Quaternion rhs = Quaternion.AngleAxis((num6 - num3) * 57.29578f, Quaternion.Inverse(initRotUpper) * normalized5);
			Quaternion rhs2 = Quaternion.AngleAxis((num7 - num4) * 57.29578f, Quaternion.Inverse(rotation) * normalized5);
			Quaternion rhs3 = Quaternion.AngleAxis(num5 * 57.29578f, Quaternion.Inverse(initRotUpper) * normalized6);
			Quaternion upperArmLocalRot_ = this.constantInput[i].initRotUpper * rhs3 * rhs;
			Quaternion lowerArmLocalRot_ = this.constantInput[i].initRotLower * rhs2;
			this.output[i] = new GorillaIKMgr.IKOutput(upperArmLocalRot_, lowerArmLocalRot_);
		}

		// Token: 0x0400260B RID: 9739
		public NativeArray<GorillaIKMgr.IKConstantInput> constantInput;

		// Token: 0x0400260C RID: 9740
		public NativeArray<GorillaIKMgr.IKInput> input;

		// Token: 0x0400260D RID: 9741
		public NativeArray<GorillaIKMgr.IKOutput> output;

		// Token: 0x0400260E RID: 9742
		private static readonly Vector3 upperArmLocalPos = new Vector3(-0.0002577677f, 0.1454885f, -0.02598158f);

		// Token: 0x0400260F RID: 9743
		private static readonly Vector3 forearmLocalPos = new Vector3(4.204223E-06f, 0.4061671f, -1.043081E-06f);

		// Token: 0x04002610 RID: 9744
		private static readonly Vector3 handLocalPos = new Vector3(3.073364E-08f, 0.3816895f, 1.117587E-08f);
	}

	// Token: 0x02000576 RID: 1398
	[BurstCompile]
	private struct IKTransformJob : IJobParallelForTransform
	{
		// Token: 0x0600227A RID: 8826 RVA: 0x00046552 File Offset: 0x00044752
		public void Execute(int index, TransformAccess xform)
		{
			if (index % 7 <= 3)
			{
				xform.localRotation = this.transformRotations[index];
				return;
			}
			xform.rotation = this.transformRotations[index];
		}

		// Token: 0x04002611 RID: 9745
		public NativeArray<Quaternion> transformRotations;
	}
}
