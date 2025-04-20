using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003EC RID: 1004
public class HitChecker : MonoBehaviour
{
	// Token: 0x06001863 RID: 6243 RVA: 0x000CBD3C File Offset: 0x000C9F3C
	public static void CheckHandHit(ref int collidersHitCount, LayerMask layerMask, float sphereRadius, ref RaycastHit nullHit, ref RaycastHit[] raycastHits, ref List<RaycastHit> raycastHitList, ref Vector3 spherecastSweep, ref GorillaTriggerColliderHandIndicator handIndicator)
	{
		spherecastSweep = handIndicator.transform.position - handIndicator.lastPosition;
		if (spherecastSweep.magnitude < 0.0001f)
		{
			spherecastSweep = Vector3.up * 0.0001f;
		}
		for (int i = 0; i < raycastHits.Length; i++)
		{
			raycastHits[i] = nullHit;
		}
		collidersHitCount = Physics.SphereCastNonAlloc(handIndicator.lastPosition, sphereRadius, spherecastSweep.normalized, raycastHits, spherecastSweep.magnitude, layerMask, QueryTriggerInteraction.Collide);
		if (collidersHitCount > 0)
		{
			raycastHitList.Clear();
			for (int j = 0; j < raycastHits.Length; j++)
			{
				if (raycastHits[j].collider != null)
				{
					raycastHitList.Add(raycastHits[j]);
				}
			}
		}
	}

	// Token: 0x06001864 RID: 6244 RVA: 0x000CBE1C File Offset: 0x000CA01C
	public static bool CheckHandIn(ref bool anyHit, ref Collider[] colliderHit, float sphereRadius, int layerMask, ref GorillaTriggerColliderHandIndicator handIndicator, ref List<Collider> collidersToBeIn)
	{
		anyHit = (Physics.OverlapSphereNonAlloc(handIndicator.transform.position, sphereRadius, colliderHit, layerMask, QueryTriggerInteraction.Collide) > 0);
		if (anyHit)
		{
			anyHit = false;
			for (int i = 0; i < colliderHit.Length; i++)
			{
				if (collidersToBeIn.Contains(colliderHit[i]))
				{
					anyHit = true;
					break;
				}
			}
		}
		return anyHit;
	}

	// Token: 0x06001865 RID: 6245 RVA: 0x000408FE File Offset: 0x0003EAFE
	public static int RayCastHitCompare(RaycastHit a, RaycastHit b)
	{
		if (a.distance < b.distance)
		{
			return -1;
		}
		if (a.distance == b.distance)
		{
			return 0;
		}
		return 1;
	}
}
