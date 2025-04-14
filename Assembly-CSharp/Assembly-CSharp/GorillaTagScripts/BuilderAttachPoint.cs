using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x0200098A RID: 2442
	public class BuilderAttachPoint : MonoBehaviour
	{
		// Token: 0x06003BB8 RID: 15288 RVA: 0x001131A9 File Offset: 0x001113A9
		private void Awake()
		{
			if (this.center == null)
			{
				this.center = base.transform;
			}
		}

		// Token: 0x04003CE8 RID: 15592
		public Transform center;
	}
}
