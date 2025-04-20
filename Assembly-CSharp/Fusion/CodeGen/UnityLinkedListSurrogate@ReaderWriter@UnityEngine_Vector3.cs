using System;
using Fusion.Internal;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D56 RID: 3414
	[WeaverGenerated]
	[Serializable]
	internal class UnityLinkedListSurrogate@ReaderWriter@UnityEngine_Vector3 : UnityLinkedListSurrogate<Vector3, ReaderWriter@UnityEngine_Vector3>
	{
		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x0600556F RID: 21871 RVA: 0x0006764A File Offset: 0x0006584A
		// (set) Token: 0x06005570 RID: 21872 RVA: 0x00067652 File Offset: 0x00065852
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

		// Token: 0x06005571 RID: 21873 RVA: 0x0006765B File Offset: 0x0006585B
		[WeaverGenerated]
		public UnityLinkedListSurrogate@ReaderWriter@UnityEngine_Vector3()
		{
		}

		// Token: 0x040058BD RID: 22717
		[WeaverGenerated]
		public Vector3[] Data = Array.Empty<Vector3>();
	}
}
