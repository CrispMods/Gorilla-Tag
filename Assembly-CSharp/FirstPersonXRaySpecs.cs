using System;
using UnityEngine;

// Token: 0x02000437 RID: 1079
public class FirstPersonXRaySpecs : MonoBehaviour
{
	// Token: 0x06001A94 RID: 6804 RVA: 0x000830A5 File Offset: 0x000812A5
	private void OnEnable()
	{
		GorillaBodyRenderer.SetAllSkeletons(true);
	}

	// Token: 0x06001A95 RID: 6805 RVA: 0x000830AD File Offset: 0x000812AD
	private void OnDisable()
	{
		GorillaBodyRenderer.SetAllSkeletons(false);
	}
}
