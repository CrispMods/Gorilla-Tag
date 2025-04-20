using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D46 RID: 3398
	[WeaverGenerated]
	[NetworkStructWeaved(4)]
	[PreserveInPlugin]
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	internal struct FixedStorage@4 : INetworkStruct
	{
		// Token: 0x04005744 RID: 22340
		[FixedBuffer(typeof(int), 4)]
		[WeaverGenerated]
		[FieldOffset(0)]
		public FixedStorage@4.<Data>e__FixedBuffer Data;

		// Token: 0x04005745 RID: 22341
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(4)]
		private int _1;

		// Token: 0x04005746 RID: 22342
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(8)]
		private int _2;

		// Token: 0x04005747 RID: 22343
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(12)]
		private int _3;

		// Token: 0x02000D47 RID: 3399
		[CompilerGenerated]
		[UnsafeValueType]
		[PreserveInPlugin]
		[WeaverGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 16)]
		public struct <Data>e__FixedBuffer
		{
			// Token: 0x04005748 RID: 22344
			[WeaverGenerated]
			public int FixedElementField;
		}
	}
}
