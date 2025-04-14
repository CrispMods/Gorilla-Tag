using System;
using UnityEngine;

// Token: 0x0200030C RID: 780
public class EnableUnpremultipliedAlpha : MonoBehaviour
{
	// Token: 0x06001296 RID: 4758 RVA: 0x00059248 File Offset: 0x00057448
	private void Start()
	{
		OVRManager.eyeFovPremultipliedAlphaModeEnabled = false;
	}
}
