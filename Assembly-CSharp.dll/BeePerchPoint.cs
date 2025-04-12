using System;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class BeePerchPoint : MonoBehaviour
{
	// Token: 0x060005DD RID: 1501 RVA: 0x000336D5 File Offset: 0x000318D5
	public Vector3 GetPoint()
	{
		return base.transform.TransformPoint(this.localPosition);
	}

	// Token: 0x04000715 RID: 1813
	[SerializeField]
	private Vector3 localPosition;
}
