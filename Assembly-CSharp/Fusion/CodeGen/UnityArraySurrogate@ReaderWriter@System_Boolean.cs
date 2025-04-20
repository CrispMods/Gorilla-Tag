using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D67 RID: 3431
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@System_Boolean : UnityArraySurrogate<bool, ReaderWriter@System_Boolean>
	{
		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06005593 RID: 21907 RVA: 0x00067790 File Offset: 0x00065990
		// (set) Token: 0x06005594 RID: 21908 RVA: 0x00067798 File Offset: 0x00065998
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

		// Token: 0x06005595 RID: 21909 RVA: 0x000677A1 File Offset: 0x000659A1
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@System_Boolean()
		{
		}

		// Token: 0x04005A4F RID: 23119
		[WeaverGenerated]
		public bool[] Data = Array.Empty<bool>();
	}
}
