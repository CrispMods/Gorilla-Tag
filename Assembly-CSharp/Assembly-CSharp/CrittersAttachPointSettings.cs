using System;

// Token: 0x0200003B RID: 59
public class CrittersAttachPointSettings : CrittersActorSettings
{
	// Token: 0x06000123 RID: 291 RVA: 0x00008545 File Offset: 0x00006745
	public override void UpdateActorSettings()
	{
		base.UpdateActorSettings();
		CrittersAttachPoint crittersAttachPoint = (CrittersAttachPoint)this.parentActor;
		crittersAttachPoint.anchorLocation = this.anchoredLocation;
		crittersAttachPoint.rb.isKinematic = true;
		crittersAttachPoint.isLeft = this.isLeft;
	}

	// Token: 0x0400015F RID: 351
	public bool isLeft;

	// Token: 0x04000160 RID: 352
	public CrittersAttachPoint.AnchoredLocationTypes anchoredLocation;
}
