using System;
using UnityEngine;

// Token: 0x02000049 RID: 73
public class CrittersGrabberSettings : CrittersActorSettings
{
	// Token: 0x06000167 RID: 359 RVA: 0x00009684 File Offset: 0x00007884
	public override void UpdateActorSettings()
	{
		base.UpdateActorSettings();
		CrittersGrabber crittersGrabber = (CrittersGrabber)this.parentActor;
		crittersGrabber.grabPosition = this._grabPosition;
		crittersGrabber.grabDistance = this._grabDistance;
	}

	// Token: 0x040001B8 RID: 440
	public Transform _grabPosition;

	// Token: 0x040001B9 RID: 441
	public float _grabDistance;
}
