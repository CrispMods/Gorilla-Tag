using System;
using UnityEngine;

// Token: 0x02000481 RID: 1153
public class GorillaQuitBox : GorillaTriggerBox
{
	// Token: 0x06001C25 RID: 7205 RVA: 0x00030607 File Offset: 0x0002E807
	private void Start()
	{
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x000435BA File Offset: 0x000417BA
	public override void OnBoxTriggered()
	{
		Application.Quit();
	}
}
