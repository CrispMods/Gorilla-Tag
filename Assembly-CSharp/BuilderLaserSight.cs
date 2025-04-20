using System;
using UnityEngine;

// Token: 0x020004D2 RID: 1234
public class BuilderLaserSight : MonoBehaviour
{
	// Token: 0x06001E03 RID: 7683 RVA: 0x000447FC File Offset: 0x000429FC
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

	// Token: 0x06001E04 RID: 7684 RVA: 0x00044832 File Offset: 0x00042A32
	public void SetPoints(Vector3 start, Vector3 end)
	{
		this.lineRenderer.positionCount = 2;
		this.lineRenderer.SetPosition(0, start);
		this.lineRenderer.SetPosition(1, end);
	}

	// Token: 0x06001E05 RID: 7685 RVA: 0x0004485A File Offset: 0x00042A5A
	public void Show(bool show)
	{
		if (this.lineRenderer != null)
		{
			this.lineRenderer.enabled = show;
		}
	}

	// Token: 0x04002120 RID: 8480
	public LineRenderer lineRenderer;
}
