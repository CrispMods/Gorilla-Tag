using System;
using UnityEngine;

// Token: 0x020004C5 RID: 1221
public class BuilderLaserSight : MonoBehaviour
{
	// Token: 0x06001DAA RID: 7594 RVA: 0x000915BD File Offset: 0x0008F7BD
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

	// Token: 0x06001DAB RID: 7595 RVA: 0x000915F3 File Offset: 0x0008F7F3
	public void SetPoints(Vector3 start, Vector3 end)
	{
		this.lineRenderer.positionCount = 2;
		this.lineRenderer.SetPosition(0, start);
		this.lineRenderer.SetPosition(1, end);
	}

	// Token: 0x06001DAC RID: 7596 RVA: 0x0009161B File Offset: 0x0008F81B
	public void Show(bool show)
	{
		if (this.lineRenderer != null)
		{
			this.lineRenderer.enabled = show;
		}
	}

	// Token: 0x040020CD RID: 8397
	public LineRenderer lineRenderer;
}
