using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200037F RID: 895
public class BalloonString : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x060014E4 RID: 5348 RVA: 0x000BE288 File Offset: 0x000BC488
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

	// Token: 0x060014E5 RID: 5349 RVA: 0x0003E0E2 File Offset: 0x0003C2E2
	private void UpdateDynamics()
	{
		this.vertices[0] = this.startPositionXf.position;
		this.vertices[this.vertices.Count - 1] = this.endPositionXf.position;
	}

	// Token: 0x060014E6 RID: 5350 RVA: 0x0003E11E File Offset: 0x0003C31E
	private void UpdateRenderPositions()
	{
		this.lineRenderer.SetPosition(0, this.startPositionXf.transform.position);
		this.lineRenderer.SetPosition(1, this.endPositionXf.transform.position);
	}

	// Token: 0x060014E7 RID: 5351 RVA: 0x000320BF File Offset: 0x000302BF
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060014E8 RID: 5352 RVA: 0x000320C8 File Offset: 0x000302C8
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060014E9 RID: 5353 RVA: 0x0003E158 File Offset: 0x0003C358
	public void SliceUpdate()
	{
		if (this.startPositionXf != null && this.endPositionXf != null)
		{
			this.UpdateDynamics();
			this.UpdateRenderPositions();
		}
	}

	// Token: 0x060014EB RID: 5355 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x0400171B RID: 5915
	public Transform startPositionXf;

	// Token: 0x0400171C RID: 5916
	public Transform endPositionXf;

	// Token: 0x0400171D RID: 5917
	private List<Vector3> vertices;

	// Token: 0x0400171E RID: 5918
	public int numSegments = 1;

	// Token: 0x0400171F RID: 5919
	private LineRenderer lineRenderer;
}
