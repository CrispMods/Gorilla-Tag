using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameObjectScheduling
{
	// Token: 0x02000C8A RID: 3210
	public class GameObjectSchedulerEventDispatcher : MonoBehaviour
	{
		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x060050F9 RID: 20729 RVA: 0x00189095 File Offset: 0x00187295
		public UnityEvent OnScheduledActivation
		{
			get
			{
				return this.onScheduledActivation;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x060050FA RID: 20730 RVA: 0x0018909D File Offset: 0x0018729D
		public UnityEvent OnScheduledDeactivation
		{
			get
			{
				return this.onScheduledDeactivation;
			}
		}

		// Token: 0x04005356 RID: 21334
		[SerializeField]
		private UnityEvent onScheduledActivation;

		// Token: 0x04005357 RID: 21335
		[SerializeField]
		private UnityEvent onScheduledDeactivation;
	}
}
