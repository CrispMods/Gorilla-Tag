using System;
using UnityEngine;

// Token: 0x0200051B RID: 1307
[RequireComponent(typeof(LineRenderer))]
public class FixedSizeTrail : MonoBehaviour
{
	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x000448D7 File Offset: 0x00042AD7
	public LineRenderer renderer
	{
		get
		{
			return this._lineRenderer;
		}
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x000448DF File Offset: 0x00042ADF
	// (set) Token: 0x06001FAA RID: 8106 RVA: 0x000448E7 File Offset: 0x00042AE7
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
	// (get) Token: 0x06001FAB RID: 8107 RVA: 0x000448FF File Offset: 0x00042AFF
	public Vector3[] points
	{
		get
		{
			return this._points;
		}
	}

	// Token: 0x06001FAC RID: 8108 RVA: 0x00044907 File Offset: 0x00042B07
	private void Reset()
	{
		this.Setup();
	}

	// Token: 0x06001FAD RID: 8109 RVA: 0x00044907 File Offset: 0x00042B07
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x06001FAE RID: 8110 RVA: 0x000EE354 File Offset: 0x000EC554
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

	// Token: 0x06001FAF RID: 8111 RVA: 0x0004490F File Offset: 0x00042B0F
	private void Update()
	{
		if (!this.manualUpdate)
		{
			this.Update(Time.deltaTime);
		}
	}

	// Token: 0x06001FB0 RID: 8112 RVA: 0x000EE434 File Offset: 0x000EC634
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

	// Token: 0x06001FB1 RID: 8113 RVA: 0x000EE4D8 File Offset: 0x000EC6D8
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

	// Token: 0x06001FB2 RID: 8114 RVA: 0x000EE690 File Offset: 0x000EC890
	private static float CalcLength(in Vector3[] positions)
	{
		float num = 0f;
		for (int i = 0; i < positions.Length - 1; i++)
		{
			num += Vector3.Distance(positions[i], positions[i + 1]);
		}
		return num;
	}

	// Token: 0x0400239F RID: 9119
	[SerializeField]
	private Transform _transform;

	// Token: 0x040023A0 RID: 9120
	[SerializeField]
	private LineRenderer _lineRenderer;

	// Token: 0x040023A1 RID: 9121
	[SerializeField]
	[Range(1f, 128f)]
	private int _segments = 8;

	// Token: 0x040023A2 RID: 9122
	[SerializeField]
	private float _length = 8f;

	// Token: 0x040023A3 RID: 9123
	public bool manualUpdate;

	// Token: 0x040023A4 RID: 9124
	[Space]
	public bool applyPhysics;

	// Token: 0x040023A5 RID: 9125
	public Vector3 gravity = new Vector3(0f, -9.8f, 0f);

	// Token: 0x040023A6 RID: 9126
	public AnimationCurve gravityCurve = AnimationCurves.EaseInCubic;

	// Token: 0x040023A7 RID: 9127
	[Space]
	private Vector3[] _points = new Vector3[8];
}
