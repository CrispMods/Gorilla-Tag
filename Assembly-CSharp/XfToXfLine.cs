using System;
using UnityEngine;

// Token: 0x020005EC RID: 1516
public class XfToXfLine : MonoBehaviour
{
	// Token: 0x060025BB RID: 9659 RVA: 0x000BA5A2 File Offset: 0x000B87A2
	private void Awake()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
	}

	// Token: 0x060025BC RID: 9660 RVA: 0x000BA5B0 File Offset: 0x000B87B0
	private void Update()
	{
		this.lineRenderer.SetPosition(0, this.pt0.transform.position);
		this.lineRenderer.SetPosition(1, this.pt1.transform.position);
	}

	// Token: 0x040029D9 RID: 10713
	public Transform pt0;

	// Token: 0x040029DA RID: 10714
	public Transform pt1;

	// Token: 0x040029DB RID: 10715
	private LineRenderer lineRenderer;
}
