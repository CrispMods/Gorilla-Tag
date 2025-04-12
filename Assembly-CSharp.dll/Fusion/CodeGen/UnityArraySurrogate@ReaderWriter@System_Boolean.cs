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
		// (get) Token: 0x0600543D RID: 21565 RVA: 0x00065D1A File Offset: 0x00063F1A
		// (set) Token: 0x0600543E RID: 21566 RVA: 0x00065D22 File Offset: 0x00063F22
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

		// Token: 0x0600543F RID: 21567 RVA: 0x00065D2B File Offset: 0x00063F2B
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@System_Boolean()
		{
		}

		// Token: 0x04005955 RID: 22869
		[WeaverGenerated]
		public bool[] Data = Array.Empty<bool>();
	}
}
