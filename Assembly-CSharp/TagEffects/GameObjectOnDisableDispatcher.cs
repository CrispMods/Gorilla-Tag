using System;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B62 RID: 2914
	public class GameObjectOnDisableDispatcher : MonoBehaviour
	{
		// Token: 0x14000085 RID: 133
		// (add) Token: 0x060048BA RID: 18618 RVA: 0x00190308 File Offset: 0x0018E508
		// (remove) Token: 0x060048BB RID: 18619 RVA: 0x00190340 File Offset: 0x0018E540
		public event GameObjectOnDisableDispatcher.OnDisabledEvent OnDisabled;

		// Token: 0x060048BC RID: 18620 RVA: 0x0005F552 File Offset: 0x0005D752
		private void OnDisable()
		{
			if (this.OnDisabled != null)
			{
				this.OnDisabled(this);
			}
		}

		// Token: 0x02000B63 RID: 2915
		// (Invoke) Token: 0x060048BF RID: 18623
		public delegate void OnDisabledEvent(GameObjectOnDisableDispatcher me);
	}
}
