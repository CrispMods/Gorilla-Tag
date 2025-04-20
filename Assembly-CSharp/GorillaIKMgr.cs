using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

// Token: 0x0200057E RID: 1406
public class GorillaIKMgr : MonoBehaviour
{
	// Token: 0x17000386 RID: 902
	// (get) Token: 0x060022C3 RID: 8899 RVA: 0x000478BD File Offset: 0x00045ABD
	public static GorillaIKMgr Instance
	{
		get
		{
			return GorillaIKMgr._instance;
		}
	}

	// Token: 0x060022C4 RID: 8900 RVA: 0x000FA910 File Offset: 0x000F8B10
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

	// Token: 0x060022C5 RID: 8901 RVA: 0x000FA9A4 File Offset: 0x000F8BA4
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

	// Token: 0x060022C6 RID: 8902 RVA: 0x000FAA14 File Offset: 0x000F8C14
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

	// Token: 0x060022C7 RID: 8903 RVA: 0x000FAA64 File Offset: 0x000F8C64
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

	// Token: 0x060022C8 RID: 8904 RVA: 0x000FAB00 File Offset: 0x000F8D00
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

	// Token: 0x060022C9 RID: 8905 RVA: 0x000FAB78 File Offset: 0x000F8D78
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

	// Token: 0x060022CA RID: 8906 RVA: 0x000FAC2C File Offset: 0x000F8E2C
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

	// Token: 0x060022CB RID: 8907 RVA: 0x000FAE7C File Offset: 0x000F907C
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

	// Token: 0x0400264B RID: 9803
	[OnEnterPlay_SetNull]
	private static GorillaIKMgr _instance;

	// Token: 0x0400264C RID: 9804
	private const int MaxSize = 20;

	// Token: 0x0400264D RID: 9805
	private List<GorillaIK> ikList = new List<GorillaIK>(20);

	// Token: 0x0400264E RID: 9806
	private int actualListSz;

	// Token: 0x0400264F RID: 9807
	private JobHandle jobHandle;

	// Token: 0x04002650 RID: 9808
	private JobHandle jobXformHandle;

	// Token: 0x04002651 RID: 9809
	private bool firstFrame = true;

	// Token: 0x04002652 RID: 9810
	private TransformAccessArray tAA;

	// Token: 0x04002653 RID: 9811
	private List<Transform> transformList;

	// Token: 0x04002654 RID: 9812
	private bool updatedSinceLastRun;

	// Token: 0x04002655 RID: 9813
	private int tFormCount = 7;

	// Token: 0x04002656 RID: 9814
	private GorillaIKMgr.IKJob job;

	// Token: 0x04002657 RID: 9815
	private GorillaIKMgr.IKTransformJob jobXform;

	// Token: 0x0200057F RID: 1407
	private struct IKConstantInput
	{
		// Token: 0x04002658 RID: 9816
		public Quaternion initRotLower;

		// Token: 0x04002659 RID: 9817
		public Quaternion initRotUpper;
	}

	// Token: 0x02000580 RID: 1408
	private struct IKInput
	{
		// Token: 0x0400265A RID: 9818
		public Vector3 targetPos;
	}

	// Token: 0x02000581 RID: 1409
	private struct IKOutput
	{
		// Token: 0x060022CD RID: 8909 RVA: 0x000478E7 File Offset: 0x00045AE7
		public IKOutput(Quaternion upperArmLocalRot_, Quaternion lowerArmLocalRot_)
		{
			this.upperArmLocalRot = upperArmLocalRot_;
			this.lowerArmLocalRot = lowerArmLocalRot_;
		}

		// Token: 0x0400265B RID: 9819
		public Quaternion upperArmLocalRot;

		// Token: 0x0400265C RID: 9820
		public Quaternion lowerArmLocalRot;
	}

	// Token: 0x02000582 RID: 1410
	[BurstCompile]
	private struct IKJob : IJobParallelFor
	{
		// Token: 0x060022CE RID: 8910 RVA: 0x000FAEFC File Offset: 0x000F90FC
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

		// Token: 0x0400265D RID: 9821
		public NativeArray<GorillaIKMgr.IKConstantInput> constantInput;

		// Token: 0x0400265E RID: 9822
		public NativeArray<GorillaIKMgr.IKInput> input;

		// Token: 0x0400265F RID: 9823
		public NativeArray<GorillaIKMgr.IKOutput> output;

		// Token: 0x04002660 RID: 9824
		private static readonly Vector3 upperArmLocalPos = new Vector3(-0.0002577677f, 0.1454885f, -0.02598158f);

		// Token: 0x04002661 RID: 9825
		private static readonly Vector3 forearmLocalPos = new Vector3(4.204223E-06f, 0.4061671f, -1.043081E-06f);

		// Token: 0x04002662 RID: 9826
		private static readonly Vector3 handLocalPos = new Vector3(3.073364E-08f, 0.3816895f, 1.117587E-08f);
	}

	// Token: 0x02000583 RID: 1411
	[BurstCompile]
	private struct IKTransformJob : IJobParallelForTransform
	{
		// Token: 0x060022D0 RID: 8912 RVA: 0x000478F7 File Offset: 0x00045AF7
		public void Execute(int index, TransformAccess xform)
		{
			if (index % 7 <= 3)
			{
				xform.localRotation = this.transformRotations[index];
				return;
			}
			xform.rotation = this.transformRotations[index];
		}

		// Token: 0x04002663 RID: 9827
		public NativeArray<Quaternion> transformRotations;
	}
}
