using System;
using UnityEngine;

// Token: 0x02000475 RID: 1141
public class GorillaQuitBox : GorillaTriggerBox
{
	// Token: 0x06001BD4 RID: 7124 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Start()
	{
	}

	// Token: 0x06001BD5 RID: 7125 RVA: 0x00088011 File Offset: 0x00086211
	public override void OnBoxTriggered()
	{
		Application.Quit();
	}
}
