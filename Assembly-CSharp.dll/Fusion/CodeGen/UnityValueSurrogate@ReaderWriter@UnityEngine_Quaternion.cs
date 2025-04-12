using System;
using Fusion.Internal;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D1B RID: 3355
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@UnityEngine_Quaternion : UnityValueSurrogate<Quaternion, ReaderWriter@UnityEngine_Quaternion>
	{
		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x060053F5 RID: 21493 RVA: 0x00065AB4 File Offset: 0x00063CB4
		// (set) Token: 0x060053F6 RID: 21494 RVA: 0x00065ABC File Offset: 0x00063CBC
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

		// Token: 0x060053F7 RID: 21495 RVA: 0x00065AC5 File Offset: 0x00063CC5
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@UnityEngine_Quaternion()
		{
		}

		// Token: 0x04005650 RID: 22096
		[WeaverGenerated]
		public Quaternion Data;
	}
}
