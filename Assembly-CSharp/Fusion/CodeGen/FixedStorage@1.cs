using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D02 RID: 3330
	[WeaverGenerated]
	[NetworkStructWeaved(1)]
	[PreserveInPlugin]
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	internal struct FixedStorage@1 : INetworkStruct
	{
		// Token: 0x040055FA RID: 22010
		[FixedBuffer(typeof(int), 1)]
		[WeaverGenerated]
		[FieldOffset(0)]
		public FixedStorage@1.<Data>e__FixedBuffer Data;

		// Token: 0x02000D03 RID: 3331
		[CompilerGenerated]
		[UnsafeValueType]
		[PreserveInPlugin]
		[WeaverGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 4)]
		public struct <Data>e__FixedBuffer
		{
			// Token: 0x040055FB RID: 22011
			[WeaverGenerated]
			public int FixedElementField;
		}
	}
}
