using System;
using UnityEngine;

// Token: 0x02000528 RID: 1320
[RequireComponent(typeof(LineRenderer))]
public class FixedSizeTrail : MonoBehaviour
{
	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06001FFE RID: 8190 RVA: 0x00045C76 File Offset: 0x00043E76
	public LineRenderer renderer
	{
		get
		{
			return this._lineRenderer;
		}
	}

	// Token: 0x1700033E RID: 830
	// (get) Token: 0x06001FFF RID: 8191 RVA: 0x00045C7E File Offset: 0x00043E7E
	// (set) Token: 0x06002000 RID: 8192 RVA: 0x00045C86 File Offset: 0x00043E86
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

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x06002001 RID: 8193 RVA: 0x00045C9E File Offset: 0x00043E9E
	public Vector3[] points
	{
		get
		{
			return this._points;
		}
	}

	// Token: 0x06002002 RID: 8194 RVA: 0x00045CA6 File Offset: 0x00043EA6
	private void Reset()
	{
		this.Setup();
	}

	// Token: 0x06002003 RID: 8195 RVA: 0x00045CA6 File Offset: 0x00043EA6
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x06002004 RID: 8196 RVA: 0x000F10D8 File Offset: 0x000EF2D8
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

	// Token: 0x06002005 RID: 8197 RVA: 0x00045CAE File Offset: 0x00043EAE
	private void Update()
	{
		if (!this.manualUpdate)
		{
			this.Update(Time.deltaTime);
		}
	}

	// Token: 0x06002006 RID: 8198 RVA: 0x000F11B8 File Offset: 0x000EF3B8
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

	// Token: 0x06002007 RID: 8199 RVA: 0x000F125C File Offset: 0x000EF45C
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

	// Token: 0x06002008 RID: 8200 RVA: 0x000F1414 File Offset: 0x000EF614
	private static float CalcLength(in Vector3[] positions)
	{
		float num = 0f;
		for (int i = 0; i < positions.Length - 1; i++)
		{
			num += Vector3.Distance(positions[i], positions[i + 1]);
		}
		return num;
	}

	// Token: 0x040023F1 RID: 9201
	[SerializeField]
	private Transform _transform;

	// Token: 0x040023F2 RID: 9202
	[SerializeField]
	private LineRenderer _lineRenderer;

	// Token: 0x040023F3 RID: 9203
	[SerializeField]
	[Range(1f, 128f)]
	private int _segments = 8;

	// Token: 0x040023F4 RID: 9204
	[SerializeField]
	private float _length = 8f;

	// Token: 0x040023F5 RID: 9205
	public bool manualUpdate;

	// Token: 0x040023F6 RID: 9206
	[Space]
	public bool applyPhysics;

	// Token: 0x040023F7 RID: 9207
	public Vector3 gravity = new Vector3(0f, -9.8f, 0f);

	// Token: 0x040023F8 RID: 9208
	public AnimationCurve gravityCurve = AnimationCurves.EaseInCubic;

	// Token: 0x040023F9 RID: 9209
	[Space]
	private Vector3[] _points = new Vector3[8];
}
