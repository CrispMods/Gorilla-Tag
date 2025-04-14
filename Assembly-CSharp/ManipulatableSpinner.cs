using System;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020003EE RID: 1006
[RequireComponent(typeof(BezierSpline))]
public class ManipulatableSpinner : ManipulatableObject
{
	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x0600188D RID: 6285 RVA: 0x0007767D File Offset: 0x0007587D
	// (set) Token: 0x0600188E RID: 6286 RVA: 0x00077685 File Offset: 0x00075885
	public float angle { get; private set; }

	// Token: 0x0600188F RID: 6287 RVA: 0x0007768E File Offset: 0x0007588E
	private void Awake()
	{
		this.spline = base.GetComponent<BezierSpline>();
	}

	// Token: 0x06001890 RID: 6288 RVA: 0x0007769C File Offset: 0x0007589C
	protected override void OnStartManipulation(GameObject grabbingHand)
	{
		Vector3 position = grabbingHand.transform.position;
		float num = this.FindPositionOnSpline(position);
		this.previousHandT = num;
	}

	// Token: 0x06001891 RID: 6289 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void OnStopManipulation(GameObject releasingHand, Vector3 releaseVelocity)
	{
	}

	// Token: 0x06001892 RID: 6290 RVA: 0x000776C4 File Offset: 0x000758C4
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

	// Token: 0x06001893 RID: 6291 RVA: 0x00077734 File Offset: 0x00075934
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

	// Token: 0x06001894 RID: 6292 RVA: 0x000777F0 File Offset: 0x000759F0
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

	// Token: 0x06001895 RID: 6293 RVA: 0x00077878 File Offset: 0x00075A78
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

	// Token: 0x06001896 RID: 6294 RVA: 0x000778F4 File Offset: 0x00075AF4
	public void SetAngle(float newAngle)
	{
		this.angle = newAngle;
	}

	// Token: 0x06001897 RID: 6295 RVA: 0x000778FD File Offset: 0x00075AFD
	public void SetVelocity(float newVelocity)
	{
		this.tVelocity = newVelocity;
	}

	// Token: 0x04001B30 RID: 6960
	public float breakDistance = 0.2f;

	// Token: 0x04001B31 RID: 6961
	public bool applyReleaseVelocity;

	// Token: 0x04001B32 RID: 6962
	public float releaseDrag = 1f;

	// Token: 0x04001B33 RID: 6963
	public float lowSpeedThreshold = 0.12f;

	// Token: 0x04001B34 RID: 6964
	public float lowSpeedDrag = 3f;

	// Token: 0x04001B35 RID: 6965
	private BezierSpline spline;

	// Token: 0x04001B36 RID: 6966
	private float previousHandT;

	// Token: 0x04001B37 RID: 6967
	private float currentHandT;

	// Token: 0x04001B38 RID: 6968
	private float tVelocity;
}
