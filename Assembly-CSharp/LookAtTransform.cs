using System;
using UnityEngine;

// Token: 0x02000871 RID: 2161
public class LookAtTransform : MonoBehaviour
{
	// Token: 0x0600348C RID: 13452 RVA: 0x00052998 File Offset: 0x00050B98
	private void Update()
	{
		base.transform.rotation = Quaternion.LookRotation(this.lookAt.position - base.transform.position);
	}

	// Token: 0x040037C1 RID: 14273
	[SerializeField]
	private Transform lookAt;
}
