using System;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B35 RID: 2869
	public class GameObjectOnDisableDispatcher : MonoBehaviour
	{
		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06004771 RID: 18289 RVA: 0x0015405C File Offset: 0x0015225C
		// (remove) Token: 0x06004772 RID: 18290 RVA: 0x00154094 File Offset: 0x00152294
		public event GameObjectOnDisableDispatcher.OnDisabledEvent OnDisabled;

		// Token: 0x06004773 RID: 18291 RVA: 0x001540C9 File Offset: 0x001522C9
		private void OnDisable()
		{
			if (this.OnDisabled != null)
			{
				this.OnDisabled(this);
			}
		}

		// Token: 0x02000B36 RID: 2870
		// (Invoke) Token: 0x06004776 RID: 18294
		public delegate void OnDisabledEvent(GameObjectOnDisableDispatcher me);
	}
}
