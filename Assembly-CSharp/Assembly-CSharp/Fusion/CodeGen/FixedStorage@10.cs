using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D15 RID: 3349
	[WeaverGenerated]
	[NetworkStructWeaved(10)]
	[PreserveInPlugin]
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	internal struct FixedStorage@10 : INetworkStruct
	{
		// Token: 0x0400563E RID: 22078
		[FixedBuffer(typeof(int), 10)]
		[WeaverGenerated]
		[FieldOffset(0)]
		public FixedStorage@10.<Data>e__FixedBuffer Data;

		// Token: 0x0400563F RID: 22079
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(4)]
		private int _1;

		// Token: 0x04005640 RID: 22080
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(8)]
		private int _2;

		// Token: 0x04005641 RID: 22081
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(12)]
		private int _3;

		// Token: 0x04005642 RID: 22082
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(16)]
		private int _4;

		// Token: 0x04005643 RID: 22083
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(20)]
		private int _5;

		// Token: 0x04005644 RID: 22084
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(24)]
		private int _6;

		// Token: 0x04005645 RID: 22085
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(28)]
		private int _7;

		// Token: 0x04005646 RID: 22086
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(32)]
		private int _8;

		// Token: 0x04005647 RID: 22087
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(36)]
		private int _9;

		// Token: 0x02000D16 RID: 3350
		[CompilerGenerated]
		[UnsafeValueType]
		[PreserveInPlugin]
		[WeaverGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 40)]
		public struct <Data>e__FixedBuffer
		{
			// Token: 0x04005648 RID: 22088
			[WeaverGenerated]
			public int FixedElementField;
		}
	}
}
