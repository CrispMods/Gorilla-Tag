using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D68 RID: 3432
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@System_Single : UnityArraySurrogate<float, ReaderWriter@System_Single>
	{
		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06005596 RID: 21910 RVA: 0x000677B4 File Offset: 0x000659B4
		// (set) Token: 0x06005597 RID: 21911 RVA: 0x000677BC File Offset: 0x000659BC
		[WeaverGenerated]
		public override float[] DataProperty
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

		// Token: 0x06005598 RID: 21912 RVA: 0x000677C5 File Offset: 0x000659C5
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@System_Single()
		{
		}

		// Token: 0x04005A50 RID: 23120
		[WeaverGenerated]
		public float[] Data = Array.Empty<float>();
	}
}
