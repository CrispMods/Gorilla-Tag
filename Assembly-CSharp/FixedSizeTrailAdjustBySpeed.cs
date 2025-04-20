using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000529 RID: 1321
public class FixedSizeTrailAdjustBySpeed : MonoBehaviour
{
	// Token: 0x0600200A RID: 8202 RVA: 0x00045CC3 File Offset: 0x00043EC3
	private void Start()
	{
		this.Setup();
	}

	// Token: 0x0600200B RID: 8203 RVA: 0x000F14AC File Offset: 0x000EF6AC
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

	// Token: 0x0600200C RID: 8204 RVA: 0x000F1528 File Offset: 0x000EF728
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

	// Token: 0x0600200D RID: 8205 RVA: 0x000F15C8 File Offset: 0x000EF7C8
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

	// Token: 0x0600200E RID: 8206 RVA: 0x000F16C0 File Offset: 0x000EF8C0
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

	// Token: 0x040023FA RID: 9210
	public FixedSizeTrail trail;

	// Token: 0x040023FB RID: 9211
	public bool adjustPhysics = true;

	// Token: 0x040023FC RID: 9212
	private Vector3 _rawVelocity;

	// Token: 0x040023FD RID: 9213
	private float _rawSpeed;

	// Token: 0x040023FE RID: 9214
	private float _speed;

	// Token: 0x040023FF RID: 9215
	private float _lastSpeed;

	// Token: 0x04002400 RID: 9216
	private Vector3 _lastPosition;

	// Token: 0x04002401 RID: 9217
	private Vector3 _initGravity;

	// Token: 0x04002402 RID: 9218
	public Vector3 gravityOffset = Vector3.zero;

	// Token: 0x04002403 RID: 9219
	[Space]
	public float retractMin = 0.5f;

	// Token: 0x04002404 RID: 9220
	[Space]
	[FormerlySerializedAs("sizeIncreaseSpeed")]
	public float expandSpeed = 16f;

	// Token: 0x04002405 RID: 9221
	[FormerlySerializedAs("sizeDecreaseSpeed")]
	public float retractSpeed = 4f;

	// Token: 0x04002406 RID: 9222
	[Space]
	public float minSpeed;

	// Token: 0x04002407 RID: 9223
	public float minLength = 1f;

	// Token: 0x04002408 RID: 9224
	public Gradient minColors = GradientHelper.FromColor(new Color(0f, 1f, 1f, 1f));

	// Token: 0x04002409 RID: 9225
	[Space]
	public float maxSpeed = 10f;

	// Token: 0x0400240A RID: 9226
	public float maxLength = 8f;

	// Token: 0x0400240B RID: 9227
	public Gradient maxColors = GradientHelper.FromColor(new Color(1f, 1f, 0f, 1f));

	// Token: 0x0400240C RID: 9228
	[Space]
	[SerializeField]
	private Gradient _mixGradient = new Gradient
	{
		colorKeys = new GradientColorKey[8],
		alphaKeys = Array.Empty<GradientAlphaKey>()
	};

	// Token: 0x0200052A RID: 1322
	[Serializable]
	public struct GradientKey
	{
		// Token: 0x06002010 RID: 8208 RVA: 0x00045CCB File Offset: 0x00043ECB
		public GradientKey(Color color, float time)
		{
			this.color = color;
			this.time = time;
		}

		// Token: 0x0400240D RID: 9229
		public Color color;

		// Token: 0x0400240E RID: 9230
		public float time;
	}
}
