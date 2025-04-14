using System;

// Token: 0x02000610 RID: 1552
public class ModIOMapTelemetryTrigger : GorillaTriggerBox
{
	// Token: 0x060026AB RID: 9899 RVA: 0x000BE8B7 File Offset: 0x000BCAB7
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

	// Token: 0x04002A8C RID: 10892
	public bool StartMapTrackingOnTrigger;

	// Token: 0x04002A8D RID: 10893
	public bool StopMapTrackingOnTrigger;
}
