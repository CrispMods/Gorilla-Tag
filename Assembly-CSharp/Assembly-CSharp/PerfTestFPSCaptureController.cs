using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000222 RID: 546
[GTStripGameObjectFromBuild("!GT_AUTOMATED_PERF_TEST")]
public class PerfTestFPSCaptureController : MonoBehaviour
{
	// Token: 0x04000FFD RID: 4093
	[SerializeField]
	private SerializablePerformanceReport<ScenePerformanceData> performanceSummary;
}
