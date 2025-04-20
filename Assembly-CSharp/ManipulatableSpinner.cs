using System;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020003F9 RID: 1017
[RequireComponent(typeof(BezierSpline))]
public class ManipulatableSpinner : ManipulatableObject
{
	// Token: 0x170002BD RID: 701
	// (get) Token: 0x060018DA RID: 6362 RVA: 0x00040D2F File Offset: 0x0003EF2F
	// (set) Token: 0x060018DB RID: 6363 RVA: 0x00040D37 File Offset: 0x0003EF37
	public float angle { get; private set; }

	// Token: 0x060018DC RID: 6364 RVA: 0x00040D40 File Offset: 0x0003EF40
	private void Awake()
	{
		this.spline = base.GetComponent<BezierSpline>();
	}

	// Token: 0x060018DD RID: 6365 RVA: 0x000CD9E4 File Offset: 0x000CBBE4
	protected override void OnStartManipulation(GameObject grabbingHand)
	{
		Vector3 position = grabbingHand.transform.position;
		float num = this.FindPositionOnSpline(position);
		this.previousHandT = num;
	}

	// Token: 0x060018DE RID: 6366 RVA: 0x00030607 File Offset: 0x0002E807
	protected override void OnStopManipulation(GameObject releasingHand, Vector3 releaseVelocity)
	{
	}

	// Token: 0x060018DF RID: 6367 RVA: 0x000CDA0C File Offset: 0x000CBC0C
	protected override bool ShouldHandDetach(GameObject hand)
	{
		if (!this.spline.Loop && (this.currentHandT >= 0.99f || this.currentHandT <= 0.01f))
		{
			return true;
		}
		Vector3 position = hand.transform.position;
		Vector3 point = this.spline.GetPoint(this.currentHandT);
		return Vector3.SqrMagnitude(position - point) > this.breakDistance * this.breakDistance;
	}

	// Token: 0x060018E0 RID: 6368 RVA: 0x000CDA7C File Offset: 0x000CBC7C
	protected override void OnHeldUpdate(GameObject hand)
	{
		float angle = this.angle;
		Vector3 position = hand.transform.position;
		this.currentHandT = this.FindPositionOnSpline(position);
		float num = this.currentHandT - this.previousHandT;
		if (this.spline.Loop)
		{
			if (num > 0.5f)
			{
				num -= 1f;
			}
			else if (num < -0.5f)
			{
				num += 1f;
			}
		}
		this.angle += num;
		this.previousHandT = this.currentHandT;
		if (this.applyReleaseVelocity && this.currentHandT <= 0.99f && this.currentHandT >= 0.01f)
		{
			this.tVelocity = (this.angle - angle) / Time.deltaTime;
		}
	}

	// Token: 0x060018E1 RID: 6369 RVA: 0x000CDB38 File Offset: 0x000CBD38
	protected override void OnReleasedUpdate()
	{
		if (this.tVelocity != 0f)
		{
			this.angle += this.tVelocity * Time.deltaTime;
			if (Mathf.Abs(this.tVelocity) < this.lowSpeedThreshold)
			{
				this.tVelocity *= 1f - this.lowSpeedDrag * Time.deltaTime;
				return;
			}
			this.tVelocity *= 1f - this.releaseDrag * Time.deltaTime;
		}
	}

	// Token: 0x060018E2 RID: 6370 RVA: 0x000CDBC0 File Offset: 0x000CBDC0
	private float FindPositionOnSpline(Vector3 grabPoint)
	{
		int i = 0;
		int num = 200;
		float num2 = 0.001f;
		float num3 = 1f / (float)num;
		float3 y = base.transform.InverseTransformPoint(grabPoint);
		float result = 0f;
		float num4 = float.PositiveInfinity;
		while (i < num)
		{
			float num5 = math.distancesq(this.spline.GetPointLocal(num2), y);
			if (num5 < num4)
			{
				num4 = num5;
				result = num2;
			}
			num2 += num3;
			i++;
		}
		return result;
	}

	// Token: 0x060018E3 RID: 6371 RVA: 0x00040D4E File Offset: 0x0003EF4E
	public void SetAngle(float newAngle)
	{
		this.angle = newAngle;
	}

	// Token: 0x060018E4 RID: 6372 RVA: 0x00040D57 File Offset: 0x0003EF57
	public void SetVelocity(float newVelocity)
	{
		this.tVelocity = newVelocity;
	}

	// Token: 0x04001B79 RID: 7033
	public float breakDistance = 0.2f;

	// Token: 0x04001B7A RID: 7034
	public bool applyReleaseVelocity;

	// Token: 0x04001B7B RID: 7035
	public float releaseDrag = 1f;

	// Token: 0x04001B7C RID: 7036
	public float lowSpeedThreshold = 0.12f;

	// Token: 0x04001B7D RID: 7037
	public float lowSpeedDrag = 3f;

	// Token: 0x04001B7E RID: 7038
	private BezierSpline spline;

	// Token: 0x04001B7F RID: 7039
	private float previousHandT;

	// Token: 0x04001B80 RID: 7040
	private float currentHandT;

	// Token: 0x04001B81 RID: 7041
	private float tVelocity;
}
