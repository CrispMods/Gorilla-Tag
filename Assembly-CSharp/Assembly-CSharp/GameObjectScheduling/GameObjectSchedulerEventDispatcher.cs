using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameObjectScheduling
{
	// Token: 0x02000C8D RID: 3213
	public class GameObjectSchedulerEventDispatcher : MonoBehaviour
	{
		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06005105 RID: 20741 RVA: 0x0018965D File Offset: 0x0018785D
		public UnityEvent OnScheduledActivation
		{
			get
			{
				return this.onScheduledActivation;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06005106 RID: 20742 RVA: 0x00189665 File Offset: 0x00187865
		public UnityEvent OnScheduledDeactivation
		{
			get
			{
				return this.onScheduledDeactivation;
			}
		}

		// Token: 0x04005368 RID: 21352
		[SerializeField]
		private UnityEvent onScheduledActivation;

		// Token: 0x04005369 RID: 21353
		[SerializeField]
		private UnityEvent onScheduledDeactivation;
	}
}
