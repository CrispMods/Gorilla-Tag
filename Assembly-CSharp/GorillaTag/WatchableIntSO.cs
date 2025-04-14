using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B9A RID: 2970
	[CreateAssetMenu(fileName = "WatchableIntSO", menuName = "ScriptableObjects/WatchableIntSO")]
	public class WatchableIntSO : WatchableGenericSO<int>
	{
		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06004AE7 RID: 19175 RVA: 0x0016A77E File Offset: 0x0016897E
		private int currentValue
		{
			get
			{
				return base.Value;
			}
		}
	}
}
