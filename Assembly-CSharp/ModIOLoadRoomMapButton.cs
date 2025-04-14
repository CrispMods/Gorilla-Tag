using System;
using System.Collections;
using GorillaTagScripts.ModIO;
using UnityEngine;

// Token: 0x020005FF RID: 1535
public class ModIOLoadRoomMapButton : GorillaPressableButton
{
	// Token: 0x06002619 RID: 9753 RVA: 0x000BBFA7 File Offset: 0x000BA1A7
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		base.StartCoroutine(this.ButtonPressed_Local());
		if (CustomMapManager.CanLoadRoomMap())
		{
			CustomMapManager.ApproveAndLoadRoomMap();
		}
	}

	// Token: 0x0600261A RID: 9754 RVA: 0x000BBFC8 File Offset: 0x000BA1C8
	private IEnumerator ButtonPressed_Local()
	{
		this.isOn = true;
		this.UpdateColor();
		yield return new WaitForSeconds(this.pressedTime);
		this.isOn = false;
		this.UpdateColor();
		yield break;
	}

	// Token: 0x04002A1E RID: 10782
	[SerializeField]
	private float pressedTime = 0.2f;
}
