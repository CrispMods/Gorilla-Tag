using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000233 RID: 563
[GTStripGameObjectFromBuild("!GT_AUTOMATED_PERF_TEST")]
public class TestTeleportDestination : MonoBehaviour
{
	// Token: 0x06000CED RID: 3309 RVA: 0x000390FB File Offset: 0x000372FB
	private void OnDrawGizmosSelected()
	{
		Debug.DrawRay(base.transform.position, base.transform.forward * 2f, Color.magenta);
	}

	// Token: 0x0400104E RID: 4174
	public GTZone[] zones;

	// Token: 0x0400104F RID: 4175
	public GameObject teleportTransform;
}
