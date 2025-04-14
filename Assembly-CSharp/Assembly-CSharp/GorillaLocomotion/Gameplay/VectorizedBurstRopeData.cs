using System;
using Unity.Collections;
using Unity.Mathematics;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B64 RID: 2916
	public struct VectorizedBurstRopeData
	{
		// Token: 0x04004BA1 RID: 19361
		public NativeArray<float4> posX;

		// Token: 0x04004BA2 RID: 19362
		public NativeArray<float4> posY;

		// Token: 0x04004BA3 RID: 19363
		public NativeArray<float4> posZ;

		// Token: 0x04004BA4 RID: 19364
		public NativeArray<int4> validNodes;

		// Token: 0x04004BA5 RID: 19365
		public NativeArray<float4> lastPosX;

		// Token: 0x04004BA6 RID: 19366
		public NativeArray<float4> lastPosY;

		// Token: 0x04004BA7 RID: 19367
		public NativeArray<float4> lastPosZ;

		// Token: 0x04004BA8 RID: 19368
		public NativeArray<float3> ropeRoots;

		// Token: 0x04004BA9 RID: 19369
		public NativeArray<float4> nodeMass;
	}
}
