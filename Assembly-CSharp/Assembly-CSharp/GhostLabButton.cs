using System;
using UnityEngine;

// Token: 0x020000AD RID: 173
public class GhostLabButton : GorillaPressableButton, IBuildValidation
{
	// Token: 0x06000462 RID: 1122 RVA: 0x0001A3B8 File Offset: 0x000185B8
	public bool BuildValidationCheck()
	{
		if (this.ghostLab == null)
		{
			Debug.LogError("ghostlab is missing", this);
			return false;
		}
		return true;
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0001A3D6 File Offset: 0x000185D6
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.ghostLab.DoorButtonPress(this.buttonIndex, this.forSingleDoor);
	}

	// Token: 0x04000517 RID: 1303
	public GhostLab ghostLab;

	// Token: 0x04000518 RID: 1304
	public int buttonIndex;

	// Token: 0x04000519 RID: 1305
	public bool forSingleDoor;
}
