using System;
using UnityEngine;

// Token: 0x02000317 RID: 791
public class EnableUnpremultipliedAlpha : MonoBehaviour
{
	// Token: 0x060012E2 RID: 4834 RVA: 0x0003CF1F File Offset: 0x0003B11F
	private void Start()
	{
		OVRManager.eyeFovPremultipliedAlphaModeEnabled = false;
	}
}
