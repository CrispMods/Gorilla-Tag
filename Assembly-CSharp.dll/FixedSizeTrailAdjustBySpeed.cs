using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200051C RID: 1308
public class FixedSizeTrailAdjustBySpeed : MonoBehaviour
{
	// Token: 0x06001FB4 RID: 8116 RVA: 0x00044924 File Offset: 0x00042B24
	private void Start()
	{
		this.Setup();
	}

	// Token: 0x06001FB5 RID: 8117 RVA: 0x000EE728 File Offset: 0x000EC928
	private void Setup()
	{
		this._lastPosition = base.transform.position;
		this._rawVelocity = Vector3.zero;
		this._rawSpeed = 0f;
		this._speed = 0f;
		if (this.trail)
		{
			this._initGravity = this.trail.gravity;
			this.trail.applyPhysics = this.adjustPhysics;
		}
		this.LerpTrailColors(0.5f);
	}

	// Token: 0x06001FB6 RID: 8118 RVA: 0x000EE7A4 File Offset: 0x000EC9A4
	private void LerpTrailColors(float t = 0.5f)
	{
		GradientColorKey[] colorKeys = this._mixGradient.colorKeys;
		int num = colorKeys.Length;
		for (int i = 0; i < num; i++)
		{
			float time = (float)i / (float)(num - 1);
			Color a = this.minColors.Evaluate(time);
			Color b = this.maxColors.Evaluate(time);
			Color color = Color.Lerp(a, b, t);
			colorKeys[i].color = color;
			colorKeys[i].time = time;
		}
		this._mixGradient.colorKeys = colorKeys;
		if (this.trail)
		{
			this.trail.renderer.colorGradient = this._mixGradient;
		}
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x000EE844 File Offset: 0x000ECA44
	private void Update()
	{
		float deltaTime = Time.deltaTime;
		Vector3 position = base.transform.position;
		this._rawVelocity = (position - this._lastPosition) / deltaTime;
		this._rawSpeed = this._rawVelocity.magnitude;
		if (this._rawSpeed > this.retractMin)
		{
			this._speed += this.expandSpeed * deltaTime;
		}
		if (this._rawSpeed <= this.retractMin)
		{
			this._speed -= this.retractSpeed * deltaTime;
		}
		if (this._speed > this.maxSpeed)
		{
			this._speed = this.maxSpeed;
		}
		this._speed = Mathf.Lerp(this._lastSpeed, this._speed, 0.5f);
		if (this._speed < 0.01f)
		{
			this._speed = 0f;
		}
		this.AdjustTrail();
		this._lastSpeed = this._speed;
		this._lastPosition = position;
	}

	// Token: 0x06001FB8 RID: 8120 RVA: 0x000EE93C File Offset: 0x000ECB3C
	private void AdjustTrail()
	{
		if (!this.trail)
		{
			return;
		}
		float num = MathUtils.Linear(this._speed, this.minSpeed, this.maxSpeed, 0f, 1f);
		float length = MathUtils.Linear(num, 0f, 1f, this.minLength, this.maxLength);
		this.trail.length = length;
		this.LerpTrailColors(num);
		if (this.adjustPhysics)
		{
			Transform transform = base.transform;
			Vector3 b = transform.forward * this.gravityOffset.z + transform.right * this.gravityOffset.x + transform.up * this.gravityOffset.y;
			Vector3 b2 = (this._initGravity + b) * (1f - num);
			this.trail.gravity = Vector3.Lerp(Vector3.zero, b2, 0.5f);
		}
	}

	// Token: 0x040023A8 RID: 9128
	public FixedSizeTrail trail;

	// Token: 0x040023A9 RID: 9129
	public bool adjustPhysics = true;

	// Token: 0x040023AA RID: 9130
	private Vector3 _rawVelocity;

	// Token: 0x040023AB RID: 9131
	private float _rawSpeed;

	// Token: 0x040023AC RID: 9132
	private float _speed;

	// Token: 0x040023AD RID: 9133
	private float _lastSpeed;

	// Token: 0x040023AE RID: 9134
	private Vector3 _lastPosition;

	// Token: 0x040023AF RID: 9135
	private Vector3 _initGravity;

	// Token: 0x040023B0 RID: 9136
	public Vector3 gravityOffset = Vector3.zero;

	// Token: 0x040023B1 RID: 9137
	[Space]
	public float retractMin = 0.5f;

	// Token: 0x040023B2 RID: 9138
	[Space]
	[FormerlySerializedAs("sizeIncreaseSpeed")]
	public float expandSpeed = 16f;

	// Token: 0x040023B3 RID: 9139
	[FormerlySerializedAs("sizeDecreaseSpeed")]
	public float retractSpeed = 4f;

	// Token: 0x040023B4 RID: 9140
	[Space]
	public float minSpeed;

	// Token: 0x040023B5 RID: 9141
	public float minLength = 1f;

	// Token: 0x040023B6 RID: 9142
	public Gradient minColors = GradientHelper.FromColor(new Color(0f, 1f, 1f, 1f));

	// Token: 0x040023B7 RID: 9143
	[Space]
	public float maxSpeed = 10f;

	// Token: 0x040023B8 RID: 9144
	public float maxLength = 8f;

	// Token: 0x040023B9 RID: 9145
	public Gradient maxColors = GradientHelper.FromColor(new Color(1f, 1f, 0f, 1f));

	// Token: 0x040023BA RID: 9146
	[Space]
	[SerializeField]
	private Gradient _mixGradient = new Gradient
	{
		colorKeys = new GradientColorKey[8],
		alphaKeys = Array.Empty<GradientAlphaKey>()
	};

	// Token: 0x0200051D RID: 1309
	[Serializable]
	public struct GradientKey
	{
		// Token: 0x06001FBA RID: 8122 RVA: 0x0004492C File Offset: 0x00042B2C
		public GradientKey(Color color, float time)
		{
			this.color = color;
			this.time = time;
		}

		// Token: 0x040023BB RID: 9147
		public Color color;

		// Token: 0x040023BC RID: 9148
		public float time;
	}
}
