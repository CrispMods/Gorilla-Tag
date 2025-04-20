using System;
using UnityEngine;

// Token: 0x02000176 RID: 374
public class IgnoreLocalRotation : MonoBehaviour
{
	// Token: 0x0600095C RID: 2396 RVA: 0x000369B4 File Offset: 0x00034BB4
	private void LateUpdate()
	{
		base.transform.rotation = Quaternion.identity;
	}
}
