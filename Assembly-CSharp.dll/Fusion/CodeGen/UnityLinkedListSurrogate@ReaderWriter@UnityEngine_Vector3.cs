using System;
using Fusion.Internal;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D28 RID: 3368
	[WeaverGenerated]
	[Serializable]
	internal class UnityLinkedListSurrogate@ReaderWriter@UnityEngine_Vector3 : UnityLinkedListSurrogate<Vector3, ReaderWriter@UnityEngine_Vector3>
	{
		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06005419 RID: 21529 RVA: 0x00065BD4 File Offset: 0x00063DD4
		// (set) Token: 0x0600541A RID: 21530 RVA: 0x00065BDC File Offset: 0x00063DDC
		[WeaverGenerated]
		public override Vector3[] DataProperty
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

		// Token: 0x0600541B RID: 21531 RVA: 0x00065BE5 File Offset: 0x00063DE5
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@UnityEngine_Vector3()
		{
		}

		// Token: 0x040057C3 RID: 22467
		[WeaverGenerated]
		public Vector3[] Data = Array.Empty<Vector3>();
	}
}
