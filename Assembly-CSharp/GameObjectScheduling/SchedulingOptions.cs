using System;
using UnityEngine;

namespace GameObjectScheduling
{
	// Token: 0x02000CBD RID: 3261
	[CreateAssetMenu(fileName = "New Options", menuName = "Game Object Scheduling/Options", order = 0)]
	public class SchedulingOptions : ScriptableObject
	{
		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x0600525F RID: 21087 RVA: 0x00065679 File Offset: 0x00063879
		public DateTime DtDebugServerTime
		{
			get
			{
				return this.dtDebugServerTime.AddSeconds((double)(Time.time * this.timescale));
			}
		}

		// Token: 0x04005466 RID: 21606
		[SerializeField]
		private string debugServerTime;

		// Token: 0x04005467 RID: 21607
		[SerializeField]
		private DateTime dtDebugServerTime;

		// Token: 0x04005468 RID: 21608
		[SerializeField]
		[Range(-60f, 3660f)]
		private float timescale = 1f;
	}
}
