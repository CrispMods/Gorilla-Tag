using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D15 RID: 3349
	[WeaverGenerated]
	[NetworkStructWeaved(4)]
	[PreserveInPlugin]
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	internal struct FixedStorage@4 : INetworkStruct
	{
		// Token: 0x04005638 RID: 22072
		[FixedBuffer(typeof(int), 4)]
		[WeaverGenerated]
		[FieldOffset(0)]
		public FixedStorage@4.<Data>e__FixedBuffer Data;

		// Token: 0x04005639 RID: 22073
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(4)]
		private int _1;

		// Token: 0x0400563A RID: 22074
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(8)]
		private int _2;

		// Token: 0x0400563B RID: 22075
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(12)]
		private int _3;

		// Token: 0x02000D16 RID: 3350
		[CompilerGenerated]
		[UnsafeValueType]
		[PreserveInPlugin]
		[WeaverGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 16)]
		public struct <Data>e__FixedBuffer
		{
			// Token: 0x0400563C RID: 22076
			[WeaverGenerated]
			public int FixedElementField;
		}
	}
}
