using System;
using UnityEngine;

// Token: 0x02000475 RID: 1141
public class GorillaQuitBox : GorillaTriggerBox
{
	// Token: 0x06001BD1 RID: 7121 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Start()
	{
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x00087C8D File Offset: 0x00085E8D
	public override void OnBoxTriggered()
	{
		Application.Quit();
	}
}
