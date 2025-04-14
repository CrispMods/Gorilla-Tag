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
		// (get) Token: 0x06005443 RID: 21571 RVA: 0x0019CF14 File Offset: 0x0019B114
		// (set) Token: 0x06005444 RID: 21572 RVA: 0x0019CF1C File Offset: 0x0019B11C
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

		// Token: 0x06005445 RID: 21573 RVA: 0x0019CF25 File Offset: 0x0019B125
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@System_Single()
		{
		}

		// Token: 0x0400596A RID: 22890
		[WeaverGenerated]
		public float[] Data = Array.Empty<float>();
	}
}
