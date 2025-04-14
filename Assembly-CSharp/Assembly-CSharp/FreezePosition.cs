using System;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class FreezePosition : MonoBehaviour
{
	// Token: 0x06000338 RID: 824 RVA: 0x00014F0C File Offset: 0x0001310C
	private void FixedUpdate()
	{
		if (this.target)
		{
			this.target.localPosition = this.localPosition;
		}
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00014F0C File Offset: 0x0001310C
	private void LateUpdate()
	{
		if (this.target)
		{
			this.target.localPosition = this.localPosition;
		}
	}

	// Token: 0x040003BF RID: 959
	public Transform target;

	// Token: 0x040003C0 RID: 960
	public Vector3 localPosition;
}
