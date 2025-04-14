using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B54 RID: 2900
	[BurstCompile]
	public struct SolveRopeJob : IJob
	{
		// Token: 0x06004874 RID: 18548 RVA: 0x0015F4F8 File Offset: 0x0015D6F8
		public void Execute()
		{
			this.Simulate();
			for (int i = 0; i < 20; i++)
			{
				this.ApplyConstraint();
			}
		}

		// Token: 0x06004875 RID: 18549 RVA: 0x0015F520 File Offset: 0x0015D720
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

		// Token: 0x06004876 RID: 18550 RVA: 0x0015F5AC File Offset: 0x0015D7AC
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

		// Token: 0x04004B1A RID: 19226
		[ReadOnly]
		public float fixedDeltaTime;

		// Token: 0x04004B1B RID: 19227
		[WriteOnly]
		public NativeArray<BurstRopeNode> nodes;

		// Token: 0x04004B1C RID: 19228
		[ReadOnly]
		public Vector3 gravity;

		// Token: 0x04004B1D RID: 19229
		[ReadOnly]
		public Vector3 rootPos;

		// Token: 0x04004B1E RID: 19230
		[ReadOnly]
		public float nodeDistance;
	}
}
