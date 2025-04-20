using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D43 RID: 3395
	[WeaverGenerated]
	[NetworkStructWeaved(10)]
	[PreserveInPlugin]
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	internal struct FixedStorage@10 : INetworkStruct
	{
		// Token: 0x04005738 RID: 22328
		[FixedBuffer(typeof(int), 10)]
		[WeaverGenerated]
		[FieldOffset(0)]
		public FixedStorage@10.<Data>e__FixedBuffer Data;

		// Token: 0x04005739 RID: 22329
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(4)]
		private int _1;

		// Token: 0x0400573A RID: 22330
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(8)]
		private int _2;

		// Token: 0x0400573B RID: 22331
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(12)]
		private int _3;

		// Token: 0x0400573C RID: 22332
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(16)]
		private int _4;

		// Token: 0x0400573D RID: 22333
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(20)]
		private int _5;

		// Token: 0x0400573E RID: 22334
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(24)]
		private int _6;

		// Token: 0x0400573F RID: 22335
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(28)]
		private int _7;

		// Token: 0x04005740 RID: 22336
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(32)]
		private int _8;

		// Token: 0x04005741 RID: 22337
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(36)]
		private int _9;

		// Token: 0x02000D44 RID: 3396
		[CompilerGenerated]
		[UnsafeValueType]
		[PreserveInPlugin]
		[WeaverGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 40)]
		public struct <Data>e__FixedBuffer
		{
			// Token: 0x04005742 RID: 22338
			[WeaverGenerated]
			public int FixedElementField;
		}
	}
}
