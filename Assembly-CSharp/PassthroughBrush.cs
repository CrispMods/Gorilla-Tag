using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000325 RID: 805
public class PassthroughBrush : MonoBehaviour
{
	// Token: 0x06001328 RID: 4904 RVA: 0x0003D082 File Offset: 0x0003B282
	private void OnDisable()
	{
		this.brushStatus = PassthroughBrush.BrushState.Idle;
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x000B6394 File Offset: 0x000B4594
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

	// Token: 0x0600132A RID: 4906 RVA: 0x000B6454 File Offset: 0x000B4654
	private void StartLine(Vector3 inkPos)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.lineSegmentPrefab, inkPos, Quaternion.identity);
		this.currentLineSegment = gameObject.GetComponent<LineRenderer>();
		this.currentLineSegment.positionCount = 1;
		this.currentLineSegment.SetPosition(0, inkPos);
		this.strokeWidth = this.currentLineSegment.startWidth;
		this.strokeLength = 0f;
		this.inkPositions.Clear();
		this.inkPositions.Add(inkPos);
		gameObject.transform.parent = this.lineContainer.transform;
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x000B64E4 File Offset: 0x000B46E4
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

	// Token: 0x0600132C RID: 4908 RVA: 0x000B658C File Offset: 0x000B478C
	public void ClearLines()
	{
		for (int i = 0; i < this.lineContainer.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.lineContainer.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x0600132D RID: 4909 RVA: 0x000B65D0 File Offset: 0x000B47D0
	public void UndoInkLine()
	{
		if (this.lineContainer.transform.childCount >= 1)
		{
			UnityEngine.Object.Destroy(this.lineContainer.transform.GetChild(this.lineContainer.transform.childCount - 1).gameObject);
		}
	}

	// Token: 0x04001523 RID: 5411
	public OVRInput.Controller controllerHand;

	// Token: 0x04001524 RID: 5412
	public GameObject lineSegmentPrefab;

	// Token: 0x04001525 RID: 5413
	public GameObject lineContainer;

	// Token: 0x04001526 RID: 5414
	public bool forceActive = true;

	// Token: 0x04001527 RID: 5415
	private LineRenderer currentLineSegment;

	// Token: 0x04001528 RID: 5416
	private List<Vector3> inkPositions = new List<Vector3>();

	// Token: 0x04001529 RID: 5417
	private float minInkDist = 0.01f;

	// Token: 0x0400152A RID: 5418
	private float strokeWidth = 0.1f;

	// Token: 0x0400152B RID: 5419
	private float strokeLength;

	// Token: 0x0400152C RID: 5420
	private PassthroughBrush.BrushState brushStatus;

	// Token: 0x02000326 RID: 806
	public enum BrushState
	{
		// Token: 0x0400152E RID: 5422
		Idle,
		// Token: 0x0400152F RID: 5423
		Inking
	}
}
