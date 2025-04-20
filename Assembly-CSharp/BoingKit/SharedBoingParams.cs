using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000D13 RID: 3347
	[CreateAssetMenu(fileName = "BoingParams", menuName = "Boing Kit/Shared Boing Params", order = 550)]
	public class SharedBoingParams : ScriptableObject
	{
		// Token: 0x06005481 RID: 21633 RVA: 0x00066DCC File Offset: 0x00064FCC
		public SharedBoingParams()
		{
			this.Params.Init();
		}

		// Token: 0x040056AF RID: 22191
		public BoingWork.Params Params;
	}
}
