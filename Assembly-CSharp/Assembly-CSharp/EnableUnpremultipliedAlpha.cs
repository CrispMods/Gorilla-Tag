using System;
using UnityEngine;

// Token: 0x0200030C RID: 780
public class EnableUnpremultipliedAlpha : MonoBehaviour
{
	// Token: 0x06001299 RID: 4761 RVA: 0x000595CC File Offset: 0x000577CC
	private void Start()
	{
		OVRManager.eyeFovPremultipliedAlphaModeEnabled = false;
	}
}
