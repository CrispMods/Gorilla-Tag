using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CE2 RID: 3298
	[CreateAssetMenu(fileName = "BoingParams", menuName = "Boing Kit/Shared Boing Params", order = 550)]
	public class SharedBoingParams : ScriptableObject
	{
		// Token: 0x0600531F RID: 21279 RVA: 0x00199DE0 File Offset: 0x00197FE0
		public SharedBoingParams()
		{
			this.Params.Init();
		}

		// Token: 0x040055A3 RID: 21923
		public BoingWork.Params Params;
	}
}
