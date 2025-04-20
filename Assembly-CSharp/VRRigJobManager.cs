using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

// Token: 0x020006AA RID: 1706
[DefaultExecutionOrder(0)]
public class VRRigJobManager : MonoBehaviour
{
	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x06002A79 RID: 10873 RVA: 0x0004CB26 File Offset: 0x0004AD26
	public static VRRigJobManager Instance
	{
		get
		{
			return VRRigJobManager._instance;
		}
	}

	// Token: 0x06002A7A RID: 10874 RVA: 0x0004CB2D File Offset: 0x0004AD2D
	private void Awake()
	{
		VRRigJobManager._instance = this;
		this.cachedInput = new NativeArray<VRRigJobManager.VRRigTransformInput>(9, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.tAA = new TransformAccessArray(9, 2);
	}

	// Token: 0x06002A7B RID: 10875 RVA: 0x0004CB52 File Offset: 0x0004AD52
	private void OnDestroy()
	{
		this.jobHandle.Complete();
		this.cachedInput.Dispose();
		this.tAA.Dispose();
	}

	// Token: 0x06002A7C RID: 10876 RVA: 0x0004CB75 File Offset: 0x0004AD75
	public void RegisterVRRig(VRRig rig)
	{
		this.rigList.Add(rig);
		this.tAA.Add(rig.transform);
		this.actualListSz++;
	}

	// Token: 0x06002A7D RID: 10877 RVA: 0x0011CBC4 File Offset: 0x0011ADC4
	public void DeregisterVRRig(VRRig rig)
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		this.rigList.Remove(rig);
		for (int i = this.actualListSz - 1; i >= 0; i--)
		{
			if (this.tAA[i] == rig.transform)
			{
				this.tAA.RemoveAtSwapBack(i);
				break;
			}
		}
		this.actualListSz--;
	}

	// Token: 0x06002A7E RID: 10878 RVA: 0x0011CC30 File Offset: 0x0011AE30
	private void CopyInput()
	{
		for (int i = 0; i < this.actualListSz; i++)
		{
			this.cachedInput[i] = new VRRigJobManager.VRRigTransformInput
			{
				rigPosition = this.rigList[i].jobPos,
				rigRotaton = this.rigList[i].jobRotation
			};
			this.tAA[i] = this.rigList[i].transform;
		}
	}

	// Token: 0x06002A7F RID: 10879 RVA: 0x0011CCB0 File Offset: 0x0011AEB0
	public void Update()
	{
		this.jobHandle.Complete();
		for (int i = 0; i < this.rigList.Count; i++)
		{
			this.rigList[i].RemoteRigUpdate();
		}
		this.CopyInput();
		VRRigJobManager.VRRigTransformJob jobData = new VRRigJobManager.VRRigTransformJob
		{
			input = this.cachedInput
		};
		this.jobHandle = jobData.Schedule(this.tAA, default(JobHandle));
	}

	// Token: 0x04002FF8 RID: 12280
	[OnEnterPlay_SetNull]
	private static VRRigJobManager _instance;

	// Token: 0x04002FF9 RID: 12281
	private const int MaxSize = 9;

	// Token: 0x04002FFA RID: 12282
	private const int questJobThreads = 2;

	// Token: 0x04002FFB RID: 12283
	private List<VRRig> rigList = new List<VRRig>(9);

	// Token: 0x04002FFC RID: 12284
	private NativeArray<VRRigJobManager.VRRigTransformInput> cachedInput;

	// Token: 0x04002FFD RID: 12285
	private TransformAccessArray tAA;

	// Token: 0x04002FFE RID: 12286
	private int actualListSz;

	// Token: 0x04002FFF RID: 12287
	private JobHandle jobHandle;

	// Token: 0x020006AB RID: 1707
	private struct VRRigTransformInput
	{
		// Token: 0x04003000 RID: 12288
		public Vector3 rigPosition;

		// Token: 0x04003001 RID: 12289
		public Quaternion rigRotaton;
	}

	// Token: 0x020006AC RID: 1708
	[BurstCompile]
	private struct VRRigTransformJob : IJobParallelForTransform
	{
		// Token: 0x06002A81 RID: 10881 RVA: 0x0004CBB7 File Offset: 0x0004ADB7
		public void Execute(int i, TransformAccess tA)
		{
			if (i < this.input.Length)
			{
				tA.position = this.input[i].rigPosition;
				tA.rotation = this.input[i].rigRotaton;
			}
		}

		// Token: 0x04003002 RID: 12290
		[ReadOnly]
		public NativeArray<VRRigJobManager.VRRigTransformInput> input;
	}
}
