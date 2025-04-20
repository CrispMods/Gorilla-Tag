using System;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class GhostLabButton : GorillaPressableButton, IBuildValidation
{
	// Token: 0x0600049C RID: 1180 RVA: 0x000336E6 File Offset: 0x000318E6
	public bool BuildValidationCheck()
	{
		if (this.ghostLab == null)
		{
			Debug.LogError("ghostlab is missing", this);
			return false;
		}
		return true;
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x00033704 File Offset: 0x00031904
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.ghostLab.DoorButtonPress(this.buttonIndex, this.forSingleDoor);
	}

	// Token: 0x04000556 RID: 1366
	public GhostLab ghostLab;

	// Token: 0x04000557 RID: 1367
	public int buttonIndex;

	// Token: 0x04000558 RID: 1368
	public bool forSingleDoor;
}
