using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B51 RID: 2897
	[BurstCompile]
	public struct SolveRopeJob : IJob
	{
		// Token: 0x06004868 RID: 18536 RVA: 0x0015EF30 File Offset: 0x0015D130
		public void Execute()
		{
			this.Simulate();
			for (int i = 0; i < 20; i++)
			{
				this.ApplyConstraint();
			}
		}

		// Token: 0x06004869 RID: 18537 RVA: 0x0015EF58 File Offset: 0x0015D158
		private void Simulate()
		{
			for (int i = 0; i < this.nodes.Length; i++)
			{
				BurstRopeNode burstRopeNode = this.nodes[i];
				Vector3 b = burstRopeNode.curPos - burstRopeNode.lastPos;
				burstRopeNode.lastPos = burstRopeNode.curPos;
				Vector3 vector = burstRopeNode.curPos + b;
				vector += this.gravity * this.fixedDeltaTime;
				burstRopeNode.curPos = vector;
				this.nodes[i] = burstRopeNode;
			}
		}

		// Token: 0x0600486A RID: 18538 RVA: 0x0015EFE4 File Offset: 0x0015D1E4
		private void ApplyConstraint()
		{
			BurstRopeNode value = this.nodes[0];
			value.curPos = this.rootPos;
			this.nodes[0] = value;
			for (int i = 0; i < this.nodes.Length - 1; i++)
			{
				BurstRopeNode burstRopeNode = this.nodes[i];
				BurstRopeNode burstRopeNode2 = this.nodes[i + 1];
				float magnitude = (burstRopeNode.curPos - burstRopeNode2.curPos).magnitude;
				float d = Mathf.Abs(magnitude - this.nodeDistance);
				Vector3 a = Vector3.zero;
				if (magnitude > this.nodeDistance)
				{
					a = (burstRopeNode.curPos - burstRopeNode2.curPos).normalized;
				}
				else if (magnitude < this.nodeDistance)
				{
					a = (burstRopeNode2.curPos - burstRopeNode.curPos).normalized;
				}
				Vector3 a2 = a * d;
				burstRopeNode.curPos -= a2 * 0.5f;
				burstRopeNode2.curPos += a2 * 0.5f;
				this.nodes[i] = burstRopeNode;
				this.nodes[i + 1] = burstRopeNode2;
			}
		}

		// Token: 0x04004B08 RID: 19208
		[ReadOnly]
		public float fixedDeltaTime;

		// Token: 0x04004B09 RID: 19209
		[WriteOnly]
		public NativeArray<BurstRopeNode> nodes;

		// Token: 0x04004B0A RID: 19210
		[ReadOnly]
		public Vector3 gravity;

		// Token: 0x04004B0B RID: 19211
		[ReadOnly]
		public Vector3 rootPos;

		// Token: 0x04004B0C RID: 19212
		[ReadOnly]
		public float nodeDistance;
	}
}
