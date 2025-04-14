using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B94 RID: 2964
	public class DeactivateOnAwake : MonoBehaviour
	{
		// Token: 0x06004AD1 RID: 19153 RVA: 0x0016A301 File Offset: 0x00168501
		private void Awake()
		{
			base.gameObject.SetActive(false);
			Object.Destroy(this);
		}
	}
}
