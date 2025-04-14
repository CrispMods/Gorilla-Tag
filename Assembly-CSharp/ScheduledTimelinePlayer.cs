using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020000D0 RID: 208
public class ScheduledTimelinePlayer : MonoBehaviour
{
	// Token: 0x06000569 RID: 1385 RVA: 0x000200C3 File Offset: 0x0001E2C3
	protected void OnEnable()
	{
		this.scheduledEventID = BetterDayNightManager.RegisterScheduledEvent(this.eventHour, new Action(this.HandleScheduledEvent));
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x000200E2 File Offset: 0x0001E2E2
	protected void OnDisable()
	{
		BetterDayNightManager.UnregisterScheduledEvent(this.scheduledEventID);
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x000200EF File Offset: 0x0001E2EF
	private void HandleScheduledEvent()
	{
		this.timeline.Play();
	}

	// Token: 0x04000647 RID: 1607
	public PlayableDirector timeline;

	// Token: 0x04000648 RID: 1608
	public int eventHour = 7;

	// Token: 0x04000649 RID: 1609
	private int scheduledEventID;
}
