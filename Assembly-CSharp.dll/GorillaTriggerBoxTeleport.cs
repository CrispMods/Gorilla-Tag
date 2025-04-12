using System;
using UnityEngine;

// Token: 0x0200047D RID: 1149
public class GorillaTriggerBoxTeleport : GorillaTriggerBox
{
	// Token: 0x06001BE5 RID: 7141 RVA: 0x00042346 File Offset: 0x00040546
	public override void OnBoxTriggered()
	{
		this.cameraOffest.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
		this.cameraOffest.transform.position = this.teleportLocation;
	}

	// Token: 0x04001EEF RID: 7919
	public Vector3 teleportLocation;

	// Token: 0x04001EF0 RID: 7920
	public GameObject cameraOffest;
}
