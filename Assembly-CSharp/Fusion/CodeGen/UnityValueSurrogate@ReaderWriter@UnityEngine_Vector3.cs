using System;
using Fusion.Internal;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D40 RID: 3392
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3 : UnityValueSurrogate<Vector3, ReaderWriter@UnityEngine_Vector3>
	{
		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06005536 RID: 21814 RVA: 0x0006748F File Offset: 0x0006568F
		// (set) Token: 0x06005537 RID: 21815 RVA: 0x00067497 File Offset: 0x00065697
		[WeaverGenerated]
		public override Vector3 DataProperty
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

		// Token: 0x06005538 RID: 21816 RVA: 0x000674A0 File Offset: 0x000656A0
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3()
		{
		}

		// Token: 0x04005735 RID: 22325
		[WeaverGenerated]
		public Vector3 Data;
	}
}
