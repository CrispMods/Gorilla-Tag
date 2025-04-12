using System;
using UnityEngine;

namespace GorillaLocomotion.Climbing
{
	// Token: 0x02000B66 RID: 2918
	public class HandHoldXSceneRef : MonoBehaviour
	{
		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06004903 RID: 18691 RVA: 0x00196F48 File Offset: 0x00195148
		public HandHold target
		{
			get
			{
				HandHold result;
				if (this.reference.TryResolve<HandHold>(out result))
				{
					return result;
				}
				return null;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06004904 RID: 18692 RVA: 0x00196F68 File Offset: 0x00195168
		public GameObject targetObject
		{
			get
			{
				GameObject result;
				if (this.reference.TryResolve(out result))
				{
					return result;
				}
				return null;
			}
		}

		// Token: 0x04004BB2 RID: 19378
		[SerializeField]
		public XSceneRef reference;
	}
}
