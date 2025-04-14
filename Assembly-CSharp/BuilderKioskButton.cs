using System;
using UnityEngine.UI;

// Token: 0x020004C4 RID: 1220
public class BuilderKioskButton : GorillaPressableButton
{
	// Token: 0x06001DA6 RID: 7590 RVA: 0x00091571 File Offset: 0x0008F771
	public override void Start()
	{
		this.currentPieceSet = BuilderKiosk.nullItem;
	}

	// Token: 0x06001DA7 RID: 7591 RVA: 0x0009157E File Offset: 0x0008F77E
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

	// Token: 0x06001DA8 RID: 7592 RVA: 0x000915B5 File Offset: 0x0008F7B5
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivation();
	}

	// Token: 0x040020CA RID: 8394
	public BuilderSetManager.BuilderSetStoreItem currentPieceSet;

	// Token: 0x040020CB RID: 8395
	public BuilderKiosk kiosk;

	// Token: 0x040020CC RID: 8396
	public Text setNameText;
}
