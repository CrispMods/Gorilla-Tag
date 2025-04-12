using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

// Token: 0x02000696 RID: 1686
[DefaultExecutionOrder(0)]
public class VRRigJobManager : MonoBehaviour
{
	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x060029EB RID: 10731 RVA: 0x0004B7E1 File Offset: 0x000499E1
	public static VRRigJobManager Instance
	{
		get
		{
			return VRRigJobManager._instance;
		}
	}

	// Token: 0x060029EC RID: 10732 RVA: 0x0004B7E8 File Offset: 0x000499E8
	private void Awake()
	{
		VRRigJobManager._instance = this;
		this.cachedInput = new NativeArray<VRRigJobManager.VRRigTransformInput>(9, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.tAA = new TransformAccessArray(9, 2);
	}

	// Token: 0x060029ED RID: 10733 RVA: 0x0004B80D File Offset: 0x00049A0D
	private void OnDestroy()
	{
		this.jobHandle.Complete();
		this.cachedInput.Dispose();
		this.tAA.Dispose();
	}

	// Token: 0x060029EE RID: 10734 RVA: 0x0004B830 File Offset: 0x00049A30
	public void RegisterVRRig(VRRig rig)
	{
		this.rigList.Add(rig);
		this.tAA.Add(rig.transform);
		this.actualListSz++;
	}

	// Token: 0x060029EF RID: 10735 RVA: 0x0011800C File Offset: 0x0011620C
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

	// Token: 0x060029F0 RID: 10736 RVA: 0x00118078 File Offset: 0x00116278
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

	// Token: 0x060029F1 RID: 10737 RVA: 0x001180F8 File Offset: 0x001162F8
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

	// Token: 0x04002F61 RID: 12129
	[OnEnterPlay_SetNull]
	private static VRRigJobManager _instance;

	// Token: 0x04002F62 RID: 12130
	private const int MaxSize = 9;

	// Token: 0x04002F63 RID: 12131
	private const int questJobThreads = 2;

	// Token: 0x04002F64 RID: 12132
	private List<VRRig> rigList = new List<VRRig>(9);

	// Token: 0x04002F65 RID: 12133
	private NativeArray<VRRigJobManager.VRRigTransformInput> cachedInput;

	// Token: 0x04002F66 RID: 12134
	private TransformAccessArray tAA;

	// Token: 0x04002F67 RID: 12135
	private int actualListSz;

	// Token: 0x04002F68 RID: 12136
	private JobHandle jobHandle;

	// Token: 0x02000697 RID: 1687
	private struct VRRigTransformInput
	{
		// Token: 0x04002F69 RID: 12137
		public Vector3 rigPosition;

		// Token: 0x04002F6A RID: 12138
		public Quaternion rigRotaton;
	}

	// Token: 0x02000698 RID: 1688
	[BurstCompile]
	private struct VRRigTransformJob : IJobParallelForTransform
	{
		// Token: 0x060029F3 RID: 10739 RVA: 0x0004B872 File Offset: 0x00049A72
		public void Execute(int i, TransformAccess tA)
		{
			if (i < this.input.Length)
			{
				tA.position = this.input[i].rigPosition;
				tA.rotation = this.input[i].rigRotaton;
			}
		}

		// Token: 0x04002F6B RID: 12139
		[ReadOnly]
		public NativeArray<VRRigJobManager.VRRigTransformInput> input;
	}
}
