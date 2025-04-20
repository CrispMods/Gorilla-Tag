using System;
using System.Collections;
using GorillaTagScripts.ModIO;
using UnityEngine;

// Token: 0x02000660 RID: 1632
public class CustomMapsLoadRoomMapButton : GorillaPressableButton
{
	// Token: 0x06002865 RID: 10341 RVA: 0x0004B752 File Offset: 0x00049952
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		base.StartCoroutine(this.ButtonPressed_Local());
		if (CustomMapManager.CanLoadRoomMap())
		{
			CustomMapManager.ApproveAndLoadRoomMap();
		}
	}

	// Token: 0x06002866 RID: 10342 RVA: 0x0004B773 File Offset: 0x00049973
	private IEnumerator ButtonPressed_Local()
	{
		this.isOn = true;
		this.UpdateColor();
		yield return new WaitForSeconds(this.pressedTime);
		this.isOn = false;
		this.UpdateColor();
		yield break;
	}

	// Token: 0x04002DCD RID: 11725
	[SerializeField]
	private float pressedTime = 0.2f;
}
