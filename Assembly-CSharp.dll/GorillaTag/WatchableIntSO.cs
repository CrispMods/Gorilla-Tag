using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B9D RID: 2973
	[CreateAssetMenu(fileName = "WatchableIntSO", menuName = "ScriptableObjects/WatchableIntSO")]
	public class WatchableIntSO : WatchableGenericSO<int>
	{
		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06004AF3 RID: 19187 RVA: 0x0006083E File Offset: 0x0005EA3E
		private int currentValue
		{
			get
			{
				return base.Value;
			}
		}
	}
}
