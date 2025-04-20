using System;
using UnityEngine;

// Token: 0x02000081 RID: 129
public class FreezePosition : MonoBehaviour
{
	// Token: 0x06000368 RID: 872 RVA: 0x00032A17 File Offset: 0x00030C17
	private void FixedUpdate()
	{
		if (this.target)
		{
			this.target.localPosition = this.localPosition;
		}
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00032A17 File Offset: 0x00030C17
	private void LateUpdate()
	{
		if (this.target)
		{
			this.target.localPosition = this.localPosition;
		}
	}

	// Token: 0x040003F2 RID: 1010
	public Transform target;

	// Token: 0x040003F3 RID: 1011
	public Vector3 localPosition;
}
