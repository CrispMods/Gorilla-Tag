using System;
using UnityEngine;

// Token: 0x0200016B RID: 363
public class IgnoreLocalRotation : MonoBehaviour
{
	// Token: 0x06000911 RID: 2321 RVA: 0x0003141F File Offset: 0x0002F61F
	private void LateUpdate()
	{
		base.transform.rotation = Quaternion.identity;
	}
}
