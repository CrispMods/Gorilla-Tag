using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D3A RID: 3386
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@System_Single : UnityArraySurrogate<float, ReaderWriter@System_Single>
	{
		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06005440 RID: 21568 RVA: 0x00065D3E File Offset: 0x00063F3E
		// (set) Token: 0x06005441 RID: 21569 RVA: 0x00065D46 File Offset: 0x00063F46
		[WeaverGenerated]
		public override float[] DataProperty
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

		// Token: 0x06005442 RID: 21570 RVA: 0x00065D4F File Offset: 0x00063F4F
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@System_Single()
		{
		}

		// Token: 0x04005956 RID: 22870
		[WeaverGenerated]
		public float[] Data = Array.Empty<float>();
	}
}
