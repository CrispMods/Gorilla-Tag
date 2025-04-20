using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion.CodeGen
{
	// Token: 0x02000D33 RID: 3379
	[WeaverGenerated]
	[NetworkStructWeaved(1)]
	[PreserveInPlugin]
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	internal struct FixedStorage@1 : INetworkStruct
	{
		// Token: 0x04005706 RID: 22278
		[FixedBuffer(typeof(int), 1)]
		[WeaverGenerated]
		[FieldOffset(0)]
		public FixedStorage@1.<Data>e__FixedBuffer Data;

		// Token: 0x02000D34 RID: 3380
		[CompilerGenerated]
		[UnsafeValueType]
		[PreserveInPlugin]
		[WeaverGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 4)]
		public struct <Data>e__FixedBuffer
		{
			// Token: 0x04005707 RID: 22279
			[WeaverGenerated]
			public int FixedElementField;
		}
	}
}
