using System;
using GorillaTag;
using UnityEngine;

// Token: 0x0200022D RID: 557
[GTStripGameObjectFromBuild("!GT_AUTOMATED_PERF_TEST")]
public class PerfTestFPSCaptureController : MonoBehaviour
{
	// Token: 0x04001042 RID: 4162
	[SerializeField]
	private SerializablePerformanceReport<ScenePerformanceData> performanceSummary;
}
