using System;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class BeePerchPoint : MonoBehaviour
{
	// Token: 0x060005DB RID: 1499 RVA: 0x00023073 File Offset: 0x00021273
	public Vector3 GetPoint()
	{
		return base.transform.TransformPoint(this.localPosition);
	}

	// Token: 0x04000714 RID: 1812
	[SerializeField]
	private Vector3 localPosition;
}
