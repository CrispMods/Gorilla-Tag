using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

// Token: 0x02000695 RID: 1685
[DefaultExecutionOrder(0)]
public class VRRigJobManager : MonoBehaviour
{
	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x060029E3 RID: 10723 RVA: 0x000D02CA File Offset: 0x000CE4CA
	public static VRRigJobManager Instance
	{
		get
		{
			return VRRigJobManager._instance;
		}
	}

	// Token: 0x060029E4 RID: 10724 RVA: 0x000D02D1 File Offset: 0x000CE4D1
	private void Awake()
	{
		VRRigJobManager._instance = this;
		this.cachedInput = new NativeArray<VRRigJobManager.VRRigTransformInput>(9, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.tAA = new TransformAccessArray(9, 2);
	}

	// Token: 0x060029E5 RID: 10725 RVA: 0x000D02F6 File Offset: 0x000CE4F6
	private void OnDestroy()
	{
		this.jobHandle.Complete();
		this.cachedInput.Dispose();
		this.tAA.Dispose();
	}

	// Token: 0x060029E6 RID: 10726 RVA: 0x000D0319 File Offset: 0x000CE519
	public void RegisterVRRig(VRRig rig)
	{
		this.rigList.Add(rig);
		this.tAA.Add(rig.transform);
		this.actualListSz++;
	}

	// Token: 0x060029E7 RID: 10727 RVA: 0x000D0348 File Offset: 0x000CE548
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

	// Token: 0x060029E8 RID: 10728 RVA: 0x000D03B4 File Offset: 0x000CE5B4
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

	// Token: 0x060029E9 RID: 10729 RVA: 0x000D0434 File Offset: 0x000CE634
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

	// Token: 0x04002F5B RID: 12123
	[OnEnterPlay_SetNull]
	private static VRRigJobManager _instance;

	// Token: 0x04002F5C RID: 12124
	private const int MaxSize = 9;

	// Token: 0x04002F5D RID: 12125
	private const int questJobThreads = 2;

	// Token: 0x04002F5E RID: 12126
	private List<VRRig> rigList = new List<VRRig>(9);

	// Token: 0x04002F5F RID: 12127
	private NativeArray<VRRigJobManager.VRRigTransformInput> cachedInput;

	// Token: 0x04002F60 RID: 12128
	private TransformAccessArray tAA;

	// Token: 0x04002F61 RID: 12129
	private int actualListSz;

	// Token: 0x04002F62 RID: 12130
	private JobHandle jobHandle;

	// Token: 0x02000696 RID: 1686
	private struct VRRigTransformInput
	{
		// Token: 0x04002F63 RID: 12131
		public Vector3 rigPosition;

		// Token: 0x04002F64 RID: 12132
		public Quaternion rigRotaton;
	}

	// Token: 0x02000697 RID: 1687
	[BurstCompile]
	private struct VRRigTransformJob : IJobParallelForTransform
	{
		// Token: 0x060029EB RID: 10731 RVA: 0x000D04C0 File Offset: 0x000CE6C0
		public void Execute(int i, TransformAccess tA)
		{
			if (i < this.input.Length)
			{
				tA.position = this.input[i].rigPosition;
				tA.rotation = this.input[i].rigRotaton;
			}
		}

		// Token: 0x04002F65 RID: 12133
		[ReadOnly]
		public NativeArray<VRRigJobManager.VRRigTransformInput> input;
	}
}
