using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D23 RID: 3363
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@System_Int32 : UnityArraySurrogate<int, ReaderWriter@System_Int32>
	{
		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x0600540D RID: 21517 RVA: 0x00065B6B File Offset: 0x00063D6B
		// (set) Token: 0x0600540E RID: 21518 RVA: 0x00065B73 File Offset: 0x00063D73
		[WeaverGenerated]
		public override int[] DataProperty
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

		// Token: 0x0600540F RID: 21519 RVA: 0x00065B7C File Offset: 0x00063D7C
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@System_Int32()
		{
		}

		// Token: 0x04005726 RID: 22310
		[WeaverGenerated]
		public int[] Data = Array.Empty<int>();
	}
}
