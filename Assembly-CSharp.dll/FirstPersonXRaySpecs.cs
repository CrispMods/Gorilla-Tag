using System;
using UnityEngine;

// Token: 0x02000437 RID: 1079
public class FirstPersonXRaySpecs : MonoBehaviour
{
	// Token: 0x06001A97 RID: 6807 RVA: 0x0004109C File Offset: 0x0003F29C
	private void OnEnable()
	{
		GorillaBodyRenderer.SetAllSkeletons(true);
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x000410A4 File Offset: 0x0003F2A4
	private void OnDisable()
	{
		GorillaBodyRenderer.SetAllSkeletons(false);
	}
}
