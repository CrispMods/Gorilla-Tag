using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B7E RID: 2942
	[BurstCompile]
	public struct SolveRopeJob : IJob
	{
		// Token: 0x060049B3 RID: 18867 RVA: 0x0019A908 File Offset: 0x00198B08
		public void Execute()
		{
			this.Simulate();
			for (int i = 0; i < 20; i++)
			{
				this.ApplyConstraint();
			}
		}

		// Token: 0x060049B4 RID: 18868 RVA: 0x0019A930 File Offset: 0x00198B30
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

		// Token: 0x060049B5 RID: 18869 RVA: 0x0019A9BC File Offset: 0x00198BBC
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

		// Token: 0x04004BFE RID: 19454
		[ReadOnly]
		public float fixedDeltaTime;

		// Token: 0x04004BFF RID: 19455
		[WriteOnly]
		public NativeArray<BurstRopeNode> nodes;

		// Token: 0x04004C00 RID: 19456
		[ReadOnly]
		public Vector3 gravity;

		// Token: 0x04004C01 RID: 19457
		[ReadOnly]
		public Vector3 rootPos;

		// Token: 0x04004C02 RID: 19458
		[ReadOnly]
		public float nodeDistance;
	}
}
