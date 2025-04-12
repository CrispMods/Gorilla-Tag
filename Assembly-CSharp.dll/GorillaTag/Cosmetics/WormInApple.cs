using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C62 RID: 3170
	public class WormInApple : MonoBehaviour
	{
		// Token: 0x06004EF8 RID: 20216 RVA: 0x00062D9D File Offset: 0x00060F9D
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

		// Token: 0x040052C1 RID: 21185
		[SerializeField]
		private UpdateBlendShapeCosmetic blendShapeCosmetic;

		// Token: 0x040052C2 RID: 21186
		public UnityEvent OnHandTapped;
	}
}
