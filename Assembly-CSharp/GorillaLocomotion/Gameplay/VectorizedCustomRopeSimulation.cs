using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B60 RID: 2912
	public class VectorizedCustomRopeSimulation : MonoBehaviour
	{
		// Token: 0x060048E3 RID: 18659 RVA: 0x00161501 File Offset: 0x0015F701
		private void Awake()
		{
			VectorizedCustomRopeSimulation.instance = this;
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x00161509 File Offset: 0x0015F709
		public static void Register(GorillaRopeSwing rope)
		{
			VectorizedCustomRopeSimulation.registerQueue.Add(rope);
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x00161516 File Offset: 0x0015F716
		public static void Unregister(GorillaRopeSwing rope)
		{
			VectorizedCustomRopeSimulation.deregisterQueue.Add(rope);
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x00161524 File Offset: 0x0015F724
		private void RegenerateData()
		{
			this.Dispose();
			foreach (GorillaRopeSwing item in VectorizedCustomRopeSimulation.registerQueue)
			{
				this.ropes.Add(item);
			}
			foreach (GorillaRopeSwing item2 in VectorizedCustomRopeSimulation.deregisterQueue)
			{
				this.ropes.Remove(item2);
			}
			VectorizedCustomRopeSimulation.registerQueue.Clear();
			VectorizedCustomRopeSimulation.deregisterQueue.Clear();
			int num = this.ropes.Count;
			while (num % 4 != 0)
			{
				num++;
			}
			int num2 = num * 32 / 4;
			this.burstData = new VectorizedBurstRopeData
			{
				posX = new NativeArray<float4>(num2, Allocator.Persistent, NativeArrayOptions.ClearMemory),
				posY = new NativeArray<float4>(num2, Allocator.Persistent, NativeArrayOptions.ClearMemory),
				posZ = new NativeArray<float4>(num2, Allocator.Persistent, NativeArrayOptions.ClearMemory),
				validNodes = new NativeArray<int4>(num2, Allocator.Persistent, NativeArrayOptions.ClearMemory),
				lastPosX = new NativeArray<float4>(num2, Allocator.Persistent, NativeArrayOptions.ClearMemory),
				lastPosY = new NativeArray<float4>(num2, Allocator.Persistent, NativeArrayOptions.ClearMemory),
				lastPosZ = new NativeArray<float4>(num2, Allocator.Persistent, NativeArrayOptions.ClearMemory),
				ropeRoots = new NativeArray<float3>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory),
				nodeMass = new NativeArray<float4>(num2, Allocator.Persistent, NativeArrayOptions.ClearMemory)
			};
			for (int i = 0; i < this.ropes.Count; i += 4)
			{
				int num3 = 0;
				while (num3 < 4 && this.ropes.Count > i + num3)
				{
					this.ropes[i + num3].ropeDataStartIndex = 32 * i / 4;
					this.ropes[i + num3].ropeDataIndexOffset = num3;
					num3++;
				}
			}
			int num4 = 0;
			for (int j = 0; j < num2; j++)
			{
				float4 value = this.burstData.posX[j];
				float4 value2 = this.burstData.posY[j];
				float4 value3 = this.burstData.posZ[j];
				int4 value4 = this.burstData.validNodes[j];
				for (int k = 0; k < 4; k++)
				{
					int num5 = num4 * 4 + k;
					int num6 = j - num4 * 32;
					if (this.ropes.Count > num5 && this.ropes[num5].nodes.Length > num6)
					{
						Vector3 localPosition = this.ropes[num5].nodes[num6].localPosition;
						value[k] = localPosition.x;
						value2[k] = localPosition.y;
						value3[k] = localPosition.z;
						value4[k] = 1;
					}
					else
					{
						value[k] = 0f;
						value2[k] = 0f;
						value3[k] = 0f;
						value4[k] = 0;
					}
				}
				if (j > 0 && (j + 1) % 32 == 0)
				{
					num4++;
				}
				this.burstData.posX[j] = value;
				this.burstData.posY[j] = value2;
				this.burstData.posZ[j] = value3;
				this.burstData.lastPosX[j] = value;
				this.burstData.lastPosY[j] = value2;
				this.burstData.lastPosZ[j] = value3;
				this.burstData.validNodes[j] = value4;
				this.burstData.nodeMass[j] = math.float4(1f, 1f, 1f, 1f);
			}
			for (int l = 0; l < this.ropes.Count; l++)
			{
				this.burstData.ropeRoots[l] = float3.zero;
			}
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x00161940 File Offset: 0x0015FB40
		private void Dispose()
		{
			if (!this.burstData.posX.IsCreated)
			{
				return;
			}
			this.burstData.posX.Dispose();
			this.burstData.posY.Dispose();
			this.burstData.posZ.Dispose();
			this.burstData.validNodes.Dispose();
			this.burstData.lastPosX.Dispose();
			this.burstData.lastPosY.Dispose();
			this.burstData.lastPosZ.Dispose();
			this.burstData.ropeRoots.Dispose();
			this.burstData.nodeMass.Dispose();
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x001619F0 File Offset: 0x0015FBF0
		private void OnDestroy()
		{
			this.Dispose();
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x001619F8 File Offset: 0x0015FBF8
		public void SetRopePos(GorillaRopeSwing ropeTarget, Vector3[] positions, bool setCurPos, bool setLastPos, int onlySetIndex = -1)
		{
			if (!this.ropes.Contains(ropeTarget))
			{
				return;
			}
			int ropeDataIndexOffset = ropeTarget.ropeDataIndexOffset;
			for (int i = 0; i < positions.Length; i++)
			{
				if (onlySetIndex < 0 || i == onlySetIndex)
				{
					int index = ropeTarget.ropeDataStartIndex + i;
					if (setCurPos)
					{
						float4 value = this.burstData.posX[index];
						float4 value2 = this.burstData.posY[index];
						float4 value3 = this.burstData.posZ[index];
						value[ropeDataIndexOffset] = positions[i].x;
						value2[ropeDataIndexOffset] = positions[i].y;
						value3[ropeDataIndexOffset] = positions[i].z;
						this.burstData.posX[index] = value;
						this.burstData.posY[index] = value2;
						this.burstData.posZ[index] = value3;
					}
					if (setLastPos)
					{
						float4 value4 = this.burstData.lastPosX[index];
						float4 value5 = this.burstData.lastPosY[index];
						float4 value6 = this.burstData.lastPosZ[index];
						value4[ropeDataIndexOffset] = positions[i].x;
						value5[ropeDataIndexOffset] = positions[i].y;
						value6[ropeDataIndexOffset] = positions[i].z;
						this.burstData.lastPosX[index] = value4;
						this.burstData.lastPosY[index] = value5;
						this.burstData.lastPosZ[index] = value6;
					}
				}
			}
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x00161BAC File Offset: 0x0015FDAC
		public void SetVelocity(GorillaRopeSwing ropeTarget, Vector3 velocity, bool wholeRope, int boneIndex = 1)
		{
			List<Vector3> list = new List<Vector3>();
			float maxLength = math.min(velocity.magnitude, 15f);
			int ropeDataStartIndex = ropeTarget.ropeDataStartIndex;
			int ropeDataIndexOffset = ropeTarget.ropeDataIndexOffset;
			if (ropeTarget.SupportsMovingAtRuntime)
			{
				velocity = Quaternion.Inverse(ropeTarget.transform.rotation) * velocity;
			}
			for (int i = 0; i < ropeTarget.nodes.Length; i++)
			{
				Vector3 vector = new Vector3(this.burstData.lastPosX[ropeDataStartIndex + i][ropeDataIndexOffset], this.burstData.lastPosY[ropeDataStartIndex + i][ropeDataIndexOffset], this.burstData.lastPosZ[ropeDataStartIndex + i][ropeDataIndexOffset]);
				if ((wholeRope || boneIndex == i) && boneIndex > 0)
				{
					Vector3 vector2 = velocity / (float)boneIndex * (float)i;
					vector2 = Vector3.ClampMagnitude(vector2, maxLength);
					list.Add(vector += vector2 * this.lastDelta);
				}
				else
				{
					list.Add(vector);
				}
			}
			int onlySetIndex = -1;
			if (!wholeRope && boneIndex > 0)
			{
				onlySetIndex = boneIndex;
			}
			this.SetRopePos(ropeTarget, list.ToArray(), true, false, onlySetIndex);
		}

		// Token: 0x060048EB RID: 18667 RVA: 0x00161CF0 File Offset: 0x0015FEF0
		public Vector3 GetNodeVelocity(GorillaRopeSwing ropeTarget, int nodeIndex)
		{
			int index = ropeTarget.ropeDataStartIndex + nodeIndex;
			int ropeDataIndexOffset = ropeTarget.ropeDataIndexOffset;
			Vector3 a = new Vector3(this.burstData.posX[index][ropeDataIndexOffset], this.burstData.posY[index][ropeDataIndexOffset], this.burstData.posZ[index][ropeDataIndexOffset]);
			Vector3 b = new Vector3(this.burstData.lastPosX[index][ropeDataIndexOffset], this.burstData.lastPosY[index][ropeDataIndexOffset], this.burstData.lastPosZ[index][ropeDataIndexOffset]);
			Vector3 vector = (a - b) / this.lastDelta;
			if (ropeTarget.SupportsMovingAtRuntime)
			{
				vector = ropeTarget.transform.rotation * vector;
			}
			return vector;
		}

		// Token: 0x060048EC RID: 18668 RVA: 0x00161DE8 File Offset: 0x0015FFE8
		public void SetMassForPlayers(GorillaRopeSwing ropeTarget, bool hasPlayers, int furthestBoneIndex = 0)
		{
			if (!this.ropes.Contains(ropeTarget))
			{
				return;
			}
			int ropeDataIndexOffset = ropeTarget.ropeDataIndexOffset;
			for (int i = 0; i < 32; i++)
			{
				int index = ropeTarget.ropeDataStartIndex + i;
				float4 value = this.burstData.nodeMass[index];
				if (hasPlayers && i == furthestBoneIndex + 1)
				{
					value[ropeDataIndexOffset] = 0.1f;
				}
				else
				{
					value[ropeDataIndexOffset] = 1f;
				}
				this.burstData.nodeMass[index] = value;
			}
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x00161E6C File Offset: 0x0016006C
		private void Update()
		{
			if (VectorizedCustomRopeSimulation.registerQueue.Count > 0 || VectorizedCustomRopeSimulation.deregisterQueue.Count > 0)
			{
				this.RegenerateData();
			}
			if (this.ropes.Count <= 0)
			{
				return;
			}
			float deltaTime = math.min(Time.deltaTime, 0.05f);
			VectorizedSolveRopeJob jobData = new VectorizedSolveRopeJob
			{
				applyConstraintIterations = this.applyConstraintIterations,
				finalPassIterations = this.finalPassIterations,
				lastDeltaTime = this.lastDelta,
				deltaTime = deltaTime,
				gravity = this.gravity,
				data = this.burstData,
				nodeDistance = this.nodeDistance,
				ropeCount = this.ropes.Count
			};
			jobData.Schedule(default(JobHandle)).Complete();
			for (int i = 0; i < this.ropes.Count; i++)
			{
				GorillaRopeSwing gorillaRopeSwing = this.ropes[i];
				if (!gorillaRopeSwing.isIdle || !gorillaRopeSwing.isFullyIdle)
				{
					int ropeDataIndexOffset = gorillaRopeSwing.ropeDataIndexOffset;
					for (int j = 0; j < gorillaRopeSwing.nodes.Length; j++)
					{
						int index = gorillaRopeSwing.ropeDataStartIndex + j;
						gorillaRopeSwing.nodes[j].localPosition = new Vector3(jobData.data.posX[index][ropeDataIndexOffset], jobData.data.posY[index][ropeDataIndexOffset], jobData.data.posZ[index][ropeDataIndexOffset]);
					}
					if (gorillaRopeSwing.SupportsMovingAtRuntime)
					{
						for (int k = 0; k < gorillaRopeSwing.nodes.Length - 1; k++)
						{
							Transform transform = gorillaRopeSwing.nodes[k];
							int ropeDataStartIndex = gorillaRopeSwing.ropeDataStartIndex;
							transform.up = gorillaRopeSwing.transform.rotation * -(gorillaRopeSwing.nodes[k + 1].localPosition - transform.localPosition);
						}
					}
					else
					{
						for (int l = 0; l < gorillaRopeSwing.nodes.Length - 1; l++)
						{
							Transform transform2 = gorillaRopeSwing.nodes[l];
							int ropeDataStartIndex2 = gorillaRopeSwing.ropeDataStartIndex;
							transform2.up = -(gorillaRopeSwing.nodes[l + 1].localPosition - transform2.localPosition);
						}
					}
				}
			}
			this.lastDelta = deltaTime;
		}

		// Token: 0x04004B82 RID: 19330
		public static VectorizedCustomRopeSimulation instance;

		// Token: 0x04004B83 RID: 19331
		public const int MAX_NODE_COUNT = 32;

		// Token: 0x04004B84 RID: 19332
		public const float MAX_ROPE_SPEED = 15f;

		// Token: 0x04004B85 RID: 19333
		private List<Transform> nodes = new List<Transform>();

		// Token: 0x04004B86 RID: 19334
		[SerializeField]
		private float nodeDistance = 1f;

		// Token: 0x04004B87 RID: 19335
		[SerializeField]
		private int applyConstraintIterations = 20;

		// Token: 0x04004B88 RID: 19336
		[SerializeField]
		private int finalPassIterations = 1;

		// Token: 0x04004B89 RID: 19337
		[SerializeField]
		private float gravity = -0.15f;

		// Token: 0x04004B8A RID: 19338
		private VectorizedBurstRopeData burstData;

		// Token: 0x04004B8B RID: 19339
		private float lastDelta = 0.02f;

		// Token: 0x04004B8C RID: 19340
		private List<GorillaRopeSwing> ropes = new List<GorillaRopeSwing>();

		// Token: 0x04004B8D RID: 19341
		private static List<GorillaRopeSwing> registerQueue = new List<GorillaRopeSwing>();

		// Token: 0x04004B8E RID: 19342
		private static List<GorillaRopeSwing> deregisterQueue = new List<GorillaRopeSwing>();
	}
}
