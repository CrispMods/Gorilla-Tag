using System;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B38 RID: 2872
	public class GameObjectOnDisableDispatcher : MonoBehaviour
	{
		// Token: 0x14000081 RID: 129
		// (add) Token: 0x0600477D RID: 18301 RVA: 0x00154624 File Offset: 0x00152824
		// (remove) Token: 0x0600477E RID: 18302 RVA: 0x0015465C File Offset: 0x0015285C
		public event GameObjectOnDisableDispatcher.OnDisabledEvent OnDisabled;

		// Token: 0x0600477F RID: 18303 RVA: 0x00154691 File Offset: 0x00152891
		private void OnDisable()
		{
			if (this.OnDisabled != null)
			{
				this.OnDisabled(this);
			}
		}

		// Token: 0x02000B39 RID: 2873
		// (Invoke) Token: 0x06004782 RID: 18306
		public delegate void OnDisabledEvent(GameObjectOnDisableDispatcher me);
	}
}
