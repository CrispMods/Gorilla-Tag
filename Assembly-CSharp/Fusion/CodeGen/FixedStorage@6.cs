using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D30 RID: 3376
	[WeaverGenerated]
	[NetworkStructWeaved(6)]
	[PreserveInPlugin]
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	internal struct FixedStorage@6 : INetworkStruct
	{
		// Token: 0x04005938 RID: 22840
		[FixedBuffer(typeof(int), 6)]
		[WeaverGenerated]
		[FieldOffset(0)]
		public FixedStorage@6.<Data>e__FixedBuffer Data;

		// Token: 0x04005939 RID: 22841
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(4)]
		private int _1;

		// Token: 0x0400593A RID: 22842
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(8)]
		private int _2;

		// Token: 0x0400593B RID: 22843
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(12)]
		private int _3;

		// Token: 0x0400593C RID: 22844
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(16)]
		private int _4;

		// Token: 0x0400593D RID: 22845
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(20)]
		private int _5;

		// Token: 0x02000D31 RID: 3377
		[CompilerGenerated]
		[UnsafeValueType]
		[PreserveInPlugin]
		[WeaverGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 24)]
		public struct <Data>e__FixedBuffer
		{
			// Token: 0x0400593E RID: 22846
			[WeaverGenerated]
			public int FixedElementField;
		}
	}
}
