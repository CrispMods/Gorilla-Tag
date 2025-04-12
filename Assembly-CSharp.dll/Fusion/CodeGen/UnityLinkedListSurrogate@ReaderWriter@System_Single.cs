using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D3D RID: 3389
	[WeaverGenerated]
	[Serializable]
	internal class UnityLinkedListSurrogate@ReaderWriter@System_Single : UnityLinkedListSurrogate<float, ReaderWriter@System_Single>
	{
		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06005443 RID: 21571 RVA: 0x00065D62 File Offset: 0x00063F62
		// (set) Token: 0x06005444 RID: 21572 RVA: 0x00065D6A File Offset: 0x00063F6A
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

		// Token: 0x06005445 RID: 21573 RVA: 0x00065D73 File Offset: 0x00063F73
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@System_Single()
		{
		}

		// Token: 0x0400596A RID: 22890
		[WeaverGenerated]
		public float[] Data = Array.Empty<float>();
	}
}
