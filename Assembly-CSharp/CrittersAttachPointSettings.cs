using System;

// Token: 0x0200003F RID: 63
public class CrittersAttachPointSettings : CrittersActorSettings
{
	// Token: 0x06000137 RID: 311 RVA: 0x00031149 File Offset: 0x0002F349
	public override void UpdateActorSettings()
	{
		base.UpdateActorSettings();
		CrittersAttachPoint crittersAttachPoint = (CrittersAttachPoint)this.parentActor;
		crittersAttachPoint.anchorLocation = this.anchoredLocation;
		crittersAttachPoint.rb.isKinematic = true;
		crittersAttachPoint.isLeft = this.isLeft;
	}

	// Token: 0x04000170 RID: 368
	public bool isLeft;

	// Token: 0x04000171 RID: 369
	public CrittersAttachPoint.AnchoredLocationTypes anchoredLocation;
}
