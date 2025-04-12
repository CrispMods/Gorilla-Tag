using System;
using UnityEngine;

// Token: 0x02000475 RID: 1141
public class GorillaQuitBox : GorillaTriggerBox
{
	// Token: 0x06001BD4 RID: 7124 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void Start()
	{
	}

	// Token: 0x06001BD5 RID: 7125 RVA: 0x00042281 File Offset: 0x00040481
	public override void OnBoxTriggered()
	{
		Application.Quit();
	}
}
