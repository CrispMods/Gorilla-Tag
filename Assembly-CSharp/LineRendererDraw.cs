using System;
using UnityEngine;

// Token: 0x02000252 RID: 594
public class LineRendererDraw : MonoBehaviour
{
	// Token: 0x06000DC5 RID: 3525 RVA: 0x00039DBA File Offset: 0x00037FBA
	public void SetUpLine(Transform[] points)
	{
		this.lr.positionCount = points.Length;
		this.points = points;
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x000A2A50 File Offset: 0x000A0C50
	private void LateUpdate()
	{
		for (int i = 0; i < this.points.Length; i++)
		{
			this.lr.SetPosition(i, this.points[i].position);
		}
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x00039DD1 File Offset: 0x00037FD1
	public void Enable(bool enable)
	{
		this.lr.enabled = enable;
	}

	// Token: 0x040010EB RID: 4331
	public LineRenderer lr;

	// Token: 0x040010EC RID: 4332
	public Transform[] points;
}
