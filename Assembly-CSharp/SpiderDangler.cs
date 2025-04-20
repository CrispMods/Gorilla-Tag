using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class SpiderDangler : MonoBehaviour
{
	// Token: 0x0600052B RID: 1323 RVA: 0x0008088C File Offset: 0x0007EA8C
	protected void Awake()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
		Vector3 position = base.transform.position;
		float magnitude = (this.endTransform.position - position).magnitude;
		this.ropeSegLen = magnitude / 6f;
		this.ropeSegs = new SpiderDangler.RopeSegment[6];
		for (int i = 0; i < 6; i++)
		{
			this.ropeSegs[i] = new SpiderDangler.RopeSegment(position);
			position.y -= this.ropeSegLen;
		}
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00033D91 File Offset: 0x00031F91
	protected void FixedUpdate()
	{
		this.Simulate();
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x00080914 File Offset: 0x0007EB14
	protected void LateUpdate()
	{
		this.DrawRope();
		Vector3 normalized = (this.ropeSegs[this.ropeSegs.Length - 2].pos - this.ropeSegs[this.ropeSegs.Length - 1].pos).normalized;
		this.endTransform.position = this.ropeSegs[this.ropeSegs.Length - 1].pos;
		this.endTransform.up = normalized;
		Vector4 vector = this.spinSpeeds * Time.time;
		vector = new Vector4(Mathf.Sin(vector.x), Mathf.Sin(vector.y), Mathf.Sin(vector.z), Mathf.Sin(vector.w));
		vector.Scale(this.spinScales);
		this.endTransform.Rotate(Vector3.up, vector.x + vector.y + vector.z + vector.w);
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x00080A18 File Offset: 0x0007EC18
	private void Simulate()
	{
		this.ropeSegLenScaled = this.ropeSegLen * base.transform.lossyScale.x;
		Vector3 b = new Vector3(0f, -0.5f, 0f) * Time.fixedDeltaTime;
		for (int i = 1; i < 6; i++)
		{
			Vector3 a = this.ropeSegs[i].pos - this.ropeSegs[i].posOld;
			this.ropeSegs[i].posOld = this.ropeSegs[i].pos;
			SpiderDangler.RopeSegment[] array = this.ropeSegs;
			int num = i;
			array[num].pos = array[num].pos + a * 0.95f;
			SpiderDangler.RopeSegment[] array2 = this.ropeSegs;
			int num2 = i;
			array2[num2].pos = array2[num2].pos + b;
		}
		for (int j = 0; j < 8; j++)
		{
			this.ApplyConstraint();
		}
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x00080B20 File Offset: 0x0007ED20
	private void ApplyConstraint()
	{
		this.ropeSegs[0].pos = base.transform.position;
		this.ApplyConstraintSegment(ref this.ropeSegs[0], ref this.ropeSegs[1], 0f, 1f);
		for (int i = 1; i < 5; i++)
		{
			this.ApplyConstraintSegment(ref this.ropeSegs[i], ref this.ropeSegs[i + 1], 0.5f, 0.5f);
		}
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00080BA8 File Offset: 0x0007EDA8
	private void ApplyConstraintSegment(ref SpiderDangler.RopeSegment segA, ref SpiderDangler.RopeSegment segB, float dampenA, float dampenB)
	{
		float d = (segA.pos - segB.pos).magnitude - this.ropeSegLenScaled;
		Vector3 a = (segA.pos - segB.pos).normalized * d;
		segA.pos -= a * dampenA;
		segB.pos += a * dampenB;
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x00080C34 File Offset: 0x0007EE34
	private void DrawRope()
	{
		Vector3[] array = new Vector3[6];
		for (int i = 0; i < 6; i++)
		{
			array[i] = this.ropeSegs[i].pos;
		}
		this.lineRenderer.positionCount = array.Length;
		this.lineRenderer.SetPositions(array);
	}

	// Token: 0x04000601 RID: 1537
	public Transform endTransform;

	// Token: 0x04000602 RID: 1538
	public Vector4 spinSpeeds = new Vector4(0.1f, 0.2f, 0.3f, 0.4f);

	// Token: 0x04000603 RID: 1539
	public Vector4 spinScales = new Vector4(180f, 90f, 120f, 180f);

	// Token: 0x04000604 RID: 1540
	private LineRenderer lineRenderer;

	// Token: 0x04000605 RID: 1541
	private SpiderDangler.RopeSegment[] ropeSegs;

	// Token: 0x04000606 RID: 1542
	private float ropeSegLen;

	// Token: 0x04000607 RID: 1543
	private float ropeSegLenScaled;

	// Token: 0x04000608 RID: 1544
	private const int kSegmentCount = 6;

	// Token: 0x04000609 RID: 1545
	private const float kVelocityDamper = 0.95f;

	// Token: 0x0400060A RID: 1546
	private const int kConstraintCalculationIterations = 8;

	// Token: 0x020000C9 RID: 201
	public struct RopeSegment
	{
		// Token: 0x06000533 RID: 1331 RVA: 0x00033D99 File Offset: 0x00031F99
		public RopeSegment(Vector3 pos)
		{
			this.pos = pos;
			this.posOld = pos;
		}

		// Token: 0x0400060B RID: 1547
		public Vector3 pos;

		// Token: 0x0400060C RID: 1548
		public Vector3 posOld;
	}
}
