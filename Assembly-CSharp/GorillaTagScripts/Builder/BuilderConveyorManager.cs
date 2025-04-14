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
	// Token: 0x020009F5 RID: 2549
	public class BuilderConveyorManager : MonoBehaviour
	{
		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06003FA2 RID: 16290 RVA: 0x0012D87F File Offset: 0x0012BA7F
		// (set) Token: 0x06003FA3 RID: 16291 RVA: 0x0012D886 File Offset: 0x0012BA86
		public static BuilderConveyorManager instance { get; private set; }

		// Token: 0x06003FA4 RID: 16292 RVA: 0x0012D88E File Offset: 0x0012BA8E
		private void Awake()
		{
			if (BuilderConveyorManager.instance != null && BuilderConveyorManager.instance != this)
			{
				Object.Destroy(this);
			}
			if (BuilderConveyorManager.instance == null)
			{
				BuilderConveyorManager.instance = this;
			}
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x0012D8C4 File Offset: 0x0012BAC4
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

		// Token: 0x06003FA6 RID: 16294 RVA: 0x0012DA40 File Offset: 0x0012BC40
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

		// Token: 0x06003FA7 RID: 16295 RVA: 0x0012DB7C File Offset: 0x0012BD7C
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

		// Token: 0x06003FA8 RID: 16296 RVA: 0x0012DBCC File Offset: 0x0012BDCC
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

		// Token: 0x06003FA9 RID: 16297 RVA: 0x0012DC50 File Offset: 0x0012BE50
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

		// Token: 0x06003FAA RID: 16298 RVA: 0x0012DCEC File Offset: 0x0012BEEC
		private void OnDestroy()
		{
			this.conveyorSplines.Dispose();
			this.conveyorRotations.Dispose();
			this.conveyorIndices.Dispose();
			this.jobSplineTimes.Dispose();
			this.jobShelfOffsets.Dispose();
			this.pieceTransforms.Dispose();
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x0012DD3C File Offset: 0x0012BF3C
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

		// Token: 0x06003FAC RID: 16300 RVA: 0x0012DDC8 File Offset: 0x0012BFC8
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

		// Token: 0x06003FAD RID: 16301 RVA: 0x0012DE33 File Offset: 0x0012C033
		public void RemovePieceFromJobAtIndex(int index)
		{
			BuilderRenderer.RemoveAt(this.pieceTransforms, index);
			this.jobShelfOffsets.RemoveAt(index);
			this.jobSplineTimes.RemoveAt(index);
			this.conveyorIndices.RemoveAt(index);
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x0012DE68 File Offset: 0x0012C068
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

		// Token: 0x040040A5 RID: 16549
		private NativeArray<NativeSpline> conveyorSplines;

		// Token: 0x040040A6 RID: 16550
		private NativeArray<Quaternion> conveyorRotations;

		// Token: 0x040040A7 RID: 16551
		private NativeList<int> conveyorIndices;

		// Token: 0x040040A8 RID: 16552
		private NativeList<float> jobSplineTimes;

		// Token: 0x040040A9 RID: 16553
		private NativeList<Vector3> jobShelfOffsets;

		// Token: 0x040040AA RID: 16554
		private TransformAccessArray pieceTransforms;

		// Token: 0x040040AB RID: 16555
		private BuilderTable table;

		// Token: 0x040040AC RID: 16556
		private bool isSetup;

		// Token: 0x040040AD RID: 16557
		private int maxItemCount;

		// Token: 0x040040AE RID: 16558
		private int shelfSlice;

		// Token: 0x020009F6 RID: 2550
		[BurstCompile]
		public struct EvaluateSplineJob : IJobParallelForTransform
		{
			// Token: 0x06003FB0 RID: 16304 RVA: 0x0012DED5 File Offset: 0x0012C0D5
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

			// Token: 0x06003FB1 RID: 16305 RVA: 0x0012DF11 File Offset: 0x0012C111
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

			// Token: 0x06003FB2 RID: 16306 RVA: 0x0012DF4C File Offset: 0x0012C14C
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

			// Token: 0x040040B0 RID: 16560
			public NativeSpline conveyorSpline0;

			// Token: 0x040040B1 RID: 16561
			public NativeSpline conveyorSpline1;

			// Token: 0x040040B2 RID: 16562
			public NativeSpline conveyorSpline2;

			// Token: 0x040040B3 RID: 16563
			public NativeSpline conveyorSpline3;

			// Token: 0x040040B4 RID: 16564
			[ReadOnly]
			public NativeArray<Quaternion> conveyorRotations;

			// Token: 0x040040B5 RID: 16565
			[ReadOnly]
			public NativeList<int> conveyorIndices;

			// Token: 0x040040B6 RID: 16566
			[ReadOnly]
			public NativeList<float> splineTimes;

			// Token: 0x040040B7 RID: 16567
			[ReadOnly]
			public NativeList<Vector3> shelfOffsets;
		}
	}
}
