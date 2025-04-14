using System;
using Fusion.Internal;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D0F RID: 3343
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3 : UnityValueSurrogate<Vector3, ReaderWriter@UnityEngine_Vector3>
	{
		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x060053D4 RID: 21460 RVA: 0x0019C3D8 File Offset: 0x0019A5D8
		// (set) Token: 0x060053D5 RID: 21461 RVA: 0x0019C3E0 File Offset: 0x0019A5E0
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

		// Token: 0x060053D6 RID: 21462 RVA: 0x0019C3E9 File Offset: 0x0019A5E9
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3()
		{
		}

		// Token: 0x04005629 RID: 22057
		[WeaverGenerated]
		public Vector3 Data;
	}
}
