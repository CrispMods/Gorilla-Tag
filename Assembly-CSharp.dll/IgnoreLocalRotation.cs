using System;
using UnityEngine;

// Token: 0x0200016B RID: 363
public class IgnoreLocalRotation : MonoBehaviour
{
	// Token: 0x06000911 RID: 2321 RVA: 0x000356E9 File Offset: 0x000338E9
	private void LateUpdate()
	{
		base.transform.rotation = Quaternion.identity;
	}
}
