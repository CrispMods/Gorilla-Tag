using System;
using Fusion.Internal;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D59 RID: 3417
	[WeaverGenerated]
	[Serializable]
	internal class UnityLinkedListSurrogate@ReaderWriter@UnityEngine_Quaternion : UnityLinkedListSurrogate<Quaternion, ReaderWriter@UnityEngine_Quaternion>
	{
		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06005572 RID: 21874 RVA: 0x0006766E File Offset: 0x0006586E
		// (set) Token: 0x06005573 RID: 21875 RVA: 0x00067676 File Offset: 0x00065876
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

		// Token: 0x06005574 RID: 21876 RVA: 0x0006767F File Offset: 0x0006587F
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@UnityEngine_Quaternion()
		{
		}

		// Token: 0x04005976 RID: 22902
		[WeaverGenerated]
		public Quaternion[] Data = Array.Empty<Quaternion>();
	}
}
