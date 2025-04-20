using System;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class BeePerchPoint : MonoBehaviour
{
	// Token: 0x0600061C RID: 1564 RVA: 0x00034939 File Offset: 0x00032B39
	public Vector3 GetPoint()
	{
		return base.transform.TransformPoint(this.localPosition);
	}

	// Token: 0x04000755 RID: 1877
	[SerializeField]
	private Vector3 localPosition;
}
