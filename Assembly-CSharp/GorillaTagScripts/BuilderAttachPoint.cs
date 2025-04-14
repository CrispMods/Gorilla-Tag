using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000987 RID: 2439
	public class BuilderAttachPoint : MonoBehaviour
	{
		// Token: 0x06003BAC RID: 15276 RVA: 0x00112BE1 File Offset: 0x00110DE1
		private void Awake()
		{
			if (this.center == null)
			{
				this.center = base.transform;
			}
		}

		// Token: 0x04003CD6 RID: 15574
		public Transform center;
	}
}
