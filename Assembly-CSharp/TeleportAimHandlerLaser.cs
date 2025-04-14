using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002D0 RID: 720
public class TeleportAimHandlerLaser : TeleportAimHandler
{
	// Token: 0x06001170 RID: 4464 RVA: 0x00053598 File Offset: 0x00051798
	public override void GetPoints(List<Vector3> points)
	{
		Ray ray;
		base.LocomotionTeleport.InputHandler.GetAimData(out ray);
		points.Add(ray.origin);
		points.Add(ray.origin + ray.direction * this.Range);
	}

	// Token: 0x0400135A RID: 4954
	[Tooltip("Maximum range for aiming.")]
	public float Range = 100f;
}
