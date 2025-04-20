using System;
using UnityEngine;

namespace GorillaLocomotion.Climbing
{
	// Token: 0x02000B90 RID: 2960
	public class HandHoldXSceneRef : MonoBehaviour
	{
		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06004A42 RID: 19010 RVA: 0x0019DF60 File Offset: 0x0019C160
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

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06004A43 RID: 19011 RVA: 0x0019DF80 File Offset: 0x0019C180
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

		// Token: 0x04004C96 RID: 19606
		[SerializeField]
		public XSceneRef reference;
	}
}
