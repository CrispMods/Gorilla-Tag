using System;
using UnityEngine;

// Token: 0x0200088B RID: 2187
public class PredicatableRandomRotation : MonoBehaviour
{
	// Token: 0x06003502 RID: 13570 RVA: 0x00052F13 File Offset: 0x00051113
	private void Start()
	{
		if (this.source == null)
		{
			this.source = base.transform;
		}
	}

	// Token: 0x06003503 RID: 13571 RVA: 0x0013F8DC File Offset: 0x0013DADC
	private void Update()
	{
		float d = (this.source.position.x * this.source.position.x + this.source.position.y * this.source.position.y + this.source.position.z * this.source.position.z) % 1f;
		base.transform.Rotate(this.rot * d);
	}

	// Token: 0x040037DD RID: 14301
	[SerializeField]
	private Vector3 rot = Vector3.zero;

	// Token: 0x040037DE RID: 14302
	[SerializeField]
	private Transform source;
}
