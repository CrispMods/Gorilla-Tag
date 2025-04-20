using System;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class CrittersGrabberSettings : CrittersActorSettings
{
	// Token: 0x06000180 RID: 384 RVA: 0x00031533 File Offset: 0x0002F733
	public override void UpdateActorSettings()
	{
		base.UpdateActorSettings();
		CrittersGrabber crittersGrabber = (CrittersGrabber)this.parentActor;
		crittersGrabber.grabPosition = this._grabPosition;
		crittersGrabber.grabDistance = this._grabDistance;
	}

	// Token: 0x040001DB RID: 475
	public Transform _grabPosition;

	// Token: 0x040001DC RID: 476
	public float _grabDistance;
}
