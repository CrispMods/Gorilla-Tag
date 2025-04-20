using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020000DA RID: 218
public class ScheduledTimelinePlayer : MonoBehaviour
{
	// Token: 0x060005AA RID: 1450 RVA: 0x00034245 File Offset: 0x00032445
	protected void OnEnable()
	{
		this.scheduledEventID = BetterDayNightManager.RegisterScheduledEvent(this.eventHour, new Action(this.HandleScheduledEvent));
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x00034264 File Offset: 0x00032464
	protected void OnDisable()
	{
		BetterDayNightManager.UnregisterScheduledEvent(this.scheduledEventID);
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x00034271 File Offset: 0x00032471
	private void HandleScheduledEvent()
	{
		this.timeline.Play();
	}

	// Token: 0x04000688 RID: 1672
	public PlayableDirector timeline;

	// Token: 0x04000689 RID: 1673
	public int eventHour = 7;

	// Token: 0x0400068A RID: 1674
	private int scheduledEventID;
}
