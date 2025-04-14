using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D05 RID: 3333
	[WeaverGenerated]
	[NetworkStructWeaved(1)]
	[PreserveInPlugin]
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	internal struct FixedStorage@1 : INetworkStruct
	{
		// Token: 0x0400560C RID: 22028
		[FixedBuffer(typeof(int), 1)]
		[WeaverGenerated]
		[FieldOffset(0)]
		public FixedStorage@1.<Data>e__FixedBuffer Data;

		// Token: 0x02000D06 RID: 3334
		[CompilerGenerated]
		[UnsafeValueType]
		[PreserveInPlugin]
		[WeaverGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 4)]
		public struct <Data>e__FixedBuffer
		{
			// Token: 0x0400560D RID: 22029
			[WeaverGenerated]
			public int FixedElementField;
		}
	}
}
