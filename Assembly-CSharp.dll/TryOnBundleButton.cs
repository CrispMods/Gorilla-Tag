using System;
using GorillaNetworking.Store;

// Token: 0x02000423 RID: 1059
public class TryOnBundleButton : GorillaPressableButton
{
	// Token: 0x06001A29 RID: 6697 RVA: 0x00040A72 File Offset: 0x0003EC72
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivationWithHand(isLeftHand);
		BundleManager.instance.PressTryOnBundleButton(this, isLeftHand);
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x000D3598 File Offset: 0x000D1798
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

	// Token: 0x04001CFB RID: 7419
	public int buttonIndex;

	// Token: 0x04001CFC RID: 7420
	public string playfabBundleID = "NULL";
}
