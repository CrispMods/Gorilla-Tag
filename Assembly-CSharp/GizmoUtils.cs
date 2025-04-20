﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Drawing;
using UnityEngine;

// Token: 0x02000868 RID: 2152
public static class GizmoUtils
{
	// Token: 0x0600345B RID: 13403 RVA: 0x0013DD80 File Offset: 0x0013BF80
	[Conditional("UNITY_EDITOR")]
	public unsafe static void DrawGizmo(this Collider c, Color color = default(Color))
	{
		if (c == null)
		{
			return;
		}
		if (color == default(Color) && !GizmoUtils.gColliderToColor.TryGetValue(c, out color))
		{
			color = new SRand(c.GetHashCode()).NextColor();
			GizmoUtils.gColliderToColor.Add(c, color);
		}
		CommandBuilder commandBuilder = *Draw.ingame;
		using (commandBuilder.WithMatrix(c.transform.localToWorldMatrix))
		{
			BoxCollider boxCollider = c as BoxCollider;
			if (boxCollider == null)
			{
				SphereCollider sphereCollider = c as SphereCollider;
				if (sphereCollider == null)
				{
					CapsuleCollider capsuleCollider = c as CapsuleCollider;
					if (capsuleCollider == null)
					{
						MeshCollider meshCollider = c as MeshCollider;
						if (meshCollider != null)
						{
							commandBuilder.WireMesh(meshCollider.sharedMesh, color);
						}
					}
					else
					{
						commandBuilder.WireCapsule(capsuleCollider.center, Vector3.up, capsuleCollider.height, capsuleCollider.radius, color);
					}
				}
				else
				{
					commandBuilder.WireSphere(sphereCollider.center, sphereCollider.radius, color);
				}
			}
			else
			{
				commandBuilder.WireBox(boxCollider.center, boxCollider.size, color);
			}
		}
	}

	// Token: 0x0600345C RID: 13404 RVA: 0x00030607 File Offset: 0x0002E807
	[Conditional("UNITY_EDITOR")]
	public static void DrawWireCubeTRS(Vector3 t, Quaternion r, Vector3 s)
	{
	}

	// Token: 0x040037A9 RID: 14249
	private static readonly Dictionary<Collider, Color> gColliderToColor = new Dictionary<Collider, Color>(64);
}
