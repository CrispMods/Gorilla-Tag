using System;
using UnityEngine;

// Token: 0x0200086F RID: 2159
public class PredicatableRandomRotation : MonoBehaviour
{
	// Token: 0x06003436 RID: 13366 RVA: 0x000F88A4 File Offset: 0x000F6AA4
	private void Start()
	{
		if (this.source == null)
		{
			this.source = base.transform;
		}
	}

	// Token: 0x06003437 RID: 13367 RVA: 0x000F88C0 File Offset: 0x000F6AC0
	private void Update()
	{
		float d = (this.source.position.x * this.source.position.x + this.source.position.y * this.source.position.y + this.source.position.z * this.source.position.z) % 1f;
		base.transform.Rotate(this.rot * d);
	}

	// Token: 0x0400371D RID: 14109
	[SerializeField]
	private Vector3 rot = Vector3.zero;

	// Token: 0x0400371E RID: 14110
	[SerializeField]
	private Transform source;
}
