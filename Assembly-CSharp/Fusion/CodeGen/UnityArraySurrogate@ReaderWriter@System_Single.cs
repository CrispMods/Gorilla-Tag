using System;
using Fusion.Internal;

namespace Fusion.CodeGen
{
	// Token: 0x02000D37 RID: 3383
	[WeaverGenerated]
	[Serializable]
	internal class UnityArraySurrogate@ReaderWriter@System_Single : UnityArraySurrogate<float, ReaderWriter@System_Single>
	{
		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06005434 RID: 21556 RVA: 0x0019C928 File Offset: 0x0019AB28
		// (set) Token: 0x06005435 RID: 21557 RVA: 0x0019C930 File Offset: 0x0019AB30
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

		// Token: 0x06005436 RID: 21558 RVA: 0x0019C939 File Offset: 0x0019AB39
		[WeaverGenerated]
		public UnityArraySurrogate@ReaderWriter@System_Single()
		{
		}

		// Token: 0x04005944 RID: 22852
		[WeaverGenerated]
		public float[] Data = Array.Empty<float>();
	}
}
