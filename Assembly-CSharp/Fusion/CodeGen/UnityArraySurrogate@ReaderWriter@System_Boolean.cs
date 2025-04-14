using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D36 RID: 3382
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@System_Boolean : UnityArraySurrogate<bool, ReaderWriter@System_Boolean>
	{
		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06005431 RID: 21553 RVA: 0x0019C904 File Offset: 0x0019AB04
		// (set) Token: 0x06005432 RID: 21554 RVA: 0x0019C90C File Offset: 0x0019AB0C
		[WeaverGenerated]
		public override bool[] DataProperty
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

		// Token: 0x06005433 RID: 21555 RVA: 0x0019C915 File Offset: 0x0019AB15
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@System_Boolean()
		{
		}

		// Token: 0x04005943 RID: 22851
		[WeaverGenerated]
		public bool[] Data = Array.Empty<bool>();
	}
}
