using System;
using UnityEngine;

// Token: 0x020004C5 RID: 1221
public class BuilderLaserSight : MonoBehaviour
{
	// Token: 0x06001DAD RID: 7597 RVA: 0x00091941 File Offset: 0x0008FB41
	public void Awake()
	{
		if (this.lineRenderer == null)
		{
			this.lineRenderer = base.GetComponentInChildren<LineRenderer>();
		}
		if (this.lineRenderer != null)
		{
			this.lineRenderer.enabled = false;
		}
	}

	// Token: 0x06001DAE RID: 7598 RVA: 0x00091977 File Offset: 0x0008FB77
	public void SetPoints(Vector3 start, Vector3 end)
	{
		this.lineRenderer.positionCount = 2;
		this.lineRenderer.SetPosition(0, start);
		this.lineRenderer.SetPosition(1, end);
	}

	// Token: 0x06001DAF RID: 7599 RVA: 0x0009199F File Offset: 0x0008FB9F
	public void Show(bool show)
	{
		if (this.lineRenderer != null)
		{
			this.lineRenderer.enabled = show;
		}
	}

	// Token: 0x040020CE RID: 8398
	public LineRenderer lineRenderer;
}
