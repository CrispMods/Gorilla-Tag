using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CE5 RID: 3301
	[CreateAssetMenu(fileName = "BoingParams", menuName = "Boing Kit/Shared Boing Params", order = 550)]
	public class SharedBoingParams : ScriptableObject
	{
		// Token: 0x0600532B RID: 21291 RVA: 0x00065356 File Offset: 0x00063556
		public SharedBoingParams()
		{
			this.Params.Init();
		}

		// Token: 0x040055B5 RID: 21941
		public BoingWork.Params Params;
	}
}
