using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200031A RID: 794
public class PassthroughBrush : MonoBehaviour
{
	// Token: 0x060012DF RID: 4831 RVA: 0x0005C670 File Offset: 0x0005A870
	private void OnDisable()
	{
		this.brushStatus = PassthroughBrush.BrushState.Idle;
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x0005C67C File Offset: 0x0005A87C
	private void LateUpdate()
	{
		base.transform.rotation = Quaternion.LookRotation(base.transform.position - Camera.main.transform.position);
		if (this.controllerHand != OVRInput.Controller.LTouch && this.controllerHand != OVRInput.Controller.RTouch)
		{
			return;
		}
		Vector3 position = base.transform.position;
		PassthroughBrush.BrushState brushState = this.brushStatus;
		if (brushState != PassthroughBrush.BrushState.Idle)
		{
			if (brushState != PassthroughBrush.BrushState.Inking)
			{
				return;
			}
			this.UpdateLine(position);
			if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, this.controllerHand))
			{
				this.brushStatus = PassthroughBrush.BrushState.Idle;
			}
		}
		else
		{
			if (OVRInput.GetUp(OVRInput.Button.One, this.controllerHand))
			{
				this.UndoInkLine();
			}
			if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, this.controllerHand))
			{
				this.StartLine(position);
				this.brushStatus = PassthroughBrush.BrushState.Inking;
				return;
			}
		}
	}

	// Token: 0x060012E1 RID: 4833 RVA: 0x0005C73C File Offset: 0x0005A93C
	private void StartLine(Vector3 inkPos)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.lineSegmentPrefab, inkPos, Quaternion.identity);
		this.currentLineSegment = gameObject.GetComponent<LineRenderer>();
		this.currentLineSegment.positionCount = 1;
		this.currentLineSegment.SetPosition(0, inkPos);
		this.strokeWidth = this.currentLineSegment.startWidth;
		this.strokeLength = 0f;
		this.inkPositions.Clear();
		this.inkPositions.Add(inkPos);
		gameObject.transform.parent = this.lineContainer.transform;
	}

	// Token: 0x060012E2 RID: 4834 RVA: 0x0005C7CC File Offset: 0x0005A9CC
	private void UpdateLine(Vector3 inkPos)
	{
		float magnitude = (inkPos - this.inkPositions[this.inkPositions.Count - 1]).magnitude;
		if (magnitude >= this.minInkDist)
		{
			this.inkPositions.Add(inkPos);
			this.currentLineSegment.positionCount = this.inkPositions.Count;
			this.currentLineSegment.SetPositions(this.inkPositions.ToArray());
			this.strokeLength += magnitude;
			this.currentLineSegment.material.SetFloat("_LineLength", this.strokeLength / this.strokeWidth);
		}
	}

	// Token: 0x060012E3 RID: 4835 RVA: 0x0005C874 File Offset: 0x0005AA74
	public void ClearLines()
	{
		for (int i = 0; i < this.lineContainer.transform.childCount; i++)
		{
			Object.Destroy(this.lineContainer.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x060012E4 RID: 4836 RVA: 0x0005C8B8 File Offset: 0x0005AAB8
	public void UndoInkLine()
	{
		if (this.lineContainer.transform.childCount >= 1)
		{
			Object.Destroy(this.lineContainer.transform.GetChild(this.lineContainer.transform.childCount - 1).gameObject);
		}
	}

	// Token: 0x040014DC RID: 5340
	public OVRInput.Controller controllerHand;

	// Token: 0x040014DD RID: 5341
	public GameObject lineSegmentPrefab;

	// Token: 0x040014DE RID: 5342
	public GameObject lineContainer;

	// Token: 0x040014DF RID: 5343
	public bool forceActive = true;

	// Token: 0x040014E0 RID: 5344
	private LineRenderer currentLineSegment;

	// Token: 0x040014E1 RID: 5345
	private List<Vector3> inkPositions = new List<Vector3>();

	// Token: 0x040014E2 RID: 5346
	private float minInkDist = 0.01f;

	// Token: 0x040014E3 RID: 5347
	private float strokeWidth = 0.1f;

	// Token: 0x040014E4 RID: 5348
	private float strokeLength;

	// Token: 0x040014E5 RID: 5349
	private PassthroughBrush.BrushState brushStatus;

	// Token: 0x0200031B RID: 795
	public enum BrushState
	{
		// Token: 0x040014E7 RID: 5351
		Idle,
		// Token: 0x040014E8 RID: 5352
		Inking
	}
}
