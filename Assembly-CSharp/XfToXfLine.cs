using System;
using UnityEngine;

// Token: 0x020005FA RID: 1530
public class XfToXfLine : MonoBehaviour
{
	// Token: 0x0600261D RID: 9757 RVA: 0x00049E1A File Offset: 0x0004801A
	private void Awake()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
	}

	// Token: 0x0600261E RID: 9758 RVA: 0x00049E28 File Offset: 0x00048028
	private void Update()
	{
		this.lineRenderer.SetPosition(0, this.pt0.transform.position);
		this.lineRenderer.SetPosition(1, this.pt1.transform.position);
	}

	// Token: 0x04002A38 RID: 10808
	public Transform pt0;

	// Token: 0x04002A39 RID: 10809
	public Transform pt1;

	// Token: 0x04002A3A RID: 10810
	private LineRenderer lineRenderer;
}
