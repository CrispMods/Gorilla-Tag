using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameObjectScheduling
{
	// Token: 0x02000CBB RID: 3259
	public class GameObjectSchedulerEventDispatcher : MonoBehaviour
	{
		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x0600525B RID: 21083 RVA: 0x00065669 File Offset: 0x00063869
		public UnityEvent OnScheduledActivation
		{
			get
			{
				return this.onScheduledActivation;
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x0600525C RID: 21084 RVA: 0x00065671 File Offset: 0x00063871
		public UnityEvent OnScheduledDeactivation
		{
			get
			{
				return this.onScheduledDeactivation;
			}
		}

		// Token: 0x04005462 RID: 21602
		[SerializeField]
		private UnityEvent onScheduledActivation;

		// Token: 0x04005463 RID: 21603
		[SerializeField]
		private UnityEvent onScheduledDeactivation;
	}
}
