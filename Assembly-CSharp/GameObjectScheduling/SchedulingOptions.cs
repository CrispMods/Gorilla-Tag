using System;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000C8C RID: 3212
	[CreateAssetMenu(fileName = "New Options", menuName = "Game Object Scheduling/Options", order = 0)]
	public class SchedulingOptions : ScriptableObject
	{
		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x060050FD RID: 20733 RVA: 0x001890A5 File Offset: 0x001872A5
		public DateTime DtDebugServerTime
		{
			get
			{
				return this.dtDebugServerTime.AddSeconds((double)(Time.time * this.timescale));
			}
		}

		// Token: 0x0400535A RID: 21338
		[SerializeField]
		private string debugServerTime;

		// Token: 0x0400535B RID: 21339
		[SerializeField]
		private DateTime dtDebugServerTime;

		// Token: 0x0400535C RID: 21340
		[SerializeField]
		[Range(-60f, 3660f)]
		private float timescale = 1f;
	}
}
