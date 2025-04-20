using System;
using GorillaNetworking.Store;

// Token: 0x0200042F RID: 1071
public class TryOnBundleButton : GorillaPressableButton
{
	// Token: 0x06001A7A RID: 6778 RVA: 0x00041DAB File Offset: 0x0003FFAB
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivationWithHand(isLeftHand);
		BundleManager.instance.PressTryOnBundleButton(this, isLeftHand);
	}

	// Token: 0x06001A7B RID: 6779 RVA: 0x000D6250 File Offset: 0x000D4450
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

	// Token: 0x04001D49 RID: 7497
	public int buttonIndex;

	// Token: 0x04001D4A RID: 7498
	public string playfabBundleID = "NULL";
}
