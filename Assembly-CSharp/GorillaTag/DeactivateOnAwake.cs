using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B91 RID: 2961
	public class DeactivateOnAwake : MonoBehaviour
	{
		// Token: 0x06004AC5 RID: 19141 RVA: 0x00169D39 File Offset: 0x00167F39
		private void Awake()
		{
			base.gameObject.SetActive(false);
			Object.Destroy(this);
		}
	}
}
