using System;
using UnityEngine.UI;

// Token: 0x020004D1 RID: 1233
public class BuilderKioskButton : GorillaPressableButton
{
	// Token: 0x06001DFF RID: 7679 RVA: 0x000447B0 File Offset: 0x000429B0
	public override void Start()
	{
		this.currentPieceSet = BuilderKiosk.nullItem;
	}

	// Token: 0x06001E00 RID: 7680 RVA: 0x000447BD File Offset: 0x000429BD
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

	// Token: 0x06001E01 RID: 7681 RVA: 0x000447F4 File Offset: 0x000429F4
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivation();
	}

	// Token: 0x0400211D RID: 8477
	public BuilderSetManager.BuilderSetStoreItem currentPieceSet;

	// Token: 0x0400211E RID: 8478
	public BuilderKiosk kiosk;

	// Token: 0x0400211F RID: 8479
	public Text setNameText;
}
