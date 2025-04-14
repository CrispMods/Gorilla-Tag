using System;
using System.Collections;
using GorillaTagScripts.ModIO;
using UnityEngine;

// Token: 0x02000600 RID: 1536
public class ModIOLoadRoomMapButton : GorillaPressableButton
{
	// Token: 0x06002621 RID: 9761 RVA: 0x000BC427 File Offset: 0x000BA627
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		base.StartCoroutine(this.ButtonPressed_Local());
		if (CustomMapManager.CanLoadRoomMap())
		{
			CustomMapManager.ApproveAndLoadRoomMap();
		}
	}

	// Token: 0x06002622 RID: 9762 RVA: 0x000BC448 File Offset: 0x000BA648
	private IEnumerator ButtonPressed_Local()
	{
		this.isOn = true;
		this.UpdateColor();
		yield return new WaitForSeconds(this.pressedTime);
		this.isOn = false;
		this.UpdateColor();
		yield break;
	}

	// Token: 0x04002A24 RID: 10788
	[SerializeField]
	private float pressedTime = 0.2f;
}
