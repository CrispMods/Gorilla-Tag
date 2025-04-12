using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B95 RID: 2965
	[DefaultExecutionOrder(2000)]
	public class StaticLodGroup : MonoBehaviour
	{
		// Token: 0x06004AD3 RID: 19155 RVA: 0x00060625 File Offset: 0x0005E825
		protected void Awake()
		{
			this.index = StaticLodManager.Register(this);
		}

		// Token: 0x06004AD4 RID: 19156 RVA: 0x00060633 File Offset: 0x0005E833
		protected void OnEnable()
		{
			StaticLodManager.SetEnabled(this.index, true);
		}

		// Token: 0x06004AD5 RID: 19157 RVA: 0x00060641 File Offset: 0x0005E841
		protected void OnDisable()
		{
			StaticLodManager.SetEnabled(this.index, false);
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x0006064F File Offset: 0x0005E84F
		private void OnDestroy()
		{
			StaticLodManager.Unregister(this.index);
		}

		// Token: 0x04004C45 RID: 19525
		private int index;

		// Token: 0x04004C46 RID: 19526
		public float collisionEnableDistance = 3f;

		// Token: 0x04004C47 RID: 19527
		public float uiFadeDistanceMin = 1f;

		// Token: 0x04004C48 RID: 19528
		public float uiFadeDistanceMax = 10f;
	}
}
