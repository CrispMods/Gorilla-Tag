using System;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class FreezePosition : MonoBehaviour
{
	// Token: 0x06000336 RID: 822 RVA: 0x00014BE8 File Offset: 0x00012DE8
	private void FixedUpdate()
	{
		if (this.target)
		{
			this.target.localPosition = this.localPosition;
		}
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00014BE8 File Offset: 0x00012DE8
	private void LateUpdate()
	{
		if (this.target)
		{
			this.target.localPosition = this.localPosition;
		}
	}

	// Token: 0x040003BE RID: 958
	public Transform target;

	// Token: 0x040003BF RID: 959
	public Vector3 localPosition;
}
