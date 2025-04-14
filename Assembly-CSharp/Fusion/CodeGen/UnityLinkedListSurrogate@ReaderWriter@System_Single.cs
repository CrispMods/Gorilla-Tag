using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D3A RID: 3386
	[WeaverGenerated]
	[Serializable]
	internal class UnityLinkedListSurrogate@ReaderWriter@System_Single : UnityLinkedListSurrogate<float, ReaderWriter@System_Single>
	{
		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06005437 RID: 21559 RVA: 0x0019C94C File Offset: 0x0019AB4C
		// (set) Token: 0x06005438 RID: 21560 RVA: 0x0019C954 File Offset: 0x0019AB54
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

		// Token: 0x06005439 RID: 21561 RVA: 0x0019C95D File Offset: 0x0019AB5D
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@System_Single()
		{
		}

		// Token: 0x04005958 RID: 22872
		[WeaverGenerated]
		public float[] Data = Array.Empty<float>();
	}
}
