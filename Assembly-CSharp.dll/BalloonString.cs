using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000374 RID: 884
public class BalloonString : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x0600149B RID: 5275 RVA: 0x000BBA4C File Offset: 0x000B9C4C
	private void Awake()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
		this.vertices = new List<Vector3>(this.numSegments + 1);
		if (this.startPositionXf != null && this.endPositionXf != null)
		{
			this.vertices.Add(this.startPositionXf.position);
			int num = this.vertices.Count - 2;
			for (int i = 0; i < num; i++)
			{
				float t = (float)((i + 1) / (this.vertices.Count - 1));
				Vector3 item = Vector3.Lerp(this.startPositionXf.position, this.endPositionXf.position, t);
				this.vertices.Add(item);
			}
			this.vertices.Add(this.endPositionXf.position);
		}
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x0003CE22 File Offset: 0x0003B022
	private void UpdateDynamics()
	{
		this.vertices[0] = this.startPositionXf.position;
		this.vertices[this.vertices.Count - 1] = this.endPositionXf.position;
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x0003CE5E File Offset: 0x0003B05E
	private void UpdateRenderPositions()
	{
		this.lineRenderer.SetPosition(0, this.startPositionXf.transform.position);
		this.lineRenderer.SetPosition(1, this.endPositionXf.transform.position);
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x00030F55 File Offset: 0x0002F155
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x00030F5E File Offset: 0x0002F15E
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060014A0 RID: 5280 RVA: 0x0003CE98 File Offset: 0x0003B098
	public void SliceUpdate()
	{
		if (this.startPositionXf != null && this.endPositionXf != null)
		{
			this.UpdateDynamics();
			this.UpdateRenderPositions();
		}
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x040016D4 RID: 5844
	public Transform startPositionXf;

	// Token: 0x040016D5 RID: 5845
	public Transform endPositionXf;

	// Token: 0x040016D6 RID: 5846
	private List<Vector3> vertices;

	// Token: 0x040016D7 RID: 5847
	public int numSegments = 1;

	// Token: 0x040016D8 RID: 5848
	private LineRenderer lineRenderer;
}
