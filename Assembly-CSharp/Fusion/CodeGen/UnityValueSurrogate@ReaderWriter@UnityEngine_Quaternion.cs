using System;
using Fusion.Internal;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D49 RID: 3401
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@UnityEngine_Quaternion : UnityValueSurrogate<Quaternion, ReaderWriter@UnityEngine_Quaternion>
	{
		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x0600554B RID: 21835 RVA: 0x0006752A File Offset: 0x0006572A
		// (set) Token: 0x0600554C RID: 21836 RVA: 0x00067532 File Offset: 0x00065732
		[WeaverGenerated]
		public override Quaternion DataProperty
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

		// Token: 0x0600554D RID: 21837 RVA: 0x0006753B File Offset: 0x0006573B
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@UnityEngine_Quaternion()
		{
		}

		// Token: 0x0400574A RID: 22346
		[WeaverGenerated]
		public Quaternion Data;
	}
}
