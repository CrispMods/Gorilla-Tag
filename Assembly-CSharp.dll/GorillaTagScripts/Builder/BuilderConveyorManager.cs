using System;
using Photon.Pun;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Splines;

namespace GorillaTagScripts.Builder
{
	// Token: 0x020009F8 RID: 2552
	public class BuilderConveyorManager : MonoBehaviour
	{
		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06003FAE RID: 16302 RVA: 0x00058CD4 File Offset: 0x00056ED4
		// (set) Token: 0x06003FAF RID: 16303 RVA: 0x00058CDB File Offset: 0x00056EDB
		public static BuilderConveyorManager instance { get; private set; }

		// Token: 0x06003FB0 RID: 16304 RVA: 0x00058CE3 File Offset: 0x00056EE3
		private void Awake()
		{
			if (BuilderConveyorManager.instance != null && BuilderConveyorManager.instance != this)
			{
				UnityEngine.Object.Destroy(this);
			}
			if (BuilderConveyorManager.instance == null)
			{
				BuilderConveyorManager.instance = this;
			}
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x00167D68 File Offset: 0x00165F68
		public void UpdateManager()
		{
			foreach (BuilderConveyor builderConveyor in this.table.conveyors)
			{
				builderConveyor.UpdateConveyor();
			}
			bool flag = false;
			bool flag2 = this.pieceTransforms.length >= this.pieceTransforms.capacity - 5;
			for (int i = this.jobSplineTimes.Length - 1; i >= 0; i--)
			{
				BuilderConveyor builderConveyor2 = this.table.conveyors[this.conveyorIndices[i]];
				float num = Time.deltaTime * builderConveyor2.GetFrameMovement();
				float num2 = this.jobSplineTimes[i] + num;
				this.jobSplineTimes[i] = Mathf.Clamp(num2, 0f, 1f);
				if (PhotonNetwork.IsMasterClient && (!flag || flag2) && (double)num2 > 0.999)
				{
					builderConveyor2.RemovePieceFromConveyor(this.pieceTransforms[i]);
					this.RemovePieceFromJobAtIndex(i);
					flag = true;
				}
			}
			for (int j = this.shelfSlice; j < this.table.conveyors.Count; j += BuilderTable.SHELF_SLICE_BUCKETS)
			{
				this.table.conveyors[j].UpdateShelfSliced();
			}
			this.shelfSlice = (this.shelfSlice + 1) % BuilderTable.SHELF_SLICE_BUCKETS;
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x00167EE4 File Offset: 0x001660E4
		public void Setup(BuilderTable mytable)
		{
			if (this.isSetup)
			{
				return;
			}
			this.table = mytable;
			this.conveyorSplines = new NativeArray<NativeSpline>(this.table.conveyors.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.conveyorRotations = new NativeArray<Quaternion>(this.table.conveyors.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			int num = 0;
			for (int i = 0; i < this.table.conveyors.Count; i++)
			{
				this.conveyorSplines[i] = this.table.conveyors[i].nativeSpline;
				this.conveyorRotations[i] = this.table.conveyors[i].GetSpawnTransform().rotation;
				num += this.table.conveyors[i].GetMaxItemsOnConveyor();
			}
			this.maxItemCount = num;
			this.conveyorIndices = new NativeList<int>(this.maxItemCount, Allocator.Persistent);
			this.jobSplineTimes = new NativeList<float>(this.maxItemCount, Allocator.Persistent);
			this.jobShelfOffsets = new NativeList<Vector3>(this.maxItemCount, Allocator.Persistent);
			this.pieceTransforms = new TransformAccessArray(this.maxItemCount, 3);
			this.isSetup = true;
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x00168020 File Offset: 0x00166220
		public float GetSplineProgressForPiece(BuilderPiece piece)
		{
			for (int i = 0; i < this.pieceTransforms.length; i++)
			{
				if (this.pieceTransforms[i] == piece.transform)
				{
					return this.jobSplineTimes[i];
				}
			}
			return 0f;
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x00168070 File Offset: 0x00166270
		public int GetPieceCreateTimestamp(BuilderPiece piece)
		{
			for (int i = 0; i < this.pieceTransforms.length; i++)
			{
				if (this.pieceTransforms[i] == piece.transform)
				{
					BuilderConveyor builderConveyor = this.table.conveyors[this.conveyorIndices[i]];
					int num = Mathf.RoundToInt(this.jobSplineTimes[i] / builderConveyor.GetFrameMovement() * 1000f);
					return PhotonNetwork.ServerTimestamp - num;
				}
			}
			return 0;
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x001680F4 File Offset: 0x001662F4
		public void OnClearTable()
		{
			foreach (BuilderConveyor builderConveyor in this.table.conveyors)
			{
				builderConveyor.OnClearTable();
			}
			for (int i = this.pieceTransforms.length - 1; i >= 0; i--)
			{
				this.pieceTransforms.RemoveAtSwapBack(i);
			}
			this.jobSplineTimes.Clear();
			this.jobShelfOffsets.Clear();
			this.conveyorIndices.Clear();
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x00168190 File Offset: 0x00166390
		private void OnDestroy()
		{
			this.conveyorSplines.Dispose();
			this.conveyorRotations.Dispose();
			this.conveyorIndices.Dispose();
			this.jobSplineTimes.Dispose();
			this.jobShelfOffsets.Dispose();
			this.pieceTransforms.Dispose();
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x001681E0 File Offset: 0x001663E0
		public JobHandle ConstructJobHandle()
		{
			BuilderConveyorManager.EvaluateSplineJob jobData = new BuilderConveyorManager.EvaluateSplineJob
			{
				conveyorRotations = this.conveyorRotations,
				conveyorIndices = this.conveyorIndices,
				shelfOffsets = this.jobShelfOffsets,
				splineTimes = this.jobSplineTimes
			};
			for (int i = 0; i < this.conveyorSplines.Length; i++)
			{
				jobData.SetSplineAt(i, this.conveyorSplines[i]);
			}
			return jobData.Schedule(this.pieceTransforms, default(JobHandle));
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x0016826C File Offset: 0x0016646C
		public void AddPieceToJob(BuilderPiece piece, float splineTime, int conveyorID)
		{
			if (this.pieceTransforms.length >= this.pieceTransforms.capacity)
			{
				Debug.LogError("Too many pieces on conveyor!");
			}
			this.pieceTransforms.Add(piece.transform);
			this.conveyorIndices.Add(conveyorID);
			this.jobShelfOffsets.Add(piece.desiredShelfOffset);
			this.jobSplineTimes.Add(splineTime);
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x00058D18 File Offset: 0x00056F18
		public void RemovePieceFromJobAtIndex(int index)
		{
			BuilderRenderer.RemoveAt(this.pieceTransforms, index);
			this.jobShelfOffsets.RemoveAt(index);
			this.jobSplineTimes.RemoveAt(index);
			this.conveyorIndices.RemoveAt(index);
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x001682D8 File Offset: 0x001664D8
		public void RemovePieceFromJob(BuilderPiece piece)
		{
			for (int i = 0; i < this.pieceTransforms.length; i++)
			{
				if (this.pieceTransforms[i] == piece.transform)
				{
					BuilderRenderer.RemoveAt(this.pieceTransforms, i);
					this.jobShelfOffsets.RemoveAt(i);
					this.jobSplineTimes.RemoveAt(i);
					this.conveyorIndices.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x040040B7 RID: 16567
		private NativeArray<NativeSpline> conveyorSplines;

		// Token: 0x040040B8 RID: 16568
		private NativeArray<Quaternion> conveyorRotations;

		// Token: 0x040040B9 RID: 16569
		private NativeList<int> conveyorIndices;

		// Token: 0x040040BA RID: 16570
		private NativeList<float> jobSplineTimes;

		// Token: 0x040040BB RID: 16571
		private NativeList<Vector3> jobShelfOffsets;

		// Token: 0x040040BC RID: 16572
		private TransformAccessArray pieceTransforms;

		// Token: 0x040040BD RID: 16573
		private BuilderTable table;

		// Token: 0x040040BE RID: 16574
		private bool isSetup;

		// Token: 0x040040BF RID: 16575
		private int maxItemCount;

		// Token: 0x040040C0 RID: 16576
		private int shelfSlice;

		// Token: 0x020009F9 RID: 2553
		[BurstCompile]
		public struct EvaluateSplineJob : IJobParallelForTransform
		{
			// Token: 0x06003FBC RID: 16316 RVA: 0x00058D4A File Offset: 0x00056F4A
			public NativeSpline GetSplineAt(int index)
			{
				switch (index)
				{
				case 0:
					return this.conveyorSpline0;
				case 1:
					return this.conveyorSpline1;
				case 2:
					return this.conveyorSpline2;
				case 3:
					return this.conveyorSpline3;
				default:
					return this.conveyorSpline0;
				}
			}

			// Token: 0x06003FBD RID: 16317 RVA: 0x00058D86 File Offset: 0x00056F86
			public void SetSplineAt(int index, NativeSpline s)
			{
				switch (index)
				{
				case 0:
					this.conveyorSpline0 = s;
					return;
				case 1:
					this.conveyorSpline1 = s;
					return;
				case 2:
					this.conveyorSpline2 = s;
					return;
				case 3:
					this.conveyorSpline3 = s;
					return;
				default:
					return;
				}
			}

			// Token: 0x06003FBE RID: 16318 RVA: 0x00168348 File Offset: 0x00166548
			public void Execute(int index, TransformAccess transform)
			{
				float splineT = this.splineTimes[index];
				Vector3 point = this.shelfOffsets[index];
				int index2 = this.conveyorIndices[index];
				NativeSpline splineAt = this.GetSplineAt(index2);
				Quaternion rotation = this.conveyorRotations[index2];
				float t;
				Vector3 position = CurveUtility.EvaluatePosition(splineAt.GetCurve(splineAt.SplineToCurveT(splineT, out t)), t) + rotation * point;
				transform.position = position;
			}

			// Token: 0x040040C2 RID: 16578
			public NativeSpline conveyorSpline0;

			// Token: 0x040040C3 RID: 16579
			public NativeSpline conveyorSpline1;

			// Token: 0x040040C4 RID: 16580
			public NativeSpline conveyorSpline2;

			// Token: 0x040040C5 RID: 16581
			public NativeSpline conveyorSpline3;

			// Token: 0x040040C6 RID: 16582
			[ReadOnly]
			public NativeArray<Quaternion> conveyorRotations;

			// Token: 0x040040C7 RID: 16583
			[ReadOnly]
			public NativeList<int> conveyorIndices;

			// Token: 0x040040C8 RID: 16584
			[ReadOnly]
			public NativeList<float> splineTimes;

			// Token: 0x040040C9 RID: 16585
			[ReadOnly]
			public NativeList<Vector3> shelfOffsets;
		}
	}
}
