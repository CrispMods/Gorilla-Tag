using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002DB RID: 731
public class TeleportAimHandlerLaser : TeleportAimHandler
{
	// Token: 0x060011BC RID: 4540 RVA: 0x000AE6B0 File Offset: 0x000AC8B0
	public override void GetPoints(List<Vector3> points)
	{
		Ray ray;
		base.LocomotionTeleport.InputHandler.GetAimData(out ray);
		points.Add(ray.origin);
		points.Add(ray.origin + ray.direction * this.Range);
	}

	// Token: 0x040013A2 RID: 5026
	[Tooltip("Maximum range for aiming.")]
	public float Range = 100f;
}
