using System;
using UnityEngine;

// Token: 0x02000247 RID: 583
public class LineRendererDraw : MonoBehaviour
{
	// Token: 0x06000D7A RID: 3450 RVA: 0x00045686 File Offset: 0x00043886
	public void SetUpLine(Transform[] points)
	{
		this.lr.positionCount = points.Length;
		this.points = points;
	}

	// Token: 0x06000D7B RID: 3451 RVA: 0x000456A0 File Offset: 0x000438A0
	private void LateUpdate()
	{
		for (int i = 0; i < this.points.Length; i++)
		{
			this.lr.SetPosition(i, this.points[i].position);
		}
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x000456D9 File Offset: 0x000438D9
	public void Enable(bool enable)
	{
		this.lr.enabled = enable;
	}

	// Token: 0x040010A5 RID: 4261
	public LineRenderer lr;

	// Token: 0x040010A6 RID: 4262
	public Transform[] points;
}
