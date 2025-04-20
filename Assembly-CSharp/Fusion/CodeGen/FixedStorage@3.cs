using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D3D RID: 3389
	[WeaverGenerated]
	[NetworkStructWeaved(3)]
	[PreserveInPlugin]
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	internal struct FixedStorage@3 : INetworkStruct
	{
		// Token: 0x04005730 RID: 22320
		[FixedBuffer(typeof(int), 3)]
		[WeaverGenerated]
		[FieldOffset(0)]
		public FixedStorage@3.<Data>e__FixedBuffer Data;

		// Token: 0x04005731 RID: 22321
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(4)]
		private int _1;

		// Token: 0x04005732 RID: 22322
		[WeaverGenerated]
		[NonSerialized]
		[FieldOffset(8)]
		private int _2;

		// Token: 0x02000D3E RID: 3390
		[CompilerGenerated]
		[UnsafeValueType]
		[PreserveInPlugin]
		[WeaverGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 12)]
		public struct <Data>e__FixedBuffer
		{
			// Token: 0x04005733 RID: 22323
			[WeaverGenerated]
			public int FixedElementField;
		}
	}
}
