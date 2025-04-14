using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D39 RID: 3385
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@System_Boolean : UnityArraySurrogate<bool, ReaderWriter@System_Boolean>
	{
		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x0600543D RID: 21565 RVA: 0x0019CECC File Offset: 0x0019B0CC
		// (set) Token: 0x0600543E RID: 21566 RVA: 0x0019CED4 File Offset: 0x0019B0D4
		[WeaverGenerated]
		public override bool[] DataProperty
		{
			[WeaverGenerated]
			get
			{
				return this.Data;
			}
			[WeaverGenerated]
			set
			{
				this.Data = value;
			}
		}

		// Token: 0x0600543F RID: 21567 RVA: 0x0019CEDD File Offset: 0x0019B0DD
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@System_Boolean()
		{
		}

		// Token: 0x04005955 RID: 22869
		[WeaverGenerated]
		public bool[] Data = Array.Empty<bool>();
	}
}
