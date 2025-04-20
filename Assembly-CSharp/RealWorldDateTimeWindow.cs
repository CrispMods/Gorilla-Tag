using System;
using UniLabs.Time;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class RealWorldDateTimeWindow : ScriptableObject
{
	// Token: 0x06000320 RID: 800 RVA: 0x0003268B File Offset: 0x0003088B
	public bool MatchesDate(DateTime utcDate)
	{
		return this.startTime <= utcDate && this.endTime >= utcDate;
	}

	// Token: 0x040003BF RID: 959
	[SerializeField]
	private UDateTime startTime;

	// Token: 0x040003C0 RID: 960
	[SerializeField]
	private UDateTime endTime;
}
