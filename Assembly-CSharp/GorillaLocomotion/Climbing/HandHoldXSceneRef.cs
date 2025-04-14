using System;
using UnityEngine;

namespace GorillaLocomotion.Climbing
{
	// Token: 0x02000B63 RID: 2915
	public class HandHoldXSceneRef : MonoBehaviour
	{
		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x060048F7 RID: 18679 RVA: 0x00162B94 File Offset: 0x00160D94
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

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x060048F8 RID: 18680 RVA: 0x00162BB4 File Offset: 0x00160DB4
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

		// Token: 0x04004BA0 RID: 19360
		[SerializeField]
		public XSceneRef reference;
	}
}
