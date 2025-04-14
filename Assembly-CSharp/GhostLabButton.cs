using System;
using UnityEngine;

// Token: 0x020000AD RID: 173
public class GhostLabButton : GorillaPressableButton, IBuildValidation
{
	// Token: 0x06000460 RID: 1120 RVA: 0x0001A094 File Offset: 0x00018294
	public bool BuildValidationCheck()
	{
		if (this.ghostLab == null)
		{
			Debug.LogError("ghostlab is missing", this);
			return false;
		}
		return true;
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x0001A0B2 File Offset: 0x000182B2
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.ghostLab.DoorButtonPress(this.buttonIndex, this.forSingleDoor);
	}

	// Token: 0x04000516 RID: 1302
	public GhostLab ghostLab;

	// Token: 0x04000517 RID: 1303
	public int buttonIndex;

	// Token: 0x04000518 RID: 1304
	public bool forSingleDoor;
}
