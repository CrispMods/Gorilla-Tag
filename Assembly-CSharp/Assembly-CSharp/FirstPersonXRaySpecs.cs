using System;
using UnityEngine;

// Token: 0x02000437 RID: 1079
public class FirstPersonXRaySpecs : MonoBehaviour
{
	// Token: 0x06001A97 RID: 6807 RVA: 0x00083429 File Offset: 0x00081629
	private void OnEnable()
	{
		GorillaBodyRenderer.SetAllSkeletons(true);
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x00083431 File Offset: 0x00081631
	private void OnDisable()
	{
		GorillaBodyRenderer.SetAllSkeletons(false);
	}
}
