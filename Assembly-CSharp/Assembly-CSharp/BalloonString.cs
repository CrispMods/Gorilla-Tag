﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000374 RID: 884
public class BalloonString : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x0600149B RID: 5275 RVA: 0x00065650 File Offset: 0x00063850
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

	// Token: 0x0600149C RID: 5276 RVA: 0x00065720 File Offset: 0x00063920
	private void UpdateDynamics()
	{
		this.vertices[0] = this.startPositionXf.position;
		this.vertices[this.vertices.Count - 1] = this.endPositionXf.position;
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x0006575C File Offset: 0x0006395C
	private void UpdateRenderPositions()
	{
		this.lineRenderer.SetPosition(0, this.startPositionXf.transform.position);
		this.lineRenderer.SetPosition(1, this.endPositionXf.transform.position);
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x0000FC06 File Offset: 0x0000DE06
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x0000FC0F File Offset: 0x0000DE0F
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060014A0 RID: 5280 RVA: 0x00065796 File Offset: 0x00063996
	public void SliceUpdate()
	{
		if (this.startPositionXf != null && this.endPositionXf != null)
		{
			this.UpdateDynamics();
			this.UpdateRenderPositions();
		}
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x0000FD18 File Offset: 0x0000DF18
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
