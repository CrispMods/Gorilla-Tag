using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002D0 RID: 720
public class TeleportAimHandlerLaser : TeleportAimHandler
{
	// Token: 0x06001173 RID: 4467 RVA: 0x000ABE18 File Offset: 0x000AA018
	public override void GetPoints(List<Vector3> points)
	{
		Ray ray;
		base.LocomotionTeleport.InputHandler.GetAimData(out ray);
		points.Add(ray.origin);
		points.Add(ray.origin + ray.direction * this.Range);
	}

	// Token: 0x0400135B RID: 4955
	[Tooltip("Maximum range for aiming.")]
	public float Range = 100f;
}
