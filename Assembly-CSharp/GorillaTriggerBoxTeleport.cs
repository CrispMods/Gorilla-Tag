using System;
using UnityEngine;

// Token: 0x0200047D RID: 1149
public class GorillaTriggerBoxTeleport : GorillaTriggerBox
{
	// Token: 0x06001BE2 RID: 7138 RVA: 0x00087DBD File Offset: 0x00085FBD
	public override void OnBoxTriggered()
	{
		this.cameraOffest.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
		this.cameraOffest.transform.position = this.teleportLocation;
	}

	// Token: 0x04001EEE RID: 7918
	public Vector3 teleportLocation;

	// Token: 0x04001EEF RID: 7919
	public GameObject cameraOffest;
}
