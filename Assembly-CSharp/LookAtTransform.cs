using System;
using UnityEngine;

// Token: 0x02000857 RID: 2135
public class LookAtTransform : MonoBehaviour
{
	// Token: 0x060033D1 RID: 13265 RVA: 0x000F6E77 File Offset: 0x000F5077
	private void Update()
	{
		base.transform.rotation = Quaternion.LookRotation(this.lookAt.position - base.transform.position);
	}

	// Token: 0x04003705 RID: 14085
	[SerializeField]
	private Transform lookAt;
}
