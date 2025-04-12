using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000227 RID: 551
[GTStripGameObjectFromBuild("!GT_AUTOMATED_PERF_TEST")]
public class PerfTestObjectDestroyer : MonoBehaviour
{
	// Token: 0x06000CA2 RID: 3234 RVA: 0x00037E2D File Offset: 0x0003602D
	private void Start()
	{
		UnityEngine.Object.DestroyImmediate(base.gameObject, true);
	}
}
