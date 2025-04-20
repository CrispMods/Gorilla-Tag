using System;
using UnityEngine;

// Token: 0x02000489 RID: 1161
public class GorillaTriggerBoxTeleport : GorillaTriggerBox
{
	// Token: 0x06001C36 RID: 7222 RVA: 0x0004367F File Offset: 0x0004187F
	public override void OnBoxTriggered()
	{
		this.cameraOffest.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
		this.cameraOffest.transform.position = this.teleportLocation;
	}

	// Token: 0x04001F3D RID: 7997
	public Vector3 teleportLocation;

	// Token: 0x04001F3E RID: 7998
	public GameObject cameraOffest;
}
