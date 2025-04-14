using System;
using UnityEngine;

// Token: 0x0200016B RID: 363
public class IgnoreLocalRotation : MonoBehaviour
{
	// Token: 0x0600090F RID: 2319 RVA: 0x000310FB File Offset: 0x0002F2FB
	private void LateUpdate()
	{
		base.transform.rotation = Quaternion.identity;
	}
}
