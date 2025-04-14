using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020000D0 RID: 208
public class ScheduledTimelinePlayer : MonoBehaviour
{
	// Token: 0x0600056B RID: 1387 RVA: 0x000203E7 File Offset: 0x0001E5E7
	protected void OnEnable()
	{
		this.scheduledEventID = BetterDayNightManager.RegisterScheduledEvent(this.eventHour, new Action(this.HandleScheduledEvent));
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x00020406 File Offset: 0x0001E606
	protected void OnDisable()
	{
		BetterDayNightManager.UnregisterScheduledEvent(this.scheduledEventID);
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x00020413 File Offset: 0x0001E613
	private void HandleScheduledEvent()
	{
		this.timeline.Play();
	}

	// Token: 0x04000648 RID: 1608
	public PlayableDirector timeline;

	// Token: 0x04000649 RID: 1609
	public int eventHour = 7;

	// Token: 0x0400064A RID: 1610
	private int scheduledEventID;
}
