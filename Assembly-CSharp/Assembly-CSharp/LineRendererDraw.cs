using System;
using UnityEngine;

// Token: 0x02000247 RID: 583
public class LineRendererDraw : MonoBehaviour
{
	// Token: 0x06000D7C RID: 3452 RVA: 0x000459CA File Offset: 0x00043BCA
	public void SetUpLine(Transform[] points)
	{
		this.lr.positionCount = points.Length;
		this.points = points;
	}

	// Token: 0x06000D7D RID: 3453 RVA: 0x000459E4 File Offset: 0x00043BE4
	private void LateUpdate()
	{
		for (int i = 0; i < this.points.Length; i++)
		{
			this.lr.SetPosition(i, this.points[i].position);
		}
	}

	// Token: 0x06000D7E RID: 3454 RVA: 0x00045A1D File Offset: 0x00043C1D
	public void Enable(bool enable)
	{
		this.lr.enabled = enable;
	}

	// Token: 0x040010A6 RID: 4262
	public LineRenderer lr;

	// Token: 0x040010A7 RID: 4263
	public Transform[] points;
}
