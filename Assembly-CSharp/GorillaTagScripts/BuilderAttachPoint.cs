using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009AD RID: 2477
	public class BuilderAttachPoint : MonoBehaviour
	{
		// Token: 0x06003CC4 RID: 15556 RVA: 0x00057B0F File Offset: 0x00055D0F
		private void Awake()
		{
			if (this.center == null)
			{
				this.center = base.transform;
			}
		}

		// Token: 0x04003DB0 RID: 15792
		public Transform center;
	}
}
