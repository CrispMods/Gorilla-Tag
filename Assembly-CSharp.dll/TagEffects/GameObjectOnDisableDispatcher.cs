using System;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B38 RID: 2872
	public class GameObjectOnDisableDispatcher : MonoBehaviour
	{
		// Token: 0x14000081 RID: 129
		// (add) Token: 0x0600477D RID: 18301 RVA: 0x00189394 File Offset: 0x00187594
		// (remove) Token: 0x0600477E RID: 18302 RVA: 0x001893CC File Offset: 0x001875CC
		public event GameObjectOnDisableDispatcher.OnDisabledEvent OnDisabled;

		// Token: 0x0600477F RID: 18303 RVA: 0x0005DB3B File Offset: 0x0005BD3B
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
