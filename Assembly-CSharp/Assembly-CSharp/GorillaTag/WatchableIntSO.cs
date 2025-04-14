using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B9D RID: 2973
	[CreateAssetMenu(fileName = "WatchableIntSO", menuName = "ScriptableObjects/WatchableIntSO")]
	public class WatchableIntSO : WatchableGenericSO<int>
	{
		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06004AF3 RID: 19187 RVA: 0x0016AD46 File Offset: 0x00168F46
		private int currentValue
		{
			get
			{
				return base.Value;
			}
		}
	}
}
