using System;
using UnityEngine.UI;

// Token: 0x020004C4 RID: 1220
public class BuilderKioskButton : GorillaPressableButton
{
	// Token: 0x06001DA9 RID: 7593 RVA: 0x000918F5 File Offset: 0x0008FAF5
	public override void Start()
	{
		this.currentPieceSet = BuilderKiosk.nullItem;
	}

	// Token: 0x06001DAA RID: 7594 RVA: 0x00091902 File Offset: 0x0008FB02
	public override void UpdateColor()
	{
		if (this.currentPieceSet.isNullItem)
		{
			this.buttonRenderer.material = this.unpressedMaterial;
			this.myText.text = "";
			return;
		}
		base.UpdateColor();
	}

	// Token: 0x06001DAB RID: 7595 RVA: 0x00091939 File Offset: 0x0008FB39
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivation();
	}

	// Token: 0x040020CB RID: 8395
	public BuilderSetManager.BuilderSetStoreItem currentPieceSet;

	// Token: 0x040020CC RID: 8396
	public BuilderKiosk kiosk;

	// Token: 0x040020CD RID: 8397
	public Text setNameText;
}
