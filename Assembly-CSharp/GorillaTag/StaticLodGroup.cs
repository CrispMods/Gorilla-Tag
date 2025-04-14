using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B92 RID: 2962
	[DefaultExecutionOrder(2000)]
	public class StaticLodGroup : MonoBehaviour
	{
		// Token: 0x06004AC7 RID: 19143 RVA: 0x00169D4D File Offset: 0x00167F4D
		protected void Awake()
		{
			this.index = StaticLodManager.Register(this);
		}

		// Token: 0x06004AC8 RID: 19144 RVA: 0x00169D5B File Offset: 0x00167F5B
		protected void OnEnable()
		{
			StaticLodManager.SetEnabled(this.index, true);
		}

		// Token: 0x06004AC9 RID: 19145 RVA: 0x00169D69 File Offset: 0x00167F69
		protected void OnDisable()
		{
			StaticLodManager.SetEnabled(this.index, false);
		}

		// Token: 0x06004ACA RID: 19146 RVA: 0x00169D77 File Offset: 0x00167F77
		private void OnDestroy()
		{
			StaticLodManager.Unregister(this.index);
		}

		// Token: 0x04004C33 RID: 19507
		private int index;

		// Token: 0x04004C34 RID: 19508
		public float collisionEnableDistance = 3f;

		// Token: 0x04004C35 RID: 19509
		public float uiFadeDistanceMin = 1f;

		// Token: 0x04004C36 RID: 19510
		public float uiFadeDistanceMax = 10f;
	}
}
