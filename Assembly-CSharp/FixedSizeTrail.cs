using System;
using UnityEngine;

// Token: 0x0200051B RID: 1307
[RequireComponent(typeof(LineRenderer))]
public class FixedSizeTrail : MonoBehaviour
{
	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x0009F7AE File Offset: 0x0009D9AE
	public LineRenderer renderer
	{
		get
		{
			return this._lineRenderer;
		}
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06001FA6 RID: 8102 RVA: 0x0009F7B6 File Offset: 0x0009D9B6
	// (set) Token: 0x06001FA7 RID: 8103 RVA: 0x0009F7BE File Offset: 0x0009D9BE
	public float length
	{
		get
		{
			return this._length;
		}
		set
		{
			this._length = Math.Clamp(value, 0f, 128f);
		}
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x0009F7D6 File Offset: 0x0009D9D6
	public Vector3[] points
	{
		get
		{
			return this._points;
		}
	}

	// Token: 0x06001FA9 RID: 8105 RVA: 0x0009F7DE File Offset: 0x0009D9DE
	private void Reset()
	{
		this.Setup();
	}

	// Token: 0x06001FAA RID: 8106 RVA: 0x0009F7DE File Offset: 0x0009D9DE
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x06001FAB RID: 8107 RVA: 0x0009F7E8 File Offset: 0x0009D9E8
	private void Setup()
	{
		this._transform = base.transform;
		if (this._lineRenderer == null)
		{
			this._lineRenderer = base.GetComponent<LineRenderer>();
		}
		if (!this._lineRenderer)
		{
			return;
		}
		this._lineRenderer.useWorldSpace = true;
		Vector3 position = this._transform.position;
		Vector3 forward = this._transform.forward;
		int num = this._segments + 1;
		this._points = new Vector3[num];
		float d = this._length / (float)this._segments;
		for (int i = 0; i < num; i++)
		{
			this._points[i] = position - forward * d * (float)i;
		}
		this._lineRenderer.positionCount = num;
		this._lineRenderer.SetPositions(this._points);
		this.Update();
	}

	// Token: 0x06001FAC RID: 8108 RVA: 0x0009F8C6 File Offset: 0x0009DAC6
	private void Update()
	{
		if (!this.manualUpdate)
		{
			this.Update(Time.deltaTime);
		}
	}

	// Token: 0x06001FAD RID: 8109 RVA: 0x0009F8DC File Offset: 0x0009DADC
	private void FixedUpdate()
	{
		if (!this.applyPhysics)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		int num = this._points.Length - 1;
		float num2 = this._length / (float)num;
		for (int i = 1; i < num; i++)
		{
			float time = (float)(i - 1) / (float)num;
			float num3 = this.gravityCurve.Evaluate(time);
			Vector3 b = this.gravity * (num3 * deltaTime);
			this._points[i] += b;
			this._points[i + 1] += b;
		}
	}

	// Token: 0x06001FAE RID: 8110 RVA: 0x0009F980 File Offset: 0x0009DB80
	public void Update(float dt)
	{
		float num = this._length / (float)(this._segments - 1);
		Vector3 position = this._transform.position;
		this._points[0] = position;
		float num2 = Vector3.Distance(this._points[0], this._points[1]);
		float num3 = num - num2;
		if (num2 > num)
		{
			Array.Copy(this._points, 0, this._points, 1, this._points.Length - 1);
		}
		for (int i = 0; i < this._points.Length - 1; i++)
		{
			Vector3 vector = this._points[i];
			Vector3 vector2 = this._points[i + 1] - vector;
			if (vector2.sqrMagnitude > num * num)
			{
				this._points[i + 1] = vector + vector2.normalized * num;
			}
		}
		if (num3 > 0f)
		{
			int num4 = this._points.Length - 1;
			int num5 = num4 - 1;
			Vector3 vector3 = this._points[num4] - this._points[num5];
			Vector3 a = vector3.normalized;
			if (this.applyPhysics)
			{
				Vector3 normalized = (this._points[num5] - this._points[num5 - 1]).normalized;
				a = Vector3.Lerp(a, normalized, 0.5f);
			}
			this._points[num4] = this._points[num5] + a * Math.Min(vector3.magnitude, num3);
		}
		this._lineRenderer.SetPositions(this._points);
	}

	// Token: 0x06001FAF RID: 8111 RVA: 0x0009FB38 File Offset: 0x0009DD38
	private static float CalcLength(in Vector3[] positions)
	{
		float num = 0f;
		for (int i = 0; i < positions.Length - 1; i++)
		{
			num += Vector3.Distance(positions[i], positions[i + 1]);
		}
		return num;
	}

	// Token: 0x0400239E RID: 9118
	[SerializeField]
	private Transform _transform;

	// Token: 0x0400239F RID: 9119
	[SerializeField]
	private LineRenderer _lineRenderer;

	// Token: 0x040023A0 RID: 9120
	[SerializeField]
	[Range(1f, 128f)]
	private int _segments = 8;

	// Token: 0x040023A1 RID: 9121
	[SerializeField]
	private float _length = 8f;

	// Token: 0x040023A2 RID: 9122
	public bool manualUpdate;

	// Token: 0x040023A3 RID: 9123
	[Space]
	public bool applyPhysics;

	// Token: 0x040023A4 RID: 9124
	public Vector3 gravity = new Vector3(0f, -9.8f, 0f);

	// Token: 0x040023A5 RID: 9125
	public AnimationCurve gravityCurve = AnimationCurves.EaseInCubic;

	// Token: 0x040023A6 RID: 9126
	[Space]
	private Vector3[] _points = new Vector3[8];
}
