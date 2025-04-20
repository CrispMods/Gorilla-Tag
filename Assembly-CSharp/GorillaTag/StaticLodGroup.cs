using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BBF RID: 3007
	[DefaultExecutionOrder(2000)]
	public class StaticLodGroup : MonoBehaviour
	{
		// Token: 0x06004C12 RID: 19474 RVA: 0x0006205D File Offset: 0x0006025D
		protected void Awake()
		{
			this.index = StaticLodManager.Register(this);
		}

		// Token: 0x06004C13 RID: 19475 RVA: 0x0006206B File Offset: 0x0006026B
		protected void OnEnable()
		{
			StaticLodManager.SetEnabled(this.index, true);
		}

		// Token: 0x06004C14 RID: 19476 RVA: 0x00062079 File Offset: 0x00060279
		protected void OnDisable()
		{
			StaticLodManager.SetEnabled(this.index, false);
		}

		// Token: 0x06004C15 RID: 19477 RVA: 0x00062087 File Offset: 0x00060287
		private void OnDestroy()
		{
			StaticLodManager.Unregister(this.index);
		}

		// Token: 0x04004D29 RID: 19753
		private int index;

		// Token: 0x04004D2A RID: 19754
		public float collisionEnableDistance = 3f;

		// Token: 0x04004D2B RID: 19755
		public float uiFadeDistanceMin = 1f;

		// Token: 0x04004D2C RID: 19756
		public float uiFadeDistanceMax = 10f;
	}
}
