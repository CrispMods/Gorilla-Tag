using System;
using Fusion.Internal;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D25 RID: 3365
	[WeaverGenerated]
	[Serializable]
	internal class UnityLinkedListSurrogate@ReaderWriter@UnityEngine_Vector3 : UnityLinkedListSurrogate<Vector3, ReaderWriter@UnityEngine_Vector3>
	{
		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x0600540D RID: 21517 RVA: 0x0019C6F0 File Offset: 0x0019A8F0
		// (set) Token: 0x0600540E RID: 21518 RVA: 0x0019C6F8 File Offset: 0x0019A8F8
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

		// Token: 0x0600540F RID: 21519 RVA: 0x0019C701 File Offset: 0x0019A901
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@UnityEngine_Vector3()
		{
		}

		// Token: 0x040057B1 RID: 22449
		[WeaverGenerated]
		public Vector3[] Data = Array.Empty<Vector3>();
	}
}
