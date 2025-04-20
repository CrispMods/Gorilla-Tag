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
	// Token: 0x02000A22 RID: 2594
	public class BuilderConveyorManager : MonoBehaviour
	{
		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x060040E7 RID: 16615 RVA: 0x0005A6D6 File Offset: 0x000588D6
		// (set) Token: 0x060040E8 RID: 16616 RVA: 0x0005A6DD File Offset: 0x000588DD
		public static BuilderConveyorManager instance { get; private set; }

		// Token: 0x060040E9 RID: 16617 RVA: 0x0005A6E5 File Offset: 0x000588E5
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

		// Token: 0x060040EA RID: 16618 RVA: 0x0016EB5C File Offset: 0x0016CD5C
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

		// Token: 0x060040EB RID: 16619 RVA: 0x0016ECD8 File Offset: 0x0016CED8
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

		// Token: 0x060040EC RID: 16620 RVA: 0x0016EE14 File Offset: 0x0016D014
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

		// Token: 0x060040ED RID: 16621 RVA: 0x0016EE64 File Offset: 0x0016D064
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

		// Token: 0x060040EE RID: 16622 RVA: 0x0016EEE8 File Offset: 0x0016D0E8
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

		// Token: 0x060040EF RID: 16623 RVA: 0x0016EF84 File Offset: 0x0016D184
		private void OnDestroy()
		{
			this.conveyorSplines.Dispose();
			this.conveyorRotations.Dispose();
			this.conveyorIndices.Dispose();
			this.jobSplineTimes.Dispose();
			this.jobShelfOffsets.Dispose();
			this.pieceTransforms.Dispose();
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x0016EFD4 File Offset: 0x0016D1D4
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

		// Token: 0x060040F1 RID: 16625 RVA: 0x0016F060 File Offset: 0x0016D260
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

		// Token: 0x060040F2 RID: 16626 RVA: 0x0005A71A File Offset: 0x0005891A
		public void RemovePieceFromJobAtIndex(int index)
		{
			BuilderRenderer.RemoveAt(this.pieceTransforms, index);
			this.jobShelfOffsets.RemoveAt(index);
			this.jobSplineTimes.RemoveAt(index);
			this.conveyorIndices.RemoveAt(index);
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x0016F0CC File Offset: 0x0016D2CC
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

		// Token: 0x0400419F RID: 16799
		private NativeArray<NativeSpline> conveyorSplines;

		// Token: 0x040041A0 RID: 16800
		private NativeArray<Quaternion> conveyorRotations;

		// Token: 0x040041A1 RID: 16801
		private NativeList<int> conveyorIndices;

		// Token: 0x040041A2 RID: 16802
		private NativeList<float> jobSplineTimes;

		// Token: 0x040041A3 RID: 16803
		private NativeList<Vector3> jobShelfOffsets;

		// Token: 0x040041A4 RID: 16804
		private TransformAccessArray pieceTransforms;

		// Token: 0x040041A5 RID: 16805
		private BuilderTable table;

		// Token: 0x040041A6 RID: 16806
		private bool isSetup;

		// Token: 0x040041A7 RID: 16807
		private int maxItemCount;

		// Token: 0x040041A8 RID: 16808
		private int shelfSlice;

		// Token: 0x02000A23 RID: 2595
		[BurstCompile]
		public struct EvaluateSplineJob : IJobParallelForTransform
		{
			// Token: 0x060040F5 RID: 16629 RVA: 0x0005A74C File Offset: 0x0005894C
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

			// Token: 0x060040F6 RID: 16630 RVA: 0x0005A788 File Offset: 0x00058988
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

			// Token: 0x060040F7 RID: 16631 RVA: 0x0016F13C File Offset: 0x0016D33C
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

			// Token: 0x040041AA RID: 16810
			public NativeSpline conveyorSpline0;

			// Token: 0x040041AB RID: 16811
			public NativeSpline conveyorSpline1;

			// Token: 0x040041AC RID: 16812
			public NativeSpline conveyorSpline2;

			// Token: 0x040041AD RID: 16813
			public NativeSpline conveyorSpline3;

			// Token: 0x040041AE RID: 16814
			[ReadOnly]
			public NativeArray<Quaternion> conveyorRotations;

			// Token: 0x040041AF RID: 16815
			[ReadOnly]
			public NativeList<int> conveyorIndices;

			// Token: 0x040041B0 RID: 16816
			[ReadOnly]
			public NativeList<float> splineTimes;

			// Token: 0x040041B1 RID: 16817
			[ReadOnly]
			public NativeList<Vector3> shelfOffsets;
		}
	}
}
