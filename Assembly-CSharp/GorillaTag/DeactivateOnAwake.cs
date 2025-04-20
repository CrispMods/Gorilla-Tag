using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BBE RID: 3006
	public class DeactivateOnAwake : MonoBehaviour
	{
		// Token: 0x06004C10 RID: 19472 RVA: 0x00062049 File Offset: 0x00060249
		private void Awake()
		{
			base.gameObject.SetActive(false);
			UnityEngine.Object.Destroy(this);
		}
	}
}
