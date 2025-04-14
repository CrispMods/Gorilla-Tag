using System;
using Fusion.Internal;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D18 RID: 3352
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@UnityEngine_Quaternion : UnityValueSurrogate<Quaternion, ReaderWriter@UnityEngine_Quaternion>
	{
		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x060053E9 RID: 21481 RVA: 0x0019C500 File Offset: 0x0019A700
		// (set) Token: 0x060053EA RID: 21482 RVA: 0x0019C508 File Offset: 0x0019A708
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

		// Token: 0x060053EB RID: 21483 RVA: 0x0019C511 File Offset: 0x0019A711
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@UnityEngine_Quaternion()
		{
		}

		// Token: 0x0400563E RID: 22078
		[WeaverGenerated]
		public Quaternion Data;
	}
}
