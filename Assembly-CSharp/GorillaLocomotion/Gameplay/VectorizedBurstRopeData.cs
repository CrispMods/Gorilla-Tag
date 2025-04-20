using System;
using Unity.Collections;
using Unity.Mathematics;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B8E RID: 2958
	public struct VectorizedBurstRopeData
	{
		// Token: 0x04004C85 RID: 19589
		public NativeArray<float4> posX;

		// Token: 0x04004C86 RID: 19590
		public NativeArray<float4> posY;

		// Token: 0x04004C87 RID: 19591
		public NativeArray<float4> posZ;

		// Token: 0x04004C88 RID: 19592
		public NativeArray<int4> validNodes;

		// Token: 0x04004C89 RID: 19593
		public NativeArray<float4> lastPosX;

		// Token: 0x04004C8A RID: 19594
		public NativeArray<float4> lastPosY;

		// Token: 0x04004C8B RID: 19595
		public NativeArray<float4> lastPosZ;

		// Token: 0x04004C8C RID: 19596
		public NativeArray<float3> ropeRoots;

		// Token: 0x04004C8D RID: 19597
		public NativeArray<float4> nodeMass;
	}
}
