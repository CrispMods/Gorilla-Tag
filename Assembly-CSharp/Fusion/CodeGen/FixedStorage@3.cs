using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D0C RID: 3340
	[WeaverGenerated]
	[NetworkStructWeaved(3)]
	[PreserveInPlugin]
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	internal struct FixedStorage@3 : INetworkStruct
	{
		// Token: 0x04005624 RID: 22052
		[FixedBuffer(typeof(int), 3)]
		[WeaverGenerated]
		[FieldOffset(0)]
		public FixedStorage@3.<Data>e__FixedBuffer Data;

		// Token: 0x04005625 RID: 22053
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(4)]
		private int _1;

		// Token: 0x04005626 RID: 22054
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(8)]
		private int _2;

		// Token: 0x02000D0D RID: 3341
		[CompilerGenerated]
		[UnsafeValueType]
		[PreserveInPlugin]
		[WeaverGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 12)]
		public struct <Data>e__FixedBuffer
		{
			// Token: 0x04005627 RID: 22055
			[WeaverGenerated]
			public int FixedElementField;
		}
	}
}
