using System;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C5F RID: 3167
	public class WormInApple : MonoBehaviour
	{
		// Token: 0x06004EEC RID: 20204 RVA: 0x00183A0A File Offset: 0x00181C0A
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

		// Token: 0x040052AF RID: 21167
		[SerializeField]
		private UpdateBlendShapeCosmetic blendShapeCosmetic;

		// Token: 0x040052B0 RID: 21168
		public UnityEvent OnHandTapped;
	}
}
