﻿using System;
using Fusion.Internal;
using UnityEngine;

namespace Fusion.CodeGen
{
	// Token: 0x02000D12 RID: 3346
	[WeaverGenerated]
	[Serializable]
	internal class UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3 : UnityValueSurrogate<Vector3, ReaderWriter@UnityEngine_Vector3>
	{
		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x060053E0 RID: 21472 RVA: 0x00065A19 File Offset: 0x00063C19
		// (set) Token: 0x060053E1 RID: 21473 RVA: 0x00065A21 File Offset: 0x00063C21
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

		// Token: 0x060053E2 RID: 21474 RVA: 0x00065A2A File Offset: 0x00063C2A
		[WeaverGenerated]
		public UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3()
		{
		}

		// Token: 0x0400563B RID: 22075
		[WeaverGenerated]
		public Vector3 Data;
	}
}
