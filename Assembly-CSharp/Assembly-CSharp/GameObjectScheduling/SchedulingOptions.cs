using System;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000C8F RID: 3215
	[CreateAssetMenu(fileName = "New Options", menuName = "Game Object Scheduling/Options", order = 0)]
	public class SchedulingOptions : ScriptableObject
	{
		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06005109 RID: 20745 RVA: 0x0018966D File Offset: 0x0018786D
		public DateTime DtDebugServerTime
		{
			get
			{
				return this.dtDebugServerTime.AddSeconds((double)(Time.time * this.timescale));
			}
		}

		// Token: 0x0400536C RID: 21356
		[SerializeField]
		private string debugServerTime;

		// Token: 0x0400536D RID: 21357
		[SerializeField]
		private DateTime dtDebugServerTime;

		// Token: 0x0400536E RID: 21358
		[SerializeField]
		[Range(-60f, 3660f)]
		private float timescale = 1f;
	}
}
