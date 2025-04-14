using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000228 RID: 552
[GTStripGameObjectFromBuild("!GT_AUTOMATED_PERF_TEST")]
public class TestTeleportDestination : MonoBehaviour
{
	// Token: 0x06000CA4 RID: 3236 RVA: 0x00042F38 File Offset: 0x00041138
	private void OnDrawGizmosSelected()
	{
		Debug.DrawRay(base.transform.position, base.transform.forward * 2f, Color.magenta);
	}

	// Token: 0x04001009 RID: 4105
	public GTZone[] zones;

	// Token: 0x0400100A RID: 4106
	public GameObject teleportTransform;
}
