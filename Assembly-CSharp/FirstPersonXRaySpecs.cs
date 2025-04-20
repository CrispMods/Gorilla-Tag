using System;
using UnityEngine;

// Token: 0x02000443 RID: 1091
public class FirstPersonXRaySpecs : MonoBehaviour
{
	// Token: 0x06001AE8 RID: 6888 RVA: 0x000423D5 File Offset: 0x000405D5
	private void OnEnable()
	{
		GorillaBodyRenderer.SetAllSkeletons(true);
	}

	// Token: 0x06001AE9 RID: 6889 RVA: 0x000423DD File Offset: 0x000405DD
	private void OnDisable()
	{
		GorillaBodyRenderer.SetAllSkeletons(false);
	}
}
