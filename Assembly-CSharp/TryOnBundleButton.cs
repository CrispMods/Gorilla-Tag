using System;
using GorillaNetworking.Store;

// Token: 0x02000423 RID: 1059
public class TryOnBundleButton : GorillaPressableButton
{
	// Token: 0x06001A26 RID: 6694 RVA: 0x00080B1A File Offset: 0x0007ED1A
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivationWithHand(isLeftHand);
		BundleManager.instance.PressTryOnBundleButton(this, isLeftHand);
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x00080B34 File Offset: 0x0007ED34
	public override void UpdateColor()
	{
		if (this.playfabBundleID == "NULL")
		{
			this.buttonRenderer.material = this.unpressedMaterial;
			if (this.myText != null)
			{
				this.myText.text = "";
			}
			return;
		}
		base.UpdateColor();
	}

	// Token: 0x04001CFA RID: 7418
	public int buttonIndex;

	// Token: 0x04001CFB RID: 7419
	public string playfabBundleID = "NULL";
}
