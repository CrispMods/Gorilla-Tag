using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D12 RID: 3346
	[WeaverGenerated]
	[NetworkStructWeaved(10)]
	[PreserveInPlugin]
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	internal struct FixedStorage@10 : INetworkStruct
	{
		// Token: 0x0400562C RID: 22060
		[FixedBuffer(typeof(int), 10)]
		[WeaverGenerated]
		[FieldOffset(0)]
		public FixedStorage@10.<Data>e__FixedBuffer Data;

		// Token: 0x0400562D RID: 22061
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(4)]
		private int _1;

		// Token: 0x0400562E RID: 22062
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(8)]
		private int _2;

		// Token: 0x0400562F RID: 22063
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(12)]
		private int _3;

		// Token: 0x04005630 RID: 22064
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(16)]
		private int _4;

		// Token: 0x04005631 RID: 22065
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(20)]
		private int _5;

		// Token: 0x04005632 RID: 22066
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(24)]
		private int _6;

		// Token: 0x04005633 RID: 22067
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(28)]
		private int _7;

		// Token: 0x04005634 RID: 22068
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(32)]
		private int _8;

		// Token: 0x04005635 RID: 22069
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(36)]
		private int _9;

		// Token: 0x02000D13 RID: 3347
		[CompilerGenerated]
		[UnsafeValueType]
		[PreserveInPlugin]
		[WeaverGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 40)]
		public struct <Data>e__FixedBuffer
		{
			// Token: 0x04005636 RID: 22070
			[WeaverGenerated]
			public int FixedElementField;
		}
	}
}
