using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BC7 RID: 3015
	[CreateAssetMenu(fileName = "WatchableIntSO", menuName = "ScriptableObjects/WatchableIntSO")]
	public class WatchableIntSO : WatchableGenericSO<int>
	{
		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06004C32 RID: 19506 RVA: 0x00062276 File Offset: 0x00060476
		private int currentValue
		{
			get
			{
				return base.Value;
			}
		}
	}
}
