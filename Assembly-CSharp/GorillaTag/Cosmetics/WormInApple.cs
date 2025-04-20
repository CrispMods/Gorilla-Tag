using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C90 RID: 3216
	public class WormInApple : MonoBehaviour
	{
		// Token: 0x0600504C RID: 20556 RVA: 0x000647C2 File Offset: 0x000629C2
		public void OnHandTap()
		{
			if (this.blendShapeCosmetic && this.blendShapeCosmetic.GetBlendValue() > 0.5f)
			{
				UnityEvent onHandTapped = this.OnHandTapped;
				if (onHandTapped == null)
				{
					return;
				}
				onHandTapped.Invoke();
			}
		}

		// Token: 0x040053BB RID: 21435
		[SerializeField]
		private UpdateBlendShapeCosmetic blendShapeCosmetic;

		// Token: 0x040053BC RID: 21436
		public UnityEvent OnHandTapped;
	}
}
