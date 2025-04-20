using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D61 RID: 3425
	[WeaverGenerated]
	[NetworkStructWeaved(6)]
	[PreserveInPlugin]
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	internal struct FixedStorage@6 : INetworkStruct
	{
		// Token: 0x04005A44 RID: 23108
		[FixedBuffer(typeof(int), 6)]
		[WeaverGenerated]
		[FieldOffset(0)]
		public FixedStorage@6.<Data>e__FixedBuffer Data;

		// Token: 0x04005A45 RID: 23109
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(4)]
		private int _1;

		// Token: 0x04005A46 RID: 23110
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(8)]
		private int _2;

		// Token: 0x04005A47 RID: 23111
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(12)]
		private int _3;

		// Token: 0x04005A48 RID: 23112
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(16)]
		private int _4;

		// Token: 0x04005A49 RID: 23113
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(20)]
		private int _5;

		// Token: 0x02000D62 RID: 3426
		[CompilerGenerated]
		[UnsafeValueType]
		[PreserveInPlugin]
		[WeaverGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 24)]
		public struct <Data>e__FixedBuffer
		{
			// Token: 0x04005A4A RID: 23114
			[WeaverGenerated]
			public int FixedElementField;
		}
	}
}
