using System;

// Token: 0x02000611 RID: 1553
public class ModIOMapTelemetryTrigger : GorillaTriggerBox
{
	// Token: 0x060026B3 RID: 9907 RVA: 0x000BED37 File Offset: 0x000BCF37
	public override void OnBoxTriggered()
	{
		base.OnBoxTriggered();
		if (this.StartMapTrackingOnTrigger && !ModIOTelemetry.IsActive)
		{
			ModIOTelemetry.StartMapTracking();
			return;
		}
		if (this.StopMapTrackingOnTrigger && ModIOTelemetry.IsActive)
		{
			ModIOTelemetry.EndMapTracking();
		}
	}

	// Token: 0x04002A92 RID: 10898
	public bool StartMapTrackingOnTrigger;

	// Token: 0x04002A93 RID: 10899
	public bool StopMapTrackingOnTrigger;
}
