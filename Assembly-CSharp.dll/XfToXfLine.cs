using System;
using UnityEngine;

// Token: 0x020005ED RID: 1517
public class XfToXfLine : MonoBehaviour
{
	// Token: 0x060025C3 RID: 9667 RVA: 0x00048A43 File Offset: 0x00046C43
	private void Awake()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
	}

	// Token: 0x060025C4 RID: 9668 RVA: 0x00048A51 File Offset: 0x00046C51
	private void Update()
	{
		this.lineRenderer.SetPosition(0, this.pt0.transform.position);
		this.lineRenderer.SetPosition(1, this.pt1.transform.position);
	}

	// Token: 0x040029DF RID: 10719
	public Transform pt0;

	// Token: 0x040029E0 RID: 10720
	public Transform pt1;

	// Token: 0x040029E1 RID: 10721
	private LineRenderer lineRenderer;
}
