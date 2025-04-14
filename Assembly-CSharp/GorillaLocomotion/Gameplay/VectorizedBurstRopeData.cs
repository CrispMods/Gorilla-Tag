using System;
using Unity.Collections;
using Unity.Mathematics;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B61 RID: 2913
	public struct VectorizedBurstRopeData
	{
		// Token: 0x04004B8F RID: 19343
		public NativeArray<float4> posX;

		// Token: 0x04004B90 RID: 19344
		public NativeArray<float4> posY;

		// Token: 0x04004B91 RID: 19345
		public NativeArray<float4> posZ;

		// Token: 0x04004B92 RID: 19346
		public NativeArray<int4> validNodes;

		// Token: 0x04004B93 RID: 19347
		public NativeArray<float4> lastPosX;

		// Token: 0x04004B94 RID: 19348
		public NativeArray<float4> lastPosY;

		// Token: 0x04004B95 RID: 19349
		public NativeArray<float4> lastPosZ;

		// Token: 0x04004B96 RID: 19350
		public NativeArray<float3> ropeRoots;

		// Token: 0x04004B97 RID: 19351
		public NativeArray<float4> nodeMass;
	}
}
