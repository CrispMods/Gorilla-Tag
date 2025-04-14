using System;
using UnityEngine;

// Token: 0x0200085A RID: 2138
public class LookAtTransform : MonoBehaviour
{
	// Token: 0x060033DD RID: 13277 RVA: 0x000F743F File Offset: 0x000F563F
	private void Update()
	{
		base.transform.rotation = Quaternion.LookRotation(this.lookAt.position - base.transform.position);
	}

	// Token: 0x04003717 RID: 14103
	[SerializeField]
	private Transform lookAt;
}
