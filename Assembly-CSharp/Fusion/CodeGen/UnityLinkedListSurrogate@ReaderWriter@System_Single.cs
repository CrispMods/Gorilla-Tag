using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D6B RID: 3435
	[WeaverGenerated]
	[Serializable]
	internal class UnityLinkedListSurrogate@ReaderWriter@System_Single : UnityLinkedListSurrogate<float, ReaderWriter@System_Single>
	{
		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06005599 RID: 21913 RVA: 0x000677D8 File Offset: 0x000659D8
		// (set) Token: 0x0600559A RID: 21914 RVA: 0x000677E0 File Offset: 0x000659E0
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

		// Token: 0x0600559B RID: 21915 RVA: 0x000677E9 File Offset: 0x000659E9
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@System_Single()
		{
		}

		// Token: 0x04005A64 RID: 23140
		[WeaverGenerated]
		public float[] Data = Array.Empty<float>();
	}
}
