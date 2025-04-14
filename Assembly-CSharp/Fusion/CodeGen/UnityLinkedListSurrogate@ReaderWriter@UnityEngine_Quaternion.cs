using System;
using Fusion.Internal;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D28 RID: 3368
	[WeaverGenerated]
	[Serializable]
	internal class UnityLinkedListSurrogate@ReaderWriter@UnityEngine_Quaternion : UnityLinkedListSurrogate<Quaternion, ReaderWriter@UnityEngine_Quaternion>
	{
		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06005410 RID: 21520 RVA: 0x0019C714 File Offset: 0x0019A914
		// (set) Token: 0x06005411 RID: 21521 RVA: 0x0019C71C File Offset: 0x0019A91C
		[WeaverGenerated]
		public override Quaternion[] DataProperty
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

		// Token: 0x06005412 RID: 21522 RVA: 0x0019C725 File Offset: 0x0019A925
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@UnityEngine_Quaternion()
		{
		}

		// Token: 0x0400586A RID: 22634
		[WeaverGenerated]
		public Quaternion[] Data = Array.Empty<Quaternion>();
	}
}
