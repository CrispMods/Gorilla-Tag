using System;
using UnityEngine;

// Token: 0x020000BE RID: 190
public class SpiderDangler : MonoBehaviour
{
	// Token: 0x060004EF RID: 1263 RVA: 0x0001D584 File Offset: 0x0001B784
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

	// Token: 0x060004F0 RID: 1264 RVA: 0x0001D60B File Offset: 0x0001B80B
	protected void FixedUpdate()
	{
		this.Simulate();
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x0001D614 File Offset: 0x0001B814
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

	// Token: 0x060004F2 RID: 1266 RVA: 0x0001D718 File Offset: 0x0001B918
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

	// Token: 0x060004F3 RID: 1267 RVA: 0x0001D820 File Offset: 0x0001BA20
	private void ApplyConstraint()
	{
		this.ropeSegs[0].pos = base.transform.position;
		this.ApplyConstraintSegment(ref this.ropeSegs[0], ref this.ropeSegs[1], 0f, 1f);
		for (int i = 1; i < 5; i++)
		{
			this.ApplyConstraintSegment(ref this.ropeSegs[i], ref this.ropeSegs[i + 1], 0.5f, 0.5f);
		}
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x0001D8A8 File Offset: 0x0001BAA8
	private void ApplyConstraintSegment(ref SpiderDangler.RopeSegment segA, ref SpiderDangler.RopeSegment segB, float dampenA, float dampenB)
	{
		float d = (segA.pos - segB.pos).magnitude - this.ropeSegLenScaled;
		Vector3 a = (segA.pos - segB.pos).normalized * d;
		segA.pos -= a * dampenA;
		segB.pos += a * dampenB;
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x0001D934 File Offset: 0x0001BB34
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

	// Token: 0x040005C1 RID: 1473
	public Transform endTransform;

	// Token: 0x040005C2 RID: 1474
	public Vector4 spinSpeeds = new Vector4(0.1f, 0.2f, 0.3f, 0.4f);

	// Token: 0x040005C3 RID: 1475
	public Vector4 spinScales = new Vector4(180f, 90f, 120f, 180f);

	// Token: 0x040005C4 RID: 1476
	private LineRenderer lineRenderer;

	// Token: 0x040005C5 RID: 1477
	private SpiderDangler.RopeSegment[] ropeSegs;

	// Token: 0x040005C6 RID: 1478
	private float ropeSegLen;

	// Token: 0x040005C7 RID: 1479
	private float ropeSegLenScaled;

	// Token: 0x040005C8 RID: 1480
	private const int kSegmentCount = 6;

	// Token: 0x040005C9 RID: 1481
	private const float kVelocityDamper = 0.95f;

	// Token: 0x040005CA RID: 1482
	private const int kConstraintCalculationIterations = 8;

	// Token: 0x020000BF RID: 191
	public struct RopeSegment
	{
		// Token: 0x060004F7 RID: 1271 RVA: 0x0001D9D9 File Offset: 0x0001BBD9
		public RopeSegment(Vector3 pos)
		{
			this.pos = pos;
			this.posOld = pos;
		}

		// Token: 0x040005CB RID: 1483
		public Vector3 pos;

		// Token: 0x040005CC RID: 1484
		public Vector3 posOld;
	}
}
